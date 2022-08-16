using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace GS
{
    public sealed class Program
    {
        #region Static fields 
        private static readonly Uri Uri = new Uri("https://consolevariations.com/blog/every-gamecube-variation-complete-color-list-2");
        #endregion
        
        private static void Main(string[] args)
        {
            var web      = new HtmlWeb();
            var document = web.Load(Uri)
                              .DocumentNode.SelectNodes("//h1")
                              .Where(h => Regex.IsMatch(h.InnerText, "\\d+\\. .+"))
                              .Select(h => h.InnerText.Split('.').Last().Trim())
                              .ToArray();
            
            Array.ForEach(document, Console.WriteLine);
        }
    }
}