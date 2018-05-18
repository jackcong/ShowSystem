using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Mail
{
    public class MailObject
    {
        public string MailSender { get; set; }
        public string MailReceiver { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public List<MailAttachment> MailAttachments { get; set; }
        public DateTime SentOn { get; set; }
        public string ItemID { get; set; }

        public string EmlFileName { get; set; }

        public DateTime MailRecDate { get; set; }

        public int AttachmentFlag { get; set; }
    }
    public class MailAttachment
    {
        public string FileName { get; set; }
        public string FileGUIDName { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
    }
}
