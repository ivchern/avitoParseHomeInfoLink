using System.Text.RegularExpressions;
using System.Net.Http;

namespace WindowsFormsApp3
{
    class ParseAvito
    {
         public string adress { get; set; }
         public string AvitoParse(string avitoLink)
        {
            using (HttpClient hc = new HttpClient())
                adress = hc.GetStringAsync(avitoLink).GetAwaiter().GetResult();
            Match match = Regex.Match(adress, "\"><span class=\"item-address__string\">\n (.*?)\n </span>");
            adress = match.Groups[1].Value;
            return adress;
        }
       
    }
}
//"<tr>\r\n                <td>(.*?)</td>\r\n                <td>(.*?)</td>\r\n                <td>(.*?)</a></td>\r\n                <td>(.*?)</td>\r\n                <td>(.*?)</td>\r\n                <td>(.*?)</td>\r\n                <td>(.*?)</td>\r\n            </tr>\r\n"