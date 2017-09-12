using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml;
using Discord.Commands;
using System.Net;

namespace HarmonyBot
{
    class StepmaniaParser
    {
        public static string StepMania(string uid)
        {
            if (!(uid.All(Char.IsDigit)))
                return "Thats not a UID number!";
            var html = @"http://stepmaniaonline.net/index.php?page=profile&uid=" + uid;
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            /*if (web.StatusCode == HttpStatusCode.OK)
                return "User not found.";*/
            if (htmlDoc.DocumentNode.SelectSingleNode("/html/body/div/div[2]/div[2]/div[1]/center").InnerHtml.Contains("Site News"))
                return "User not found.";
            StringBuilder sb = new StringBuilder();
            var rows = htmlDoc.DocumentNode.SelectNodes("//table//tr");
            for (int i = 2; i < 24; i++)
            {
                sb.AppendLine(string.Format("\t - {0}", rows[i].InnerText.Trim()));
            }

            return sb.ToString();
        }
    }
}
