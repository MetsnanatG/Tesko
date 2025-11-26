namespace Tesko.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<AssetStock> AssetStocks { get; set; }
        public List<TopItem> TopItems { get; set; }
        public List<TopRequester> TopRequesters { get; set; }
    }

    public class AssetStock
    {
        public string Name { get; set; }
        public int Total { get; set; }
        public int Available { get; set; }
    }

    public class TopItem
    {
        public string Name { get; set; }
        public int RequestCount { get; set; }
    }

    public class TopRequester
    {
        public string Name { get; set; }
        public int RequestCount { get; set; }
    }
}
