using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace ComLib.Mail
{
    public static class MailFactoryLoader
    {
        public static IMailService GetNewMailService()
        {
            string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            int sendPort = int.Parse(ConfigurationManager.AppSettings["SendPort"]);
            string useSSL = ConfigurationManager.AppSettings["usessl"].ToString();
            string userName = ConfigurationManager.AppSettings["username"].ToString();
            string password = ConfigurationManager.AppSettings["password"].ToString();
            string mailReceiver = ConfigurationManager.AppSettings["mailaddress"].ToString();

            var mailservice = new MailServiceExchange(userName, password, mailReceiver);
            return mailservice;
        }

        public static IMailService GetMyMailService()
        {
            var mailservice = HttpContext.Current == null
                        ? GetNewMailService()
                        : (HttpContext.Current.Items["MailService"] == null ? GetNewMailService() : HttpContext.Current.Items["MailService"] as MailServicePOP3SMTP);
            return mailservice;
        }

        public static void SendEmail(object mailObj)
        {
            string sender = ConfigurationManager.AppSettings["mailaddress"].ToString();
            IMailService mailservice = GetMyMailService();
            var mailWithContext = (mailObjWithContext)mailObj;
            MailObject mo = (MailObject)mailWithContext.mail;
            mo.MailSender = sender;
            mailservice.Send(mo);
            if (mo.MailAttachments != null)
            {
                try
                {
                    //System.IO.File.Delete(mo.MailAttachment);
                }
                catch (Exception ex)
                {
                    string strPath = mailWithContext.current.Request.PhysicalApplicationPath + "SendEmailLog.txt";
                    System.IO.File.AppendAllText(strPath, ex.Message + "\n" + ex.InnerException + "\n" + DateTime.Now.ToString() + "   \n");
                }
            }
        }

        public static void SendEmailNewThread(List<MailObject> mailObjs)
        {
            foreach (MailObject mailObj in mailObjs)
            {
                SendEmailNewThread(mailObj);
            }
        }

        public static void SendEmailNewThread(object mailObj)
        {
            try
            {
                ParameterizedThreadStart ParStart = new ParameterizedThreadStart(MailFactoryLoader.SendEmail);
                mailObjWithContext withContext = new mailObjWithContext { mail = (MailObject)mailObj, current = System.Web.HttpContext.Current };
                Thread myThread = new Thread(ParStart);
                myThread.Start(withContext);
                
            }
            catch (Exception ex)
            {
                string strPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "SendEmailLog.txt";
                System.IO.File.AppendAllText(strPath, ex.Message + "\n" + ex.InnerException + "\n" + DateTime.Now.ToString() + "   \n");
            }
        }
    }
    public class mailObjWithContext
    { 
        public MailObject mail{get;set;}
        public HttpContext current { get; set; }
    }
}
