using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ComLib.Extension;
using ComLib.Log;

namespace ComLib.Ftp
{
    public partial class FTPManager
    {
        public enum TransferBehavior
        {
            [StringValue("Active")]
            Active,
            [StringValue("Passive")]
            Passive
        }

        private Uri _uri;

        private readonly NetworkCredential _cred;
        private readonly TransferBehavior _transferBehavior;
        

        public FTPManager(string server, int port = 21, string path = "", string username = "", string password = "",
                          TransferBehavior transferBehavior = TransferBehavior.Passive)
        {
            _uri = new UriBuilder("ftp", server, port, path).Uri;

            _cred = new NetworkCredential(username, password);
            _transferBehavior = transferBehavior;
        }

        private FtpWebRequest NewRequest(string path = "")
        {
            var request = WebRequest.Create(Path.Combine(_uri.ToString(), path)) as FtpWebRequest;
            if (request == null)
                return null;
            request.Credentials = _cred;
            request.KeepAlive = false;
            request.UsePassive = _transferBehavior == TransferBehavior.Passive;
            return request;
        }

        public FTPTestResult Test()
        {
            var logger = new WinEventLogger("DCMAzure");
            logger.Log("Test: Creating new request.", LogEntryType.Info);
            var request = NewRequest();
            logger.Log("Test: New request created.", LogEntryType.Info);
            
            if (request == null)
            {
                logger.Log("Test: New request creation failed.", LogEntryType.Info);
                return FTPTestResult.Failed;
            }
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            try
            {
                logger.Log("Test: Getting response.", LogEntryType.Info);
                var response = (FtpWebResponse) request.GetResponse();
                logger.Log("Test: Getting response stream.", LogEntryType.Info);
                Stream responseStream = response.GetResponseStream();
                logger.Log("Test: Got response stream.", LogEntryType.Info);
            }
            catch(WebException e)
            {
                logger.Log("Exception: "+e.Message, LogEntryType.Info);
                if(e.InnerException!=null)
                    logger.Log("Inner Exception: "+e.InnerException.Message, LogEntryType.Error);
                var status = ((FtpWebResponse) e.Response).StatusCode;
                switch (status)
                {
                    case FtpStatusCode.Undefined:
                        return FTPTestResult.ConnectionFailed;
                    case FtpStatusCode.EnteringPassive:
                        return FTPTestResult.EnteringPassive;
                    case FtpStatusCode.NotLoggedIn:
                        return FTPTestResult.AccessDenied;
                    default:
                        return (FTPTestResult)status;
                }
            }
            return FTPTestResult.Success;
        }

        public FTPListResult List(out List<string> list)
        {
            var logger = new WinEventLogger("DCMAzure");
            logger.Log("List: Creating new request.", LogEntryType.Info);
            var request = NewRequest();
            logger.Log("List: New request created.", LogEntryType.Info);
            if (request == null)
            {
                list = null;
                return FTPListResult.Failed;
            }

            request.Method = WebRequestMethods.Ftp.ListDirectory;
            logger.Log("List: Getting response.", LogEntryType.Info);
            var response = (FtpWebResponse)request.GetResponse();
            logger.Log("List: Getting response stream.", LogEntryType.Info);
            Stream responseStream = response.GetResponseStream();
            logger.Log("List: Got response stream.", LogEntryType.Info);
            if (responseStream == null)
            {
                list = null;
                return FTPListResult.Failed;
            }
            var reader = new StreamReader(responseStream);
            var tlist = new List<string>(5);
            while(!reader.EndOfStream)
            {
                tlist.Add(reader.ReadLine());
            }
            reader.Close();
            response.Close();
            list = tlist;
            return FTPListResult.Success;
        }

        public FTPDownloadResult Download(string path, string target)
        {
            var request = NewRequest(path);
            if(request == null)
                return FTPDownloadResult.Failed;

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.UseBinary = true;
            
            FtpWebResponse response = null;
            Stream stream = null;
            FileStream fs = null;
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return FTPDownloadResult.Failed;
                }

                response = request.GetResponse() as FtpWebResponse;
                if (response == null)
                {
                    return FTPDownloadResult.Failed;
                }

                stream = response.GetResponseStream();
                if (stream == null)
                {
                    return FTPDownloadResult.Failed;
                }

                var dir = Path.GetDirectoryName(target);
                if(dir == null)
                    return FTPDownloadResult.Failed;

                Directory.CreateDirectory(dir);

                fs = new FileStream(target, FileMode.Create);

                var buf = new byte[4096];
                while (stream.CanRead)
                {
                    
                    fs.Write(buf, 0, stream.Read(buf, 0, 4096));
                }
                final:
                return FTPDownloadResult.Success;
            } catch
            {
                return FTPDownloadResult.Failed;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if (stream != null)
                    stream.Close();

                if (response != null)
                    response.Close();
            }
        }

        public FTPUploadResult Upload(string path)
        {
            var request = NewRequest(path);
            if (request == null)
                return FTPUploadResult.Failed;

            Stream stream = null;
            FileStream fs = null;
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return FTPUploadResult.Failed;
                }

                request.Method = WebRequestMethods.Ftp.UploadFile;

                stream = request.GetRequestStream();
                if (stream == null)
                {
                    return FTPUploadResult.Failed;
                }

                fs = new FileStream(path, FileMode.Open);

                var buf = new byte[4096];
                while (fs.CanRead)
                {
                    
                    stream.Write(buf, 0, fs.Read(buf, 0, 4096));
                }
            final:
                return FTPUploadResult.Success;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if (stream != null)
                    stream.Close();
            }
        }
    }
}
