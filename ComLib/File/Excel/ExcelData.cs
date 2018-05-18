using System;
using System.IO;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace ComLib.File.Excel
{
    public class ExcelData : IDownloadable
    {
        private HSSFWorkbook _workbook;

        public ExcelData()
        {
            Initialize();
        }

        public void BuildHead(string[] head)
        {
            ISheet sheet = _workbook.GetSheetAt(0);
            IRow row = sheet.CreateRow(0);
            int columnIndex = 0;
            foreach(string c in head)
            {
                row.CreateCell(columnIndex, CellType.STRING).SetCellValue(c);
                ++columnIndex;
            }
        }

        public void BuildBody(string[,] body)
        {
            ISheet sheet = _workbook.GetSheetAt(0);
            for (int i = 0; i < body.GetLength(0); ++i)
            {
                IRow row = sheet.CreateRow(i+1);
                int heightfactor = 1;
                for (int j = 0; j < body.GetLength(1); ++j)
                {
                    ICellStyle cs = _workbook.CreateCellStyle();
                    cs.WrapText = true;
                    if (!string.IsNullOrEmpty(body[i, j]))
                    {
                        heightfactor = Math.Max(heightfactor,
                                                body[i, j].Split(new[] {"\n"}, StringSplitOptions.None).Length);
                        ICell cell = row.CreateCell(j, CellType.STRING);
                        cell.SetCellValue(body[i, j]);
                        cell.CellStyle = cs;
                    }
                    
                }
                row.HeightInPoints = heightfactor * sheet.DefaultRowHeightInPoints;
            }
            for(int j=0;j<body.GetLength(1);++j)
            {
                sheet.AutoSizeColumn(j);
            }
        }

        public HSSFWorkbook Workbook
        {
            get { return _workbook; }
        }

        public string SheetName
        {
            get { return _workbook.GetSheetName(0); }
            set { _workbook.SetSheetName(0, value); }
        }

        private void Initialize()
        {
            _workbook=new HSSFWorkbook();
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "T2VSoft Inc.";
            _workbook.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            _workbook.SummaryInformation= si;
            _workbook.CreateSheet();
        }

        public string MIMEType
        {
            get
            {
                return File.MIMEType.XLS;
            }
        }

        public MemoryStream DownloadStream
        {
            get
            {
                MemoryStream ms=new MemoryStream();
                _workbook.Write(ms);
                return ms;
            }
        }
    }
}
