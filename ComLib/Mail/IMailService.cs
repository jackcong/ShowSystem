using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ComLib.Mail
{
    public interface IMailService
    {
        void Send(MailObject mailObj);

        List<MailObject> Read(string fileDir, List<string> fileType);

        List<MailObject> ReadWithSubject(string fileDir, List<string> fileType, string mailMoveToFolder, string subject, int limitOfMails = 20);

        List<MailObject> ReadWithSubject(string fileDir, List<string> fileType, string mailMoveToFolder,string subject, List<Regex> regexs, string andOr = "AND", int limitOfMails = 20);

        List<MailObject> ReadWithSubject(string fileDir, List<string> fileTypes, string moveToFolder, string mailFromFolder, string subject, List<Regex> regexs, string andOr = "AND", int limitOfMails = 20);
    }
    
    public class EmailConfigModel
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> IsEWS { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string HostName { get; set; }
        public Nullable<int> Port { get; set; }
        public Nullable<int> usessl { get; set; }
        public Nullable<int> SendPort { get; set; }
        public string SMTPHost { get; set; }
        public Nullable<int> ReceivePort { get; set; }
        public Nullable<int> SendSsl { get; set; }
        public int DownLoad { get; set; }
        public int refreshMsg { get; set; }
        public int Process { get; set; }
        public HttpSessionStateBase tempCurrent { get; set; }
    }
    
}
