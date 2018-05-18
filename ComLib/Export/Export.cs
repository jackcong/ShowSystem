using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using ComLib.Compress;
using System.Collections;

namespace ComLib.Export
{
    public class Export
    {
        public string GenerateExportFile(string exportType, DataTable dtExportData, ExportModel exportObj, Hashtable columnInfo)
        {
            string fileName = exportObj.fileName;
            //IdentityAnalogue ia = new IdentityAnalogue();
            try
            {
                //string userName = ConfigurationManager.AppSettings["ImpersonateUserName"];
                //string password = ConfigurationManager.AppSettings["ImpersonatePassWord"];
                //if (ia.ImpersonateValidUser(userName, "", password))
                //{
                string xlsFileName = exportObj.xlsFileName;
                string csvFileName = exportObj.csvFileName;
                string zipFileName = exportObj.zipFileName;

                //if (!Directory.Exists(exportObj.virtualPath))
                //{
                //    Directory.CreateDirectory(exportObj.virtualPath);
                //}

                switch (exportType)
                {
                    case "EXCEL":
                        ExportExcel(xlsFileName, dtExportData, columnInfo);
                        fileName += ".xls";
                        break;
                    case "CSV":
                        ExportCsv(csvFileName, dtExportData, columnInfo);
                        fileName += ".csv";
                        break;
                    case "CSV COMPRESS":
                        ExportCsvCompress(zipFileName, csvFileName, dtExportData, columnInfo);
                        fileName += ".zip";
                        break;
                }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileName;
        }

        public string GenerateExportFile(string exportType, DataTable dtExportData, ExportModel exportObj, List<string[]> columnInfo)
        {
            string fileName = exportObj.fileName;
            //IdentityAnalogue ia = new IdentityAnalogue();
            try
            {

                string xlsFileName = exportObj.xlsFileName;
                string csvFileName = exportObj.csvFileName;
                string zipFileName = exportObj.zipFileName;

                switch (exportType)
                {
                    case "EXCEL":
                       ExportExcel(xlsFileName, dtExportData, columnInfo);
                        fileName += ".xls";
                        break;
                    case "CSV":
                        ExportCsv(csvFileName, dtExportData, columnInfo);
                        fileName += ".csv";
                        break;
                    case "CSV COMPRESS":
                        ExportCsvCompress(zipFileName, csvFileName, dtExportData, columnInfo);
                        fileName += ".zip";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileName;
        }

        private void ExportCsv(string csvFileName, DataTable dtExportData, List<string[]> columnInfo)
        {
            string csvSplit = ",";

            StreamWriter sw = new StreamWriter(csvFileName, false, Encoding.UTF8);
            try
            {
                string strLine = string.Empty;

                //Header
                foreach (string[] column in columnInfo)
                {
                    foreach (DataColumn dc in dtExportData.Columns)
                    {
                        if (column[0] == dc.ColumnName)
                        {
                            strLine += csvSplit + column[1];
                            break;
                        }
                    }
                }

               //foreach (DataColumn dc in dtExportData.Columns)
               // {
               //     foreach (string[] column in columnInfo)
               //     {
               //         if (column[0] == dc.ColumnName)
               //         {
               //             strLine += csvSplit + column[1];
               //             break;
               //         }
               //     }
               // }

                if (!string.IsNullOrEmpty(strLine))
                {
                    sw.WriteLine(strLine.Substring(1));
                }

                //Rows
                foreach (DataRow dr in dtExportData.Rows)
                {
                    strLine = string.Empty;
                    foreach (string[] column in columnInfo)
                    {
                        foreach (DataColumn dc in dtExportData.Columns)
                        {
                            if (column[0] == dc.ColumnName)
                            {
                                strLine += csvSplit + "\"" + dr[dc.ColumnName].ToString().Replace("\"", "'") + "\"";
                            }
                        }
                    }

                    //foreach (DataColumn dc in dtExportData.Columns)
                    //{

                        
                    //}
                    if (!string.IsNullOrEmpty(strLine))
                    {
                        sw.WriteLine(strLine.Substring(1));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
        }

        private void ExportExcel(string xlsFileName, DataTable dtExportData, Hashtable columnInfo)
        {
            FileStream file = new FileStream(xlsFileName, FileMode.Create);
            try
            {
                //Create Sheet
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("ExportData");       //One sheet max rows is 65535

                //Create Header
                IRow rowHerder = sheet.CreateRow(0);
                for (int i = 0; i < dtExportData.Columns.Count; i++)
                {
                    foreach (DictionaryEntry column in columnInfo)
                    {
                        if (column.Key.ToString() == dtExportData.Columns[i].ColumnName.ToString())
                        {
                            rowHerder.CreateCell(i).SetCellValue(column.Value.ToString());
                            break;
                        }
                    }
                }

                //Add Row            
                for (int i = 0; i < dtExportData.Rows.Count; i++)
                {
                    DataRow dr = dtExportData.Rows[i];
                    IRow rowData = sheet.CreateRow(i + 1);

                    for (int j = 0; j < dtExportData.Columns.Count; j++)
                    {
                        rowData.CreateCell(j).SetCellValue(dr[dtExportData.Columns[j].ColumnName].ToString());
                    }
                }
                //Write File            
                workbook.Write(file);
            }
            catch
            {
                throw;
            }
            finally
            {
                file.Close();
            }
        }

        private void ExportExcel(string xlsFileName, DataTable dtExportData, List<string[]> columnInfo)
        {
            FileStream file = new FileStream(xlsFileName, FileMode.Create);
            try
            {
                //Create Sheet
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("ExportData");       //One sheet max rows is 65535

                //Create Header
                IRow rowHerder = sheet.CreateRow(0);
                int flag = 0;
                foreach (string[] column in columnInfo)
                {
                    for (int i = 0; i < dtExportData.Columns.Count; i++)
                    {
                        if (column[0] == dtExportData.Columns[i].ColumnName.ToString())
                        {
                            rowHerder.CreateCell(flag).SetCellValue(column[1]);
                            
                            break;
                        }
                    }
                    flag++;
                }

                //Add Row

                for (int i = 0; i < dtExportData.Rows.Count; i++)
                {
                    DataRow dr = dtExportData.Rows[i];
                    IRow rowData = sheet.CreateRow(i + 1);
                    flag = 0;
                    foreach (string[] column in columnInfo)
                    {
                        //for (int j = 0; j < dtExportData.Columns.Count; j++)
                        //{
                            //if (column[0] == dtExportData.Columns[j].ColumnName)
                            //{
                                rowData.CreateCell(flag).SetCellValue(dr[column[0]].ToString());
                               // break;
                            //}
                        //}
                                flag++;
                    }
                }
                //Write File            
                workbook.Write(file);
            }
            catch
            {
                throw;
            }
            finally
            {
                file.Close();
            }
        }

        private void ExportCsv(string csvFileName, DataTable dtExportData, Hashtable columnInfo)
        {
            string csvSplit = ",";

            StreamWriter sw = new StreamWriter(csvFileName, false, Encoding.UTF8);
            try
            {
                string strLine = string.Empty;

                //Header
                foreach (DataColumn dc in dtExportData.Columns)
                {
                    foreach (DictionaryEntry column in columnInfo)
                    {
                        if (column.Key.ToString() == dc.ColumnName)
                        {
                            strLine += csvSplit + column.Value.ToString();
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(strLine))
                {
                    sw.WriteLine(strLine.Substring(1));
                }

                //Rows
                foreach (DataRow dr in dtExportData.Rows)
                {
                    strLine = string.Empty;
                    foreach (DataColumn dc in dtExportData.Columns)
                    {
                        strLine += csvSplit + "\"" + dr[dc.ColumnName].ToString().Replace("\"", "'") + "\"";
                    }
                    if (!string.IsNullOrEmpty(strLine))
                    {
                        sw.WriteLine(strLine.Substring(1));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
        }


        private void ExportCsvCompress(string zipFileName, string csvFileName, DataTable dtExportData, Hashtable columnInfo)
        {
            ExportCsv(csvFileName, dtExportData, columnInfo);
            Zip.ZipFile(csvFileName, zipFileName, "");
            System.IO.File.Delete(csvFileName);
        }
        private void ExportCsvCompress(string zipFileName, string csvFileName, DataTable dtExportData, List<string[]> columnInfo)
        {
            ExportCsv(csvFileName, dtExportData, columnInfo);
            Zip.ZipFile(csvFileName, zipFileName, "");
            System.IO.File.Delete(csvFileName);
        }
    }
}
