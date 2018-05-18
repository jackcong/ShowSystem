using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPop.Pop3;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Web;
using OpenPop.Mime.Header;
using OpenPop.Mime;
using System.IO;
using System.Text.RegularExpressions;

namespace ComLib.Mail
{
    public class MailServicePOP3SMTP : IMailService
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public int SendPort { get; set; }
        public string UseSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MailReceiver { get; set; }
        public MailServicePOP3SMTP()
        {
        }
        public MailServicePOP3SMTP(string hostName, int sendPort, int port, string useSSL, string userName, string password, string mailReceiver)
        {
            this.HostName = hostName; this.SendPort = SendPort; this.Port = port; this.UseSSL = useSSL; this.UserName = userName; this.Password = password; this.MailReceiver = mailReceiver;
        }
        /// <summary>
        /// Function to send email.
        /// </summary>
        /// <param name="mailObj">Mail object, with subject, receiver, sender and body.</param>
        public void Send(MailObject mailObj)
        {
            try
            {
                SmtpClient smtp = new SmtpClient(HostName, SendPort);
                MailMessage message = new MailMessage();
                mailObj.MailBCC = "Faye.Gu@t2vsoft.com";
                message.IsBodyHtml = true;
                message.From = new MailAddress(mailObj.MailSender);

                //if (!string.IsNullOrEmpty(mailObj.MailAttachment))
                //{
                //    Attachment attachment = new Attachment(mailObj.MailAttachment);
                //    message.Attachments.Add(attachment);
                //}

                if (mailObj.MailAttachments != null)
                {
                    foreach (MailAttachment attachFile in mailObj.MailAttachments)
                    {
                        Attachment attachment = new Attachment(attachFile.FileName);
                        message.Attachments.Add(attachment);
                    }
                }

                if (mailObj.MailReceiver.IndexOf(';') != -1)
                {
                    string[] strMailArray = mailObj.MailReceiver.Split(';');
                    foreach (string strEmail in strMailArray)
                    {
                        if (strEmail.Trim() != "" && !message.To.Contains(new MailAddress(strEmail.Trim())))
                        {
                            message.To.Add(new MailAddress(strEmail));
                        }
                    }
                }
                else
                {
                    message.To.Add(new MailAddress(mailObj.MailReceiver));
                }

                if (mailObj.MailCC != null && mailObj.MailCC != "")
                {
                    if (mailObj.MailCC.IndexOf(';') != -1)
                    {
                        string[] strMailArray = mailObj.MailCC.Split(';');
                        foreach (string strEmail in strMailArray)
                        {
                            if (strEmail.Trim() != "" && !message.To.Contains(new MailAddress(strEmail.Trim())) && !message.CC.Contains(new MailAddress(strEmail.Trim())))
                            {
                                message.CC.Add(new MailAddress(strEmail));
                            }
                        }
                    }
                    else
                    {
                        if (mailObj.MailCC != "" && !message.To.Contains(new MailAddress(mailObj.MailCC)) && !message.CC.Contains(new MailAddress(mailObj.MailCC)))
                        {
                            message.CC.Add(new MailAddress(mailObj.MailCC));
                        }
                    }

                }
                if (mailObj.MailBCC != null)
                {
                    message.Bcc.Add(new MailAddress(mailObj.MailBCC));
                }

                message.Subject = mailObj.MailSubject;
                message.Body = mailObj.MailBody;
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                smtp.Credentials = new NetworkCredential(this.UserName, this.Password);
                smtp.EnableSsl = true;
                smtp.Timeout = 300000;
                smtp.Send(message);
                smtp.Dispose();
                message.Dispose();
            }
            catch (Exception ex)
            {
                var k = ex;
                //throw ex;
            }
        }

        public List<MailObject> Read(string fileDir, List<string> fileTypes)
        {
            List<MailObject> mailList = new List<MailObject>();
            try
            {
                using (Pop3Client client = new Pop3Client())
                {
                    // Connect to the server
                    client.Connect(this.HostName, this.Port, Boolean.Parse(this.UseSSL));

                    // Authenticate ourselves towards the server
                    client.Authenticate(this.UserName, this.Password);

                    int messageCount = client.GetMessageCount();

                    // Run trough each of these messages and download the headers
                    for (int messageItem = messageCount; messageItem > 0; messageItem--)
                    {
                        MailObject mailObj = new MailObject();
                        MessageHeader headers = client.GetMessageHeaders(messageItem);
                        RfcMailAddress from = headers.From;
                        Message msg = client.GetMessage(messageItem);

                        if (msg.Headers.To.Any(c => c.Address.ToLower() == this.MailReceiver.ToLower()))
                        {

                            MessagePart plainText = msg.FindFirstPlainTextVersion();
                            if (plainText != null)
                            {
                                // We found some plaintext!
                                mailObj.MailBody += plainText.GetBodyAsText();
                            }
                            else
                            {
                                // Might include a part holding html instead
                                MessagePart html = msg.FindFirstHtmlVersion();
                                if (html != null)
                                {
                                    // We found some html!
                                    mailObj.MailBody += html.GetBodyAsText();
                                }
                            }

                            string attachFileName = string.Empty;
                            string fileName = string.Empty;

                            List<MailAttachment> attachments = new List<MailAttachment>();
                            foreach (string fileType in fileTypes)
                            {
                                if (fileType != string.Empty)
                                {
                                    List<MessagePart> list = msg.FindAllAttachments();
                                    foreach (MessagePart attachment in list)
                                    {
                                        MailAttachment fileAttachment = new MailAttachment();
                                        fileAttachment.FileName = attachment.FileName;
                                        fileAttachment.FileExtension = attachment.FileName.Substring(attachment.FileName.LastIndexOf(".") + 1);
                                        if (attachment.FileName.Contains(fileType))
                                        {
                                            attachFileName = attachment.FileName;
                                            fileName = DateTime.Now.Ticks.ToString() + fileType;
                                            FileInfo fi = new FileInfo(fileDir + "\\" + fileName);
                                            fileAttachment.FileGUIDName = fileName;
                                            fileAttachment.FilePath = fileDir + "\\" + fileName;
                                            attachment.Save(fi);
                                            break;
                                        }
                                    }
                                }
                            }
                            mailObj.MailSubject = headers.Subject;
                            //Current only capture first receiver, will improve it in the future.
                            mailObj.MailReceiver = headers.To[0].ToString();
                            mailObj.MailSender = from.Address;
                            UnicodeEncoding encoding = new UnicodeEncoding();
                            mailList.Add(mailObj);

                            client.DeleteMessage(messageItem);
                        }
                    }
                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mailList;
        }


        
        public List<MailObject> ReadWithSubject(string fileDir, List<string> fileTypes, string moveToFolder, string mailFromFolder, string subject, List<Regex> regexs, string andOr = "AND", int limitOfMails = 20)
        {
            //add this method for implement the interface,this method used in exchange services add by jack2015-9-18.
            return null;
        }

        public List<MailObject> ReadWithSubject(string fileDir, List<string> fileTypes, string moveToFolder, string subject, List<Regex> regexs,string andOr = "AND", int limitOfMails = 20)
        {
            List<MailObject> mailList = new List<MailObject>();
            try
            {
                using (Pop3Client client = new Pop3Client())
                {
                    // Connect to the server
                    client.Connect(this.HostName, this.Port, Boolean.Parse(this.UseSSL));

                    // Authenticate ourselves towards the server
                    client.Authenticate(this.UserName, this.Password);

                    int messageCount = client.GetMessageCount();

                    // Run trough each of these messages and download the headers
                    for (int messageItem = messageCount; messageItem > 0; messageItem--)
                    {
                        MailObject mailObj = new MailObject();
                        MessageHeader headers = client.GetMessageHeaders(messageItem);
                        RfcMailAddress from = headers.From;
                        Message msg = client.GetMessage(messageItem);
                        bool matched = false;
                        foreach (Regex regex in regexs)
                        {
                            if (regex.IsMatch(msg.Headers.Subject))
                            {
                                matched = true;
                            }
                            else
                            {
                                matched = false;
                                break;
                            }
                        }
                        if (msg.Headers.To.Any(c => c.Address.ToLower() == this.MailReceiver.ToLower()) && matched)
                        {
                            MessagePart plainText = msg.FindFirstPlainTextVersion();
                            if (plainText != null)
                            {
                                // We found some plaintext!
                                mailObj.MailBody += plainText.GetBodyAsText();
                            }
                            else
                            {
                                // Might include a part holding html instead
                                MessagePart html = msg.FindFirstHtmlVersion();
                                if (html != null)
                                {
                                    // We found some html!
                                    mailObj.MailBody += html.GetBodyAsText();
                                }
                            }

                            string attachFileName = string.Empty;
                            string fileName = string.Empty;
                            bool nosuchAttachment = true;
                            List<MailAttachment> attachments = new List<MailAttachment>();
                            foreach (string fileType in fileTypes)
                            {
                                if (fileType != string.Empty)
                                {
                                    
                                    List<MessagePart> list = msg.FindAllAttachments();
                                    foreach (MessagePart attachment in list)
                                    {
                                        MailAttachment fileAttachment = new MailAttachment();
                                        fileAttachment.FileName = attachment.FileName;
                                        fileAttachment.FileExtension = attachment.FileName.Substring(attachment.FileName.LastIndexOf(".") + 1);
                                        if (fileType != "" && attachment.FileName.IndexOf(fileType) != -1)
                                        {
                                            attachFileName = attachment.FileName;

                                            string strEx = attachFileName.Substring(attachFileName.LastIndexOf("."));

                                            fileName = DateTime.Now.Ticks.ToString() + strEx;
                                            FileInfo fi = new FileInfo(fileDir + "\\" + fileName);
                                            attachment.Save(fi);
                                            fileAttachment.FileGUIDName = fileName;
                                            fileAttachment.FilePath = fileDir + "\\" + fileName;
                                            nosuchAttachment = false;
                                        }
                                        else
                                        {
                                            nosuchAttachment = true;
                                        }
                                        attachments.Add(fileAttachment);
                                    }

                                }
                            }
                            mailObj.MailSubject = headers.Subject;
                            //Current only capture first receiver, will improve it in the future.
                            mailObj.MailReceiver = headers.To[0].ToString();
                            mailObj.MailSender = from.Address;
                            mailObj.MailAttachments = attachments;
                            mailObj.SentOn = headers.DateSent;
                            UnicodeEncoding encoding = new UnicodeEncoding();
                            mailList.Add(mailObj);
                            if (nosuchAttachment == false)
                                client.DeleteMessage(messageItem);
                        }
                    }
                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mailList;
        }

        public List<MailObject> ReadWithSubject(string fileDir, List<string> fileTypes, string moveToFolder,string subject, int limitOfMails = 20)
        {
            List<MailObject> mailList = new List<MailObject>();
            try
            {
                using (Pop3Client client = new Pop3Client())
                {
                    // Connect to the server
                    client.Connect(this.HostName, this.Port, Boolean.Parse(this.UseSSL));

                    // Authenticate ourselves towards the server
                    client.Authenticate(this.UserName, this.Password);

                    int messageCount = client.GetMessageCount();

                    // Run trough each of these messages and download the headers
                    for (int messageItem = messageCount; messageItem > 0; messageItem--)
                    {
                        MailObject mailObj = new MailObject();
                        MessageHeader headers = client.GetMessageHeaders(messageItem);
                        if (headers.Subject.ToLower().Contains(subject))
                        {
                            RfcMailAddress from = headers.From;
                            Message msg = client.GetMessage(messageItem);

                            if (msg.Headers.To.Any(c => c.Address.ToLower() == this.MailReceiver.ToLower())) //&& matched
                            {
                                MessagePart plainText = msg.FindFirstPlainTextVersion();
                                if (plainText != null)
                                {
                                    // We found some plaintext!
                                    mailObj.MailBody += plainText.GetBodyAsText();
                                }
                                else
                                {
                                    // Might include a part holding html instead
                                    MessagePart html = msg.FindFirstHtmlVersion();
                                    if (html != null)
                                    {
                                        // We found some html!
                                        mailObj.MailBody += html.GetBodyAsText();
                                    }
                                }

                                string attachFileName = string.Empty;
                                string fileName = string.Empty;
                                bool nosuchAttachment = true;
                                List<MailAttachment> attachments = new List<MailAttachment>();
                                foreach (string fileType in fileTypes)
                                {

                                    if (fileType != string.Empty)
                                    {
                                        List<MessagePart> list = msg.FindAllAttachments();
                                        foreach (MessagePart attachment in list)
                                        {

                                            MailAttachment fileAttachment = new MailAttachment();
                                            fileAttachment.FileName = attachment.FileName;
                                            fileAttachment.FileExtension = attachment.FileName.Substring(attachment.FileName.LastIndexOf(".") + 1);

                                            if (attachment.FileName.IndexOf(fileType) != -1)//attachment.FileName.ToLower().Contains("spa order filter")
                                            {
                                                attachFileName = attachment.FileName;
                                                string strEx = attachFileName.Substring(attachFileName.LastIndexOf("."));

                                                fileName = DateTime.Now.Ticks.ToString() + strEx;
                                                FileInfo fi = new FileInfo(fileDir + "\\" + fileName);
                                                attachment.Save(fi);
                                                fileAttachment.FileGUIDName = fileName;
                                                fileAttachment.FilePath = fileDir + "\\" + fileName;
                                                nosuchAttachment = false;
                                            }
                                            else
                                            {
                                                nosuchAttachment = false;
                                            }
                                            attachments.Add(fileAttachment);
                                        }
                                        
                                    }
                                    else
                                    {
                                        nosuchAttachment = false;
                                    }
                                }
                                mailObj.MailSubject = headers.Subject;
                                //Current only capture first receiver, will improve it in the future.
                                mailObj.MailReceiver = headers.To[0].ToString();
                                mailObj.MailSender = from.Address;
                                mailObj.MailAttachments = attachments;
                                mailObj.SentOn = headers.DateSent;
                                UnicodeEncoding encoding = new UnicodeEncoding();
                                mailList.Add(mailObj);
                                if (nosuchAttachment == false)
                                    client.DeleteMessage(messageItem);
                            }

                        }
                    }
                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mailList;
        }

    }
}
