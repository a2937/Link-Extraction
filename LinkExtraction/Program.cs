namespace LinkExtraction
{
    public static class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Welcome to this link extractor program");
            Console.WriteLine("This will read the destination of all links on the page.");
            string? URL;
            do
            {
                Console.Write("Enter URL to read > ");
                URL = Console.ReadLine();
                if (URL == null || URL.Trim().Length == 0)
                {
                    Console.WriteLine("No input received");
                }
            }
            while (URL == null);
            var hrefs = Scraper.ScrapeLinks(URL);

            if (hrefs.Length > 0)
            {
                Console.Out.WriteLine("Found links:");
                for (int i = 0; i < hrefs.Length; i++)
                {
                    Console.Out.WriteLine(hrefs[i]);
                }
                await WriteFile(URL, hrefs);
                Console.Out.WriteLine("\n");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Found Links.txt");
                Console.WriteLine("All links have been written to " + path + ".");
            }
            else
            {
                Console.WriteLine("No links found on web address: " + URL + ".");
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static async Task WriteFile(string url, string[] links)
        {
            using StreamWriter file = new("Found Links.txt");
            await file.WriteLineAsync("Links for " + url);
            foreach (string line in links)
            {
                await file.WriteLineAsync(line);
            }
            file.Close();
        }
    }
}

