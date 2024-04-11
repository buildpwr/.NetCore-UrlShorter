namespace BuildPWR.UrlShorter.Entities
{
    public class UrlItem
    {
        public int Id { get; set; }
        public string BaseUrl { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
    }

}
