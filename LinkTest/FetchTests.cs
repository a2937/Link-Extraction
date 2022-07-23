using HtmlAgilityPack;

namespace LinkExtraction.Test
{
    public class FetchTests
    {
        /// <summary>
        /// Ensure that every test file can actually be read
        /// </summary>
        /// <param name="fileName">The test file</param>
        /// <exception cref="XunitException">If the file can't be reached on the disk</exception>
        [Theory]
        [InlineData("no_links.html")]
        [InlineData("five_links.html")]
        [InlineData("relative_links.html")]
        public void Local_File_URL_Can_Be_Fetched(string fileName)
        {
            string curDir = Path.Combine(Directory.GetCurrentDirectory(), "test_pages");
            var fileLocation = Path.Combine(curDir, fileName);
            try
            {
                HtmlDocument document = new HtmlDocument();
                document.Load(fileLocation);
                Scraper.GetNodes("https://localhost", document);
            }
            catch (Exception ex)
            {
                throw new XunitException(ex.Message);
            }
        }

        /// <summary>
        /// Ensures that if an improper url is fed into the link scraper; 
        /// no links are actually returned
        /// </summary>
        [Fact]
        public void Get_Broken_URL_Returns_No_Links()
        {
            var content = Scraper.ScrapeLinks("asdafdassda");
            Assert.Empty(content);
        }


        /// <summary>
        /// Confirms that if an file with no links is read; there will be 
        /// no links in there.
        /// </summary>
        [Fact]
        public void Get_URL_With_No_Links()
        {
            string curDir = Path.Combine(Directory.GetCurrentDirectory(), "test_pages");
            var fileLocation = Path.Combine(curDir, "no_links.html");
            HtmlDocument document = new HtmlDocument();
            document.Load(fileLocation);
            var content = Scraper.GetNodes("https://localhost", document);
            Assert.Empty(content);
        }

        /// <summary>
        /// Ensures that five links are found when there are five links
        /// </summary>
        [Fact]
        public void Get_URL_With_Five_Links()
        {
            string curDir = Path.Combine(Directory.GetCurrentDirectory(), "test_pages");
            var fileLocation = Path.Combine(curDir, "five_links.html");
            HtmlDocument document = new HtmlDocument();
            document.Load(fileLocation);
            var content = Scraper.GetNodes("https://localhost", document);
            Assert.Equal(5, content.Length);
        }

        /// <summary>
        /// Ensures that relative URLs are changed into absolute URLs 
        /// and that absolute URLs are unchanged.
        /// </summary>
        [Fact]
        public void Get_Changed_Relative_URL()
        {
            string curDir = Path.Combine(Directory.GetCurrentDirectory(), "test_pages");
            var fileLocation = Path.Combine(curDir, "relative_links.html");
            HtmlDocument document = new HtmlDocument();
            document.Load(fileLocation);
            var content = Scraper.GetNodes("https://localhost", document);
            Assert.Contains("https://localhost//no_links.html", content);
            Assert.Contains("https://www.freecodecamp.org", content);
        }
    }
}
