using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ComLib.Extension;

namespace ComLib.File.Csv
{
    public static class CsvFileLoader
    {
        public static CsvData Load(string path)
        {
            string[] data = System.IO.File.ReadAllLines(path);
            return Load(data);
        }



        public static CsvData Load(Stream fs)
        {
            StreamReader reader = new StreamReader(fs);
            List<string> data = new List<string>();
            while(!reader.EndOfStream)
            {
                data.Add(reader.ReadLine());
            }
            return Load(data);
        }

        public static CsvData Load(IEnumerable<string> data)
        {
            // TODO: Add support for multiline string.
            Regex regex = new Regex("(\\\"([^\\\"]|(\\\"\\\"))*\\\")|[^\",]+|(?<=,)[^,]*?(?=,)|^[^,]*?(?=,)|(?<=,)[^,]*?$");

            List<MatchCollection> allMatches = new List<MatchCollection>();
            foreach (var c in data)
            {
                allMatches.Add(regex.Matches(c));
            }
            if (allMatches.Count == 0)
            {
                throw new System.Exception("The file does not seem to be a valid CSV file.");
            }
            CsvData csvData = new CsvData();
            string[] head = new string[allMatches[0].Count];
            
            for (int i = 0; i < allMatches[0].Count; ++i)
            {
                head[i] = allMatches[0][i].ToString().TrimOne('\"').Replace("\"\"", "\"");
            }
            csvData.BuildHead(head);
            for (int i = 1; i < allMatches.Count; ++i)
            {
                CsvRow cr = new CsvRow();
                for (int j = 0; j < allMatches[i].Count; j++)
                {
                    cr.Add(new CsvCell(head[j], allMatches[i][j].ToString()));
                }
                csvData.Add(cr);
            }
            return csvData;
        }
    }
}
