using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tesko.Data;
using Tesko.Hubs;
using Tesko.Models;

namespace Tesko.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly TeskoDbContext _context;
        private readonly IHubContext<DashboardHub> _hubContext;

        public RequestsController(TeskoDbContext context, IHubContext<DashboardHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            // Fallback or handle error if needed, but [Authorize] should ensure we have a user
            return 0; 
        }

        // GET: Requests (My Requests)
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var requests = await _context.Requests
                .Include(r => r.Asset)
                .Include(r => r.Approver)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
            return View(requests);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Name");
            return View();
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssetId,Quantity,Purpose")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.UserId = GetCurrentUserId();
                request.Status = "Pending";
                request.RequestDate = DateTime.Now;
                
                _context.Add(request);
                
                // Log Audit
                var audit = new AuditLog
                {
                    Action = "Request",
                    UserId = request.UserId,
                    Details = $"Requested {request.Quantity} of Asset {request.AssetId}",
                    Timestamp = DateTime.Now
                };
                _context.Add(audit);
                
                await _context.SaveChangesAsync();
                
                // Notify Dashboard
                await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate");

                TempData["Success"] = "Request submitted successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Name", request.AssetId);
            return View(request);
        }

        // GET: Requests/ApprovalQueue
        public async Task<IActionResult> ApprovalQueue()
        {
            // Only for Approvers/Admins
            var requests = await _context.Requests
                .Include(r => r.Asset)
                .Include(r => r.User)
                .Where(r => r.Status == "Pending")
                .OrderBy(r => r.RequestDate)
                .ToListAsync();
            return View(requests);
        }

        // GET: Requests/History
        public async Task<IActionResult> History()
        {
            var userId = GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            IQueryable<Request> query = _context.Requests
                .Include(r => r.Asset)
                .Include(r => r.User)
                .Include(r => r.Approver)
                .Where(r => r.Status == "Approved" || r.Status == "Rejected");

            // If not Admin or Approver, restrict to own requests
            if (userRole != "Admin" && userRole != "Approver")
            {
                query = query.Where(r => r.UserId == userId);
            }

            var history = await query.OrderByDescending(r => r.ActionDate).ToListAsync();
            return View(history);
        }

        // POST: Requests/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _context.Requests.Include(r => r.Asset).FirstOrDefaultAsync(r => r.Id == id);
            if (request == null) return NotFound();

            var asset = request.Asset;
            if (asset.AvailableStock < request.Quantity)
            {
                TempData["Error"] = "Not enough stock to approve this request.";
                return RedirectToAction(nameof(ApprovalQueue));
            }

            // Deduct stock
            asset.AvailableStock -= request.Quantity;
            asset.AllocatedStock += request.Quantity;
            _context.Update(asset);

            // Update Request
            request.Status = "Approved";
            request.ApproverId = GetCurrentUserId();
            request.ActionDate = DateTime.Now;
            _context.Update(request);

            // Log Audit
            var audit = new AuditLog
            {
                Action = "Approve",
                UserId = GetCurrentUserId(),
                Details = $"Approved Request {request.Id} for {request.Quantity} of {asset.Name}",
                Timestamp = DateTime.Now
            };
            _context.Add(audit);

            await _context.SaveChangesAsync();
            
            // Notify Dashboard
            await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate");

            return RedirectToAction(nameof(ApprovalQueue));
        }

        // POST: Requests/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string comment)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null) return NotFound();

            request.Status = "Rejected";
            request.ApproverId = GetCurrentUserId();
            request.ActionDate = DateTime.Now;
            request.Comment = comment;
            _context.Update(request);

            // Log Audit
            var audit = new AuditLog
            {
                Action = "Reject",
                UserId = GetCurrentUserId(),
                Details = $"Rejected Request {request.Id}. Reason: {comment}",
                Timestamp = DateTime.Now
            };
            _context.Add(audit);

            await _context.SaveChangesAsync();

            // Notify Dashboard
            await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate");

            return RedirectToAction(nameof(ApprovalQueue));
        }
    }
}
