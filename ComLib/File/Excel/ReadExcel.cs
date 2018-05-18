using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using ComLib.File.Csv;

namespace ComLib.File.Excel
{
    public class ReadExcel
    {
        public DataTable GetDataTableFromExcel(string fullpath)
        {
            FileStream stream = System.IO.File.Open(fullpath, FileMode.Open, FileAccess.Read);

            IWorkbook workbook;

            var extension = Path.GetExtension(fullpath);
            extension = extension.ToLower();
            if (extension == ".xls")
            {
                workbook = new HSSFWorkbook(stream);
            }
            else if (extension == ".xlsx")
            {
                workbook = new XSSFWorkbook(stream);
            }
            else
            {
                throw new NotSupportedException();
            }
            
            ISheet sheet = workbook.GetSheetAt(0);

            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(0);

            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.PhysicalNumberOfRows;

            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null || row.PhysicalNumberOfCells == 0)
                {
                    break;
                }

                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (j < 0)
                    {
                        break;
                    }
                    if (row.GetCell(j) != null)

                        if (row.GetCell(j).CellType == CellType.FORMULA)
                        {
                            dataRow[j] = row.GetCell(j).NumericCellValue.ToString();
                        }
                        else
                        {

                            dataRow[j] = row.GetCell(j).ToString();
                        }
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }

        public DataTable GetDataTableFromExcel(string fullpath,int sheetIndex)
        {
            FileStream stream = System.IO.File.Open(fullpath, FileMode.Open, FileAccess.Read);

            IWorkbook workbook;

            var extension = Path.GetExtension(fullpath);
            extension = extension.ToLower();
            if (extension == ".xls")
            {
                workbook = new HSSFWorkbook(stream);
            }
            else if (extension == ".xlsx")
            {
                workbook = new XSSFWorkbook(stream);
            }
            else
            {
                throw new NotSupportedException();
            }

            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(0);

            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.PhysicalNumberOfRows;

            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null || row.PhysicalNumberOfCells == 0)
                {
                    break;
                }

                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (j < 0)
                    {
                        break;
                    }
                    if (row.GetCell(j) != null)

                        if (row.GetCell(j).CellType == CellType.FORMULA)
                        {
                            dataRow[j] = row.GetCell(j).NumericCellValue.ToString();
                        }
                        else
                        {

                            dataRow[j] = row.GetCell(j).ToString();
                        }
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }
        private DataTable GetDataTableFromExcel(int sheetIndex, IWorkbook workbook)
        {

            ISheet sheet = workbook.GetSheetAt(sheetIndex);
    
            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(0);

            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.PhysicalNumberOfRows;

            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null || row.PhysicalNumberOfCells == 0)
                {
                    break;
                }

                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (j < 0)
                    {
                        break;
                    }
                    if (row.GetCell(j) != null)

                        if (row.GetCell(j).CellType == CellType.FORMULA)
                        {
                            dataRow[j] = row.GetCell(j).NumericCellValue.ToString();
                        }
                        else
                        {

                            dataRow[j] = row.GetCell(j).ToString();
                        }
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }
        public DataSet GetDataSetFromExcel(string fullpath)
        {
            DataSet ds = new DataSet();
            FileStream stream = System.IO.File.Open(fullpath, FileMode.Open, FileAccess.Read);

            IWorkbook workbook;

            var extension = Path.GetExtension(fullpath);

            if (extension == ".xls")
            {
                workbook = new HSSFWorkbook(stream);
            }
            else if (extension == ".xlsx")
            {
                workbook = new XSSFWorkbook(stream);
            }
            else
            {
                throw new NotSupportedException();
            }
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                ds.Tables.Add(GetDataTableFromExcel(i, workbook));
            }
            return ds;
        }
        public DataTable GetDataTableFromCsv(string fullpath)
        {
            StreamReader sr = new StreamReader(fullpath);
            string[] titleRow = CSVProcessor.CSVParseRow(sr).ToArray();
            List<string[]> data = new List<string[]>();
            DataTable t = new DataTable();
            t.Columns.AddRange(titleRow.Select(c => new DataColumn(c)).ToArray());
            string[] row = CSVProcessor.CSVParseRow(sr).ToArray();
            while (row.Count() > 0)
            {
                t.Rows.Add(row.Take(row.Count()).ToArray());
                row = CSVProcessor.CSVParseRow(sr).ToArray();
            }
            sr.Close();

            return t;
        }

        public DataTable GetDataTableFromFile(string fullpath)
        {
            if (!System.IO.File.Exists(fullpath))
            {
                throw new FileNotFoundException(string.Format("%s is not found.", fullpath));
            }
            var extension = Path.GetExtension(fullpath);
            extension = extension.ToLower();
            switch (extension)
            {
                case ".xls":
                case ".xlsx":
                    return GetDataTableFromExcel(fullpath);
                case ".csv":
                    return GetDataTableFromCsv(fullpath);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
