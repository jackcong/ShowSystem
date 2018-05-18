using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Kent.Boogaart.KBCsv;

namespace ComLib.File.Csv
{
    public class CsvFile : IDataFile
    {
        private readonly bool _hasHeaders;
        private readonly string _path;
        readonly string[] _header;
        private string fileName;

        public CsvFile(string path)
        {
            this._path = path;
            fileName = new FileInfo(path).Name;
            CsvReader reader = new CsvReader(path);
            _header = reader.ReadHeaderRecord().Values.ToArray();
            _hasHeaders = true;
        }

        public CsvFile(string path, params string[] headers)
        {
            this._path = path;
            fileName = new FileInfo(path).Name;
            CsvReader reader = new CsvReader(path);
            _header = headers.Clone() as string[];
            _hasHeaders = false;
        }

        public string[] GetHeaders()
        {
            return _header;
        }

        public DataTable ToDataTable()
        {
            CsvReader reader = new CsvReader(_path);
            if (_hasHeaders)
                reader.ReadDataRecord();
            ICollection<DataRecord> records = reader.ReadDataRecords();
            DataTable dt = new DataTable();
            foreach (string head in _header)
            {
                DataColumn dc = new DataColumn(head);
                dt.Columns.Add(dc);
            }
            for (int i = 0; i < records.Count; i++)
            {
                if (records.ElementAt(i).Values.Count == 0)
                    continue;
                DataRow dr = dt.NewRow();
                dr.ItemArray = records.ElementAt(i).Values.ToArray();
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
