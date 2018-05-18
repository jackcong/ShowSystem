using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Ftp
{
    public partial class FTPManager
    {
        public enum FTPListResult
        {
            Success,
            Failed
        }


        public enum FTPUploadResult
        {
            Success,
            Failed
        }

        public enum FTPDownloadResult
        {
            Success,
            Failed
        }

        public enum FTPTestResult
        {
            Success,
            ConnectionTimeout,
            ConnectionFailed,
            EnteringPassive,
            AccessDenied,
            Failed
        }
    }
}
