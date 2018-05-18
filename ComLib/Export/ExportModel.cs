using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Export
{
    public class ExportModel
    {
        public string virtualPath { get; set; }
        public string xlsFileName { get; set; }
        public string csvFileName { get; set; }
        public string zipFileName { get; set; }
        public string fileName { get; set; }
    }
}
