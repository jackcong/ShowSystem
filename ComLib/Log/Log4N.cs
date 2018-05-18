using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ComLib.Mail;

namespace ComLib.Log
{
    public class Log4N
    {
        public static void writeLog(string location,Exception ex, bool sendEmail)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Productivity.Logging");
            log.Info(DateTime.Now.ToString()+" ["+location+"] " + ex.Message, ex.InnerException);

            if (sendEmail)
            {
                var obj = ConfigurationManager.AppSettings["ExceptionMailReceiver"];
                string strMail = "";
                if (obj == null)
                {
                    strMail = "cong.lianjian@t2vsoft.com";
                }
                else
                {
                    strMail = obj.ToString();
                }

                string strContent = string.Empty;

                strContent += " [" + location + "] ";

                if (ex.Message != null)
                {
                    strContent += ex.Message;
                }
                if (ex.InnerException != null)
                {
                    strContent += ex.InnerException.Source + ex.InnerException.StackTrace;
                }

                MailObject mailObj = new MailObject();
                mailObj.MailReceiver = strMail;
                mailObj.MailSubject = "Exception From Productivity,Env: " + System.Web.HttpContext.Current.Request.Url.ToString() + "" + DateTime.Now.ToString();
                mailObj.MailBody = strContent;
                MailFactoryLoader.SendEmailNewThread(mailObj);
            }
        }
    }
}
