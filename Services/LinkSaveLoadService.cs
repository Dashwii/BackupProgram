using BackupProgram.Models;
using BackupProgram.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.Services
{
    public class LinkSaveLoadService : IFileHandlingService
    {
        public static void SaveLinksJson(List<SourceLinkModel> links)
        {
            if (links == null) { throw new ArgumentNullException(); }
            var jsonString = JsonConvert.SerializeObject(links, Formatting.Indented);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "links.json");
            File.WriteAllText(path, jsonString);
        }

        public static List<SourceLinkModel> LoadLinksJson()
        {
            List<SourceLinkModel> links = new();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "links.json");
            if (File.Exists(path))
            {
                string data = File.ReadAllText(path);
                links.AddRange(JsonConvert.DeserializeObject<List<SourceLinkModel>>(data)!);
                return Cleanup(links!);
            }
            else
            {
                return links;
            }    
        }

        /// <summary>
        /// Remove links that have not existent directories.
        /// </summary>
        /// <param name="links"></param>
        /// <returns></returns>
        private static List<SourceLinkModel> Cleanup(List<SourceLinkModel> links)
        {
            for (int i = links.Count - 1; i >= 0; i--)
            {
                var link = links[i];
                if (!Directory.Exists(link.FilePath))
                {
                    // In the future only toggle it off and give user a warning that the path does not exist instead of removing.
                    links.Remove(link);
                }
                else
                {
                    for (int j = link.DestLinks.Count - 1;j >=0 ; j--)
                    {
                        var destLink = link.DestLinks[j];
                        if (!Directory.Exists(destLink.FilePath))
                        {
                            link.DestLinks.Remove(destLink);
                        }
                    }
                }
            }
            return links;
        }
    }
}
