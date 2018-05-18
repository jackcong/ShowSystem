using BizLogic;
using ComLib.Exceptions;
using ComLib.Export;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebModel.Account;
using WebModel.Col;

namespace TimeSheet.Controllers
{
    public class ExportController : Controller
    {
        //
        // GET: /Export/
        public ExportBizLogic exportBiz;// = new ExportBizLogic();
        UserModel user;

        public ExportController()
        {
            user = AccountHelper.GetCurrentUser();

            exportBiz = new ExportBizLogic(user);
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ExportTimeSheetList(string mainId, string fullSearch, string colInfos)
        {
            try
            {
                BuildExportParamater("TimeSheet", mainId, fullSearch, colInfos);
                return PartialView("Export");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ExportTimeSheetDetailList(string mainId, string fullSearch, string colInfos)
        {
            try
            {
                BuildExportParamater("TimeSheetDetail", mainId, fullSearch, colInfos);
                return PartialView("Export");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ExportFile(string dataType, string exportType, string exportFields, string fullSreach, string exportFieldsMataData,int userType)
        {
            
            DataTable dtExportData = new DataTable();
            switch (dataType)
            {
                case "TimeSheet":
                    dtExportData = exportBiz.GetTimeSheetExportData(fullSreach,userType);
                    break;
                case "TimeSheetDetail":
                    dtExportData = exportBiz.GetTimeSheetDetailExportData(fullSreach,userType);
                    break;
            }

            string fileName = "ExportData_" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + (new Random()).Next(100, 999).ToString();
            Export exportController = new Export();
            ExportModel exportObj;
            List<string[]> listTemp;

            listTemp = getTempList(dataType, exportFields, exportFieldsMataData, dtExportData);

            //Generate Export File
            exportObj = getExportObject(fileName);

            fileName = exportController.GenerateExportFile(exportType, dtExportData, exportObj, listTemp);

            return "../DownLoadData/" + fileName;
        }
        public FileStreamResult DownloadFile(string fileNameWithPath)
        {
            try
            {
                string absoluFilePath = Server.MapPath(fileNameWithPath);
                string fileName = fileNameWithPath.Substring(fileNameWithPath.LastIndexOf("/") + 1, fileNameWithPath.Length - fileNameWithPath.LastIndexOf("/") - 1);
                FileStream fs = new FileStream(absoluFilePath, FileMode.Open);

                int length = (int)fs.Length;
                byte[] data = new byte[length];
                fs.Position = 0;
                fs.Read(data, 0, length);
                MemoryStream ms = new MemoryStream(data);
                fs.Close();
                System.IO.File.Delete(absoluFilePath);
                return File(ms, "application/octet-stream", Server.UrlEncode(fileName));
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                throw new FileOperationException();
            }
        }
        private List<string[]> getTempList(string dataType, string exportFields, string exportFieldsMataData, DataTable dtExportData)
        {
            List<string[]> listTemp = new List<string[]>();
            //Filter Columns
            string[] exportFieldList = exportFields.Split(',');

            var listCol = JsonConvert.DeserializeObject(exportFieldsMataData, typeof(List<colInfoModel>)) as List<colInfoModel>;

            foreach (colInfoModel col in listCol)
            {
                if ((col.disType == "text" || col.disType == "date") && col.isHidden.Trim() != "true") // 
                {
                    foreach (string s in exportFieldList)
                    {
                        if (s == col.columnName)
                        {
                            string[] str = { col.columnName, col.disName_en };
                            listTemp.Add(str);
                            break;
                        }
                    }
                }
            }

            for (int i = dtExportData.Columns.Count - 1; i >= 0; i--)
            {
                bool exportFalg = false;
                foreach (string fieldName in exportFieldList)
                {
                    if (dtExportData.Columns[i].ColumnName == fieldName)
                    {
                        exportFalg = true;
                        break;
                    }

                    if (dataType == "ClaimExceptions" && dtExportData.Columns[i].ColumnName == "POSIDLink")
                    {
                        exportFalg = true;
                        break;
                    }
                }

                if (!exportFalg)
                    dtExportData.Columns.Remove(dtExportData.Columns[i]);
            }

            return listTemp;
        }
        private ExportModel getExportObject(string fileName)
        {
            ExportModel exportObj = new ExportModel();
            Export exportController = new Export();
            exportObj.virtualPath = ConfigurationManager.AppSettings["DownLoadVirtualPath"];

            exportObj.xlsFileName = Server.MapPath(exportObj.virtualPath) + fileName + ".xls";
            exportObj.csvFileName = Server.MapPath(exportObj.virtualPath) + fileName + ".csv";
            exportObj.zipFileName = Server.MapPath(exportObj.virtualPath) + fileName + ".zip";

            exportObj.fileName = fileName;

            return exportObj;
        }
        private void BuildExportParamater(string dataType, string mainId, string fullSearch, string colInfos)
        {
            List<string[]> hashTemp = new List<string[]>();

            var listCol = JsonConvert.DeserializeObject(colInfos, typeof(List<colInfoModel>)) as List<colInfoModel>;
            foreach (colInfoModel col in listCol)
            {
                if ((col.disType == "text" || col.disType == "date")) // 
                {
                    //Hashtable colInfo = new Hashtable();
                    string[] str = { col.columnName, col.disName_en };

                    hashTemp.Add(str);
                }
            }

            ViewData["DataType"] = dataType;
            ViewData["MainId"] = mainId;
            ViewData["SearchField"] = "";
            ViewData["SearchString"] = fullSearch;
            ViewData["SearchOper"] = "";
            ViewData["Sidx"] = "";
            ViewData["Sord"] = "";
            //ViewData["ExportFields"] = colInfo;
            ViewData["ExportFields"] = hashTemp;
            ViewData["ExportFieldsMataData"] = colInfos;
        }
    }
}