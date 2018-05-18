using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ComLib.Mail
{
    public class MailServiceExchange : IMailService
    {
        static Account user = new Account();
        private int tempCurrentProcess;
        HttpSessionStateBase tempCurrent;
        ExchangeService service = null;

        public string MailReceiver;
        public string mailReceiverAddress;

        public MailServiceExchange(string EmailAddress, string pwd, string mailReceiver)
        {
            user.EMailAddress = EmailAddress;
            user.Password = pwd;
            service = MailAuth.ConnectToService(user);
            this.MailReceiver = mailReceiver;
        }

        public MailServiceExchange(string EmailAddress, string pwd, string mailReceiver, string mailReceiverAddress)
        {
            user.EMailAddress = EmailAddress;
            user.Password = pwd;
            service = MailAuth.ConnectToService(user);
            this.MailReceiver = mailReceiver;
            this.mailReceiverAddress = mailReceiverAddress;
        }

        public MailServiceExchange(string EmailAddress, string pwd, string exchangeVersion, HttpSessionStateBase current)
        {
            user.EMailAddress = EmailAddress;
            user.Password = pwd;
            ExchangeVersion ev = ExchangeVersion.Exchange2013;
            if (exchangeVersion == "Exchange2013")
            {
                ev = ExchangeVersion.Exchange2013;
            }
            else if (exchangeVersion == "Exchange2007_SP1")
            {
                ev = ExchangeVersion.Exchange2007_SP1;
            }
            else if (exchangeVersion == "Exchange2010")
            {
                ev = ExchangeVersion.Exchange2010;
            }
            else if (exchangeVersion == "Exchange2010_SP1")
            {
                ev = ExchangeVersion.Exchange2010_SP1;
            }
            else if (exchangeVersion == "Exchange2010_SP2")
            {
                ev = ExchangeVersion.Exchange2010_SP2;
            }
            user.Version = ev;
            tempCurrent = current;
            service = MailAuth.ConnectToService(user);
        }

        static PropertySet psPropset = new PropertySet()
        {
            RequestedBodyType = BodyType.Text,
            BasePropertySet = BasePropertySet.FirstClassProperties
        };

        public void Send(MailObject mail)
        {
            try
            {
                var message = new EmailMessage(service)
                {
                    From = mail.MailSender,
                    Subject = mail.MailSubject,
                    Body = mail.MailBody
                };

                if (mail.MailReceiver != null)
                    message.ToRecipients.AddRange(mail.MailReceiver.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray());
                if (mail.MailCC != null)
                    message.CcRecipients.AddRange(mail.MailCC.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray());
                if (mail.MailBCC != null)
                    message.BccRecipients.AddRange(mail.MailBCC.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray());

                if (mail.MailAttachments != null)
                {
                    foreach (MailAttachment MailAttachment in mail.MailAttachments)
                    {
                        if (!string.IsNullOrEmpty(MailAttachment.FilePath))
                        {
                            message.Attachments.AddFileAttachment(MailAttachment.FilePath);
                        }
                    }
                }

                message.SendAndSaveCopy();
            }
            catch (Exception ex)
            {
                Send(mail);
            }
        }

        public void SendWithCustom(MailObject mail)
        {
            var message = new EmailMessage(service)
            {
                From = mail.MailSender,
                Subject = mail.MailSubject,
                Body = mail.MailBody
            };
            if (mail.MailReceiver != null)
                message.ToRecipients.AddRange(mail.MailReceiver.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray());
            if (mail.MailCC != null)
                message.CcRecipients.AddRange(mail.MailCC.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray());
            if (mail.MailBCC != null)
                message.BccRecipients.AddRange(mail.MailBCC.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray());

            if (mail.MailAttachments != null)
            {
                foreach (MailAttachment mailAttachment in mail.MailAttachments)
                {
                    if (!string.IsNullOrEmpty(mailAttachment.FileName))
                    {
                        message.Attachments.AddFileAttachment(mailAttachment.FileName);
                    }
                }
            }
            message.SendAndSaveCopy();
        }

        public void Send(MailObject mail, bool isSaveEml = false, string fileDir = "", string sender = "Car")
        {
            var message = new EmailMessage(service)
            {
                From = mail.MailSender,
                Subject = mail.MailSubject,
                Body = mail.MailBody
            };
            if (mail.MailReceiver != null)
                message.ToRecipients.AddRange(mail.MailReceiver.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray());
            if (mail.MailCC != null)
                message.CcRecipients.AddRange(mail.MailCC.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray());
            if (mail.MailBCC != null)
                message.BccRecipients.AddRange(mail.MailBCC.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray());

            foreach (MailAttachment mailAttachment in mail.MailAttachments)
            {
                if (!string.IsNullOrEmpty(mailAttachment.FileName))
                {
                    message.Attachments.AddFileAttachment(mailAttachment.FileName);
                }
            }
            //ExtendedPropertyDefinition PR_DEFERRED_SEND_TIME = new ExtendedPropertyDefinition(16367,
            //                                                                           MapiPropertyType.SystemTime);
            //// Identify when the email message will be sent and delivered. The email message is delivered three minutes after the
            //// next line executes.
            //string sendTime = DateTime.Now.AddMinutes(10).ToUniversalTime().ToString();

            //// Set the extended properties with the dateTime for when the mail will be delivered.
            //message.SetExtendedProperty(PR_DEFERRED_SEND_TIME, sendTime);


            message.SendAndSaveCopy();
            if (isSaveEml)
            {
                EmailMessage msg = Read(mail.MailSubject);
                msg.Load(new PropertySet(ItemSchema.MimeContent));
                string fileName = DateTime.Now.Ticks.ToString() + ".eml";
                mail.EmlFileName = fileName;
                using (FileStream fs = new FileStream(fileDir + fileName, FileMode.OpenOrCreate))
                {
                    fs.Write(msg.MimeContent.Content, 0, msg.MimeContent.Content.Length);
                }
            }
        }

        public EmailMessage Read(string subject)
        {
            SearchFilter filter = new SearchFilter.IsEqualTo(EmailMessageSchema.Subject, subject);
            var results = service.FindItems(WellKnownFolderName.SentItems, filter, new ItemView(1));
            foreach (var result in results.Items)
            {
                EmailMessage msg = EmailMessage.Bind(service, result.Id, psPropset);
                return msg;
            }
            return null;
        }


        public List<MailObject> Read(string fileDir, List<string> fileType)
        {
            //PullMail(fileDir, fileType, receiver, regexs);
            SearchFilter filter = new SearchFilter.IsEqualTo(EmailMessageSchema.DisplayTo, this.MailReceiver);
            var results = service.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(10));
            var lst = new List<MailObject>();
            foreach (var result in results.Items)
            {
                EmailMessage msg = EmailMessage.Bind(service, result.Id, psPropset);
                var obj = new MailObject();
                obj.MailSubject = msg.Subject;

                obj.ItemID = result.Id.UniqueId;
                obj.MailBody = msg.Body;
                obj.MailSender = msg.Sender.Address;
                obj.MailReceiver = string.Join(";", msg.ToRecipients.Select(c => c.Address).ToList());
                obj.MailCC = string.Join(";", msg.CcRecipients.Select(c => c.Address).ToList());
                obj.MailBCC = string.Join(";", msg.BccRecipients.Select(c => c.Address).ToList());
                obj.MailRecDate = msg.DateTimeReceived;
                obj.SentOn = msg.DateTimeReceived;

                if (msg.HasAttachments && fileType.Count > 0)
                {
                    msg.Load(new PropertySet(EmailMessageSchema.Attachments));
                    var attachments = msg.Attachments;
                    List<MailAttachment> attachFiles = new List<MailAttachment>();
                    foreach (var attachment in attachments)
                    {
                        MailAttachment attachmentFile = new MailAttachment();
                        attachmentFile.FileName = attachment.Name;
                        attachFiles.Add(attachmentFile);
                    }
                    obj.MailAttachments = attachFiles;
                }
                lst.Add(obj);
            }
            return lst;
        }

        public List<MailObject> Read(string fileDir, List<string> fileType, List<Regex> regexs)
        {
            //PullMail(fileDir, fileType, receiver, regexs);
            SearchFilter filter = new SearchFilter.IsEqualTo(EmailMessageSchema.DisplayTo, this.MailReceiver);
            var results = service.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(10));
            var lst = new List<MailObject>();
            bool matched = false;
            if (regexs.Count == 0)
                matched = true;
            foreach (var result in results.Items)
            {
                foreach (var regex in regexs)
                {
                    if (!string.IsNullOrEmpty(result.Subject) && regex.IsMatch(result.Subject))
                    {
                        matched = true;
                        break;
                    }
                }
                if (!matched)
                    continue;
                if (regexs.Count != 0)
                    matched = false;
                EmailMessage msg = EmailMessage.Bind(service, result.Id, psPropset);
                var obj = new MailObject();
                obj.MailSubject = msg.Subject;

                obj.ItemID = result.Id.UniqueId;
                obj.MailBody = msg.Body;
                obj.MailSender = msg.Sender.Address;
                obj.MailReceiver = string.Join(";", msg.ToRecipients.Select(c => c.Address).ToList());
                obj.MailCC = string.Join(";", msg.CcRecipients.Select(c => c.Address).ToList());
                obj.MailBCC = string.Join(";", msg.BccRecipients.Select(c => c.Address).ToList());

                if (msg.HasAttachments && fileType.Count > 0)
                {
                    msg.Load(new PropertySet(EmailMessageSchema.Attachments));
                    var attachments = msg.Attachments;
                    List<MailAttachment> attachFiles = new List<MailAttachment>();
                    foreach (var attachment in attachments)
                    {
                        MailAttachment attachmentFile = new MailAttachment();
                        attachmentFile.FileName = attachment.Name;
                        attachFiles.Add(attachmentFile);
                    }
                    obj.MailAttachments = attachFiles;
                }
                lst.Add(obj);
            }
            return lst;
        }

        public List<MailObject> ReadWithSubject(string fileDir, List<string> fileTypes, string mailMoveToFolder, string subject, int limitOfMails = 20)
        {
            FindItemsResults<Item> rectimeresults = null;
            var lst = new List<MailObject>();
            //SearchFilter sf = new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, true);

            var folderResults = service.FindFolders(WellKnownFolderName.Inbox, new FolderView(20));
            FolderId fi = null;
            foreach (var result in folderResults)
            {
                if (result.DisplayName == mailMoveToFolder)
                {
                    fi = result.Id;
                }
            }

            rectimeresults = service.FindItems(WellKnownFolderName.Inbox, new ItemView(limitOfMails));

            foreach (var rectimeresult in rectimeresults.Items)
            {
                EmailMessage msg = EmailMessage.Bind(service, rectimeresult.Id, psPropset);
                if (string.IsNullOrEmpty(subject) || (!string.IsNullOrEmpty(subject) && msg.Subject.ToLower().Contains(subject)))
                {
                    if (msg.ToRecipients.Any(c => c.Address.ToLower() == this.MailReceiver.ToLower()))
                    {
                        var obj = new MailObject();
                        obj.MailSubject = msg.Subject;

                        obj.ItemID = rectimeresult.Id.UniqueId;
                        obj.MailBody = msg.Body;
                        obj.MailSender = msg.Sender.Address;
                        obj.MailReceiver = string.Join(";", msg.ToRecipients.Select(c => c.Address).ToList());
                        obj.MailCC = string.Join(";", msg.CcRecipients.Select(c => c.Address).ToList());
                        obj.MailBCC = string.Join(";", msg.BccRecipients.Select(c => c.Address).ToList());
                        obj.MailRecDate = msg.DateTimeReceived;
                        obj.SentOn = msg.DateTimeReceived;

                        if (msg.HasAttachments)
                        {
                            obj.AttachmentFlag = 1;
                        }
                        else
                        {
                            obj.AttachmentFlag = 0;
                        }
                        if (msg.HasAttachments && fileTypes.Count > 0)
                        {
                            List<MailAttachment> listFiles = new List<MailAttachment>();
                            msg.Load(new PropertySet(EmailMessageSchema.Attachments));
                            var attachments = msg.Attachments;
                            foreach (FileAttachment attachment in attachments)
                            {

                                string attachFileName = string.Empty;

                                MailAttachment attachmentFile = new MailAttachment();
                                if (attachment != null)
                                {
                                    attachFileName = attachment.Name;
                                    var strEx = attachFileName.Substring(attachFileName.LastIndexOf(".") + 1);
                                    foreach (string fileType in fileTypes)
                                    {
                                        if (fileType.Contains(strEx))
                                        {
                                            if (!Directory.Exists(fileDir))
                                            {
                                                Directory.CreateDirectory(fileDir);
                                            }
                                            string fileName = DateTime.Now.Ticks.ToString() + attachFileName;
                                            attachment.Load(fileDir + "\\" + fileName);
                                            
                                            attachmentFile.FileName = attachFileName;
                                            attachmentFile.FileGUIDName = fileName;
                                            attachmentFile.FileExtension = strEx;
                                            attachmentFile.FilePath = fileDir + "\\" + fileName;
                                            //StorageHelper.UploadToStorge(obj.MailSender.Replace("@", "at").Replace("_", "underscore").Replace("-", "dash").Replace(".", "dot"), StorageHelper.FileToStream(attachFileName), fileName);
                                            listFiles.Add(attachmentFile);
                                        }
                                    }
                                }
                                obj.MailAttachments = listFiles;
                            }
                        }
                        lst.Add(obj);
                        if (fi == null)
                        {
                            msg.Delete(DeleteMode.MoveToDeletedItems);
                        }
                        else
                        {
                            msg.Move(fi);
                        }
                    }
                }
            }
            return lst;
        }


        public List<MailObject> ReadWithSubject(string fileDir, List<string> fileTypes, string moveToFolder,string mailFromFolder, string subject, List<Regex> regexs, string andOr = "AND", int limitOfMails = 20)
        {
            FindItemsResults<Item> rectimeresults = null;
            var lst = new List<MailObject>();
            //SearchFilter sf = new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, true);

            var folderResults = service.FindFolders(WellKnownFolderName.Inbox, new FolderView(20));
            FolderId fi = null;
            FolderId fromFi = null;
            bool matched = false;
            foreach (var result in folderResults)
            {
                if (result.DisplayName == moveToFolder)
                {
                    fi = result.Id;
                }

                if (result.DisplayName == mailFromFolder)
                {
                    fromFi = result.Id;
                }
            }

            rectimeresults = service.FindItems(fromFi, new ItemView(limitOfMails));
            //rectimeresults = service.FindItems(WellKnownFolderName.Inbox, new ItemView(limitOfMails));
            if (regexs.Count == 0)
                matched = true;

            foreach (var rectimeresult in rectimeresults.Items)
            {
                foreach (var regex in regexs)
                {
                    if (!string.IsNullOrEmpty(rectimeresult.Subject) && regex.IsMatch(rectimeresult.Subject))
                    {
                        matched = true;
                        if (andOr == "OR")
                            break;
                        else
                            continue;
                    }
                    else
                    {
                        if (andOr == "AND")
                        {
                            matched = false;
                            break;
                        }
                    }
                }

                if (!matched)
                    continue;

                EmailMessage msg = EmailMessage.Bind(service, rectimeresult.Id, psPropset);

                if (string.IsNullOrEmpty(subject) || (!string.IsNullOrEmpty(subject) && msg.Subject == subject))
                {
                    if (msg.Sender.Address.ToString().ToLower() == this.mailReceiverAddress)
                    {
                        var obj = new MailObject();
                        obj.MailSubject = msg.Subject;

                        obj.ItemID = rectimeresult.Id.UniqueId;
                        obj.MailBody = msg.Body.Text.Replace("\r\n", "<br/>");
                        obj.MailSender = msg.Sender.Address;
                        obj.MailReceiver = string.Join(";", msg.ToRecipients.Select(c => c.Address).ToList());
                        obj.MailCC = string.Join(";", msg.CcRecipients.Select(c => c.Address).ToList());
                        obj.MailBCC = string.Join(";", msg.BccRecipients.Select(c => c.Address).ToList());
                        obj.MailRecDate = msg.DateTimeReceived;
                        obj.SentOn = msg.DateTimeReceived;

                        if (msg.HasAttachments)
                        {
                            obj.AttachmentFlag = 1;
                        }
                        else
                        {
                            obj.AttachmentFlag = 0;
                        }

                        if (msg.HasAttachments && fileTypes.Count > 0)
                        {
                            List<MailAttachment> listFiles = new List<MailAttachment>();
                            msg.Load(new PropertySet(EmailMessageSchema.Attachments));
                            var attachments = msg.Attachments;
                            foreach (FileAttachment attachment in attachments)
                            {
                                string attachFileName = string.Empty;
                                if (attachment != null)
                                {
                                    attachFileName = attachment.Name;
                                    var strEx = attachFileName.Substring(attachFileName.LastIndexOf(".") + 1);
                                    MailAttachment attachmentFile = new MailAttachment();
                                    attachmentFile.FileName = attachFileName;
                                    attachmentFile.FileExtension = strEx;
                                    foreach (string fileType in fileTypes)
                                    {
                                        if (fileType.Contains(strEx))
                                        {
                                            if (!Directory.Exists(fileDir))
                                            {
                                                Directory.CreateDirectory(fileDir);
                                            }
                                            string fileName = DateTime.Now.Ticks.ToString() + attachFileName;

                                            attachment.Load(fileDir + "\\" + fileName);

                                            attachmentFile.FileName = attachFileName;
                                            attachmentFile.FileGUIDName = fileName;
                                            attachmentFile.FileExtension = strEx;
                                            attachmentFile.FilePath = fileDir + "\\" + fileName;
                                            //StorageHelper.UploadToStorge(obj.MailSender.Replace("@", "at").Replace("_", "underscore").Replace("-", "dash").Replace(".", "dot"), StorageHelper.FileToStream(attachFileName), fileName);
                                            //file.Delete();
                                            listFiles.Add(attachmentFile);
                                        }
                                    }
                                }
                                obj.MailAttachments = listFiles;
                            }
                        }

                        //Save EML file
                        msg.Load(new PropertySet(ItemSchema.MimeContent));
                        string emlName = DateTime.Now.Ticks.ToString() + ".eml";
                        obj.EmlFileName = emlName;

                        if (!Directory.Exists(fileDir))
                        {
                            Directory.CreateDirectory(fileDir);
                        }

                        using (FileStream fs = new FileStream(fileDir + "\\" + emlName, FileMode.OpenOrCreate))
                        {
                            fs.Write(msg.MimeContent.Content, 0, msg.MimeContent.Content.Length);
                        }

                        lst.Add(obj);
                        if (fi == null)
                        {
                            //Console.Write("shold deleete");
                            msg.Delete(DeleteMode.MoveToDeletedItems);
                        }
                        else
                        {
                            msg.Move(fi);
                        }
                    }
                }
            }
            return lst;
        }

        public List<MailObject> ReadWithSubject(string fileDir, List<string> fileTypes, string moveToFolder, string subject, List<Regex> regexs, string andOr = "AND", int limitOfMails = 20)
        {
            FindItemsResults<Item> rectimeresults = null;
            var lst = new List<MailObject>();
            //SearchFilter sf = new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, true);

            var folderResults = service.FindFolders(WellKnownFolderName.Inbox, new FolderView(20));
            FolderId fi = null;
            bool matched = false;
            foreach (var result in folderResults)
            {
                if (result.DisplayName == moveToFolder)
                {
                    fi = result.Id;
                }
            }

            rectimeresults = service.FindItems(WellKnownFolderName.Inbox, new ItemView(limitOfMails));
            if (regexs.Count == 0)
                matched = true;
            foreach (var rectimeresult in rectimeresults.Items)
            {
                foreach (var regex in regexs)
                {
                    if (!string.IsNullOrEmpty(rectimeresult.Subject) && regex.IsMatch(rectimeresult.Subject))
                    {
                        matched = true;
                        if (andOr == "OR")
                            break;
                        else
                            continue;
                    }
                    else
                    {
                        if (andOr == "AND")
                        {
                            matched = false;
                            break;
                        }
                    }
                }
                if (!matched)
                    continue;
                EmailMessage msg = EmailMessage.Bind(service, rectimeresult.Id, psPropset);
                if (string.IsNullOrEmpty(subject) || (!string.IsNullOrEmpty(subject) && msg.Subject == subject))
                {
                    if (msg.ToRecipients.Any(c => c.Address.ToLower() == this.MailReceiver.ToLower()))
                    {
                        var obj = new MailObject();
                        obj.MailSubject = msg.Subject;

                        obj.ItemID = rectimeresult.Id.UniqueId;
                        obj.MailBody = msg.Body;
                        obj.MailSender = msg.Sender.Address;
                        obj.MailReceiver = string.Join(";", msg.ToRecipients.Select(c => c.Address).ToList());
                        obj.MailCC = string.Join(";", msg.CcRecipients.Select(c => c.Address).ToList());
                        obj.MailBCC = string.Join(";", msg.BccRecipients.Select(c => c.Address).ToList());
                        obj.MailRecDate = msg.DateTimeReceived;
                        obj.SentOn = msg.DateTimeReceived;



                        if (msg.HasAttachments)
                        {
                            obj.AttachmentFlag = 1;
                        }
                        else
                        {
                            obj.AttachmentFlag = 0;
                        }
                        if (msg.HasAttachments && fileTypes.Count > 0)
                        {
                            List<MailAttachment> listFiles = new List<MailAttachment>();
                            msg.Load(new PropertySet(EmailMessageSchema.Attachments));
                            var attachments = msg.Attachments;
                            foreach (FileAttachment attachment in attachments)
                            {
                                string attachFileName = string.Empty;
                                if (attachment != null)
                                {
                                    attachFileName = attachment.Name;
                                    var strEx = attachFileName.Substring(attachFileName.LastIndexOf(".") + 1);
                                    MailAttachment attachmentFile = new MailAttachment();
                                    attachmentFile.FileName = attachFileName;
                                    attachmentFile.FileExtension = strEx;
                                    foreach (string fileType in fileTypes)
                                    {
                                        if (fileType.Contains(strEx))
                                        {
                                            if (!Directory.Exists(fileDir))
                                            {
                                                Directory.CreateDirectory(fileDir);
                                            }
                                            string fileName = DateTime.Now.Ticks.ToString() + attachFileName;
                                            attachment.Load(fileDir + "\\" + fileName);

                                            attachmentFile.FileName = attachFileName;
                                            attachmentFile.FileGUIDName = fileName;
                                            attachmentFile.FileExtension = strEx;
                                            attachmentFile.FilePath = fileDir + "\\" + fileName;
                                            //StorageHelper.UploadToStorge(obj.MailSender.Replace("@", "at").Replace("_", "underscore").Replace("-", "dash").Replace(".", "dot"), StorageHelper.FileToStream(attachFileName), fileName);
                                            //file.Delete();
                                            listFiles.Add(attachmentFile);
                                        }
                                    }
                                }
                                obj.MailAttachments = listFiles;
                            }
                        }

                        //Save EML file
                        msg.Load(new PropertySet(ItemSchema.MimeContent));
                        string emlName = DateTime.Now.Ticks.ToString() + ".eml";
                        obj.EmlFileName = emlName;
                        using (FileStream fs = new FileStream(fileDir + "\\" + emlName, FileMode.OpenOrCreate))
                        {
                            fs.Write(msg.MimeContent.Content, 0, msg.MimeContent.Content.Length);
                        }

                        lst.Add(obj);
                        if (fi == null)
                        {
                            msg.Delete(DeleteMode.MoveToDeletedItems);
                        }
                        else
                        {
                            msg.Move(fi);
                        }
                    }
                }
            }
            return lst;
        }
       
        public int GetCurrentProcess()
        {
            return tempCurrentProcess;
        }

        public List<MailObject> GetAllNewMail(string fileDir, MailObject em, string mailItemID = "")//string receiver, List<string> fileType,
        {
            tempCurrentProcess = 2;

            tempCurrent[user.EMailAddress] = 2;
            FindItemsResults<Item> rectimeresults = null;
            var lst = new List<MailObject>();
            if (em != null)
            {
                //SearchFilter filter = new SearchFilter.IsEqualTo(EmailMessageSchema.Id, mailItemID);
                //var results = service.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(10));
                //filter = new SearchFilter.IsGreaterThanOrEqualTo(EmailMessageSchema.DateTimeReceived, results.Items[0].DateTimeReceived);
                //rectimeresults = service.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(50));
                try
                {
                    mailItemID = em.ItemID;
                    var result = Item.Bind(service, new ItemId(mailItemID));
                    if (result != null)
                    {
                        SearchFilter filter = new SearchFilter.IsGreaterThanOrEqualTo(EmailMessageSchema.DateTimeReceived, result.DateTimeReceived);
                        rectimeresults = service.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(200));
                    }
                    else
                    {
                        return lst;
                    }
                }
                catch (Exception ex)
                {
                    SearchFilter sf = new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false);

                    rectimeresults = service.FindItems(WellKnownFolderName.Inbox, sf, new ItemView(int.MaxValue));
                }
            }
            else
            {
                SearchFilter sf = new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false);
                //rectimeresults = service.FindItems(WellKnownFolderName.Inbox, new ItemView(50));


                //rectimeresults = service.FindItems(WellKnownFolderName.Inbox, sf, new ItemView(int.MaxValue));
                rectimeresults = service.FindItems(WellKnownFolderName.Inbox, sf, new ItemView(int.MaxValue));
            }
            //SearchFilter filter = new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false);
            //rectimeresults = service.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(50));

            int i = 0;

            foreach (var rectimeresult in rectimeresults.Items)
            {
                i++;
                tempCurrentProcess = i * (96 / rectimeresults.Items.Count) + 2;
                tempCurrent[user.EMailAddress] = tempCurrentProcess;
                //if ((((rectimeresult.DisplayCc != null) && (rectimeresult.DisplayCc.ToUpper() == receiver.ToUpper()))
                //            || ((rectimeresult.DisplayTo != null) && (rectimeresult.DisplayTo.ToUpper() == receiver.ToUpper()))))
                if (rectimeresult.DisplayCc != null
                            || rectimeresult.DisplayTo != null)
                {
                    if (rectimeresult.Id.UniqueId == mailItemID)
                    {
                        break;
                    }
                    EmailMessage msg = EmailMessage.Bind(service, rectimeresult.Id, psPropset);
                    var obj = new MailObject();
                    obj.MailSubject = msg.Subject;

                    obj.ItemID = rectimeresult.Id.UniqueId;
                    obj.MailBody = msg.Body;
                    obj.MailSender = msg.Sender.Address;
                    obj.MailReceiver = string.Join(";", msg.ToRecipients.Select(c => c.Address).ToList());
                    obj.MailCC = string.Join(";", msg.CcRecipients.Select(c => c.Address).ToList());
                    obj.MailBCC = string.Join(";", msg.BccRecipients.Select(c => c.Address).ToList());
                    obj.MailRecDate = msg.DateTimeReceived;
                    obj.SentOn = msg.DateTimeReceived;
                    if (msg.HasAttachments)
                    {
                        obj.AttachmentFlag = 1;
                    }
                    else
                    {
                        obj.AttachmentFlag = 0;
                    }
                    if (msg.HasAttachments)// && fileType.Count > 0
                    {
                        msg.Load(new PropertySet(EmailMessageSchema.Attachments));
                        var attachments = msg.Attachments;
                        List<MailAttachment> attachFileList = new List<MailAttachment>();

                        foreach (var attachment in attachments)
                        {
                            FileAttachment fileAttachment = attachment as FileAttachment;
                            if (fileAttachment != null)
                            {
                                MailAttachment attachmentFile = new MailAttachment();

                                string fileName = string.Empty;
                                if (fileAttachment.Name.IndexOf(".") > 0)
                                {
                                    var strEx = fileAttachment.Name.Substring(fileAttachment.Name.LastIndexOf("."));
                                    //if (!fileType.Contains(strEx))
                                    //    continue;
                                    attachmentFile.FileName = fileAttachment.Name;
                                    attachmentFile.FileExtension = strEx;
                                    fileName = DateTime.Now.Ticks.ToString() + strEx;
                                    attachmentFile.FileGUIDName = fileName;
                                }
                                else
                                {
                                    fileName = fileAttachment.Name;
                                }

                                fileAttachment.Load(fileDir + fileName);
                                //attachFileList.Add(fileAttachment.Name);
                                attachFileList.Add(attachmentFile);
                                //obj.MailAttachment = fileAttachment.Name;
                                //obj.SaveFileName = fileName;
                            }
                        }
                        obj.MailAttachments = attachFileList;
                    }
                    lst.Add(obj);
                    //msg.Delete(DeleteMode.SoftDelete);
                    //msg.IsRead = true;
                    //msg.Update(ConflictResolutionMode.AlwaysOverwrite);
                }
            }
            return lst;
        }

        public IMailService AuthenticateService(EmailConfigModel eModel, out bool connectBool)
        {
            MailServiceExchange mailservice = null;
            try
            {
                string strExcVer = eModel.Domain;

                mailservice = new MailServiceExchange(eModel.Email, eModel.Password, strExcVer, eModel.tempCurrent);
                connectBool = true;
                return mailservice;
            }
            catch (Exception ex)
            {
                connectBool = false;
                return null;
            }

        }

        public void SaveEmlFile(MailObject mail, string fileDir)
        {
            EmailMessage msg = ReadFromInbox(mail.ItemID);
            msg.Load(new PropertySet(ItemSchema.MimeContent));
            string fileName = DateTime.Now.Ticks.ToString() + ".eml";
            mail.EmlFileName = fileName;
            using (FileStream fs = new FileStream(fileDir + fileName, FileMode.OpenOrCreate))
            {
                fs.Write(msg.MimeContent.Content, 0, msg.MimeContent.Content.Length);
            }
        }
        private EmailMessage ReadFromInbox(string itemID)
        {
            SearchFilter filter = new SearchFilter.IsEqualTo(EmailMessageSchema.Id, itemID);
            var results = service.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(1));
            foreach (var result in results.Items)
            {
                EmailMessage msg = EmailMessage.Bind(service, result.Id, psPropset);
                return msg;
            }
            return null;
        }

    }
}
