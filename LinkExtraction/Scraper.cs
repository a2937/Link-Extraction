using HtmlAgilityPack;

namespace LinkExtraction
{
    public static class Scraper
    {
        /// <summary>
        /// Returns an absolute version of all the anchor
        /// tags's hrefs.
        /// </summary>
        /// <param name="url">The url of the web page to check</param>
        /// <returns>A collection of all links found on the page</returns>
        public static string[] ScrapeLinks(String url)
        {
            HtmlWeb web = new HtmlWeb();
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                try
                {
                    HtmlDocument doc = web.Load(url);
                    return GetNodes(url, doc);
                }
                catch (Exception)
                {
                    return Array.Empty<string>();
                }
            }
            return Array.Empty<string>();
        }

        public static String[] GetNodes(String url, HtmlDocument doc)
        {
            var anchors = doc.DocumentNode.Descendants("A");
            return anchors.Select(x => GetURL(url, x)).ToArray();
        }

        private static String GetURL(String baseURL, HtmlNode x)
        {
            var url = x.GetAttributeValue("href", "");
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return Uri.UnescapeDataString(url);
            }
            else if (Uri.IsWellFormedUriString(url, UriKind.Relative))
            {
                return baseURL + "/" + url;
            }
            else
            {
                return "";
            }
        }
    }
}
