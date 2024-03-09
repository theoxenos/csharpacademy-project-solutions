using HtmlAgilityPack;

namespace SportsResultsNotifier.Services;

public class ScraperService
{
    public void ScrapeSite()
    {
        var url = "https://www.basketball-reference.com/"; // Use the specific URL for the data you're interested in
        var web = new HtmlWeb();
        var doc = web.Load(url);

        // Navigate to the specific data you want to scrape
        // For example, if you're interested in a particular table:
        var table = doc.DocumentNode.SelectSingleNode("//table[@id='example_table_id']");

        // Extract the data you need
        // This will depend on the structure of the HTML
        var data = "";
        foreach (var row in table.SelectNodes("tbody/tr"))
        {
            foreach (var cell in row.SelectNodes("td"))
            {
                data += cell.InnerText + " "; // Adjust based on the data format you need
            }
            data += "\n"; // New line for each row
        }
    }
}