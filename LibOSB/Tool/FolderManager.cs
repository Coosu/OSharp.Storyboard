using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOsb.Tool
{
    class FolderManager
    {
        public Dictionary<string, string> Difficulty { get; set; } = new Dictionary<string, string>();
        public FolderManager(string folderPath)
        {
            LoadFiles(folderPath);
        }

        private void LoadFiles(string folderPath)
        {
            var file_list = new List<string>();
            file_list.AddRange(Directory.EnumerateFiles(folderPath, "*.osu", SearchOption.TopDirectoryOnly));
            foreach (var path in file_list)
            {
                string o = File.ReadAllText(path);
                string ver_str = "Version:";
                int i = o.IndexOf(ver_str) + ver_str.Length;
                int i2 = o.IndexOf("\n", i);
                string diff_name = o.Substring(i, i2 - i).Trim('\r');
                Difficulty.Add(diff_name, path);
            }
        }
    }
}
