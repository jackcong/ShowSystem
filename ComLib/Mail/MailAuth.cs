using Microsoft.Exchange.WebServices.Data;
using System;
using System.Net;
namespace ComLib.Mail
{
    public class Account
    {
        ExchangeVersion ev = ExchangeVersion.Exchange2013;
        public ExchangeVersion Version { get { return ev; } set { ev = value; } }
        public string EMailAddress { get; set; }
        public string Password { get; set; }
        public Uri AutodiscoverUrl { get; set; }
    }
    public class MailAuth
    {
        static MailAuth()
        {
            CertificateCallback.Initialize();
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }

            return result;
        }

        public static ExchangeService ConnectToService(Account userData)
        {
            return ConnectToService(userData, null);
        }

        public static ExchangeService ConnectToService(Account userData, ITraceListener listener)
        {
            ExchangeService service = new ExchangeService(userData.Version);

            if (listener != null)
            {
                service.TraceListener = listener;
                service.TraceFlags = TraceFlags.All;
                service.TraceEnabled = true;
            }

            service.Credentials = new NetworkCredential(userData.EMailAddress, userData.Password);
            userData.AutodiscoverUrl = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
            if (userData.AutodiscoverUrl == null)
            {
                service.AutodiscoverUrl(userData.EMailAddress, RedirectionUrlValidationCallback);
                userData.AutodiscoverUrl = service.Url;
            }
            else
            {
                service.Url = userData.AutodiscoverUrl;
            }

            return service;
        }

        public static ExchangeService ConnectToServiceWithImpersonation(
          Account userData,
          string impersonatedUserSMTPAddress)
        {
            return ConnectToServiceWithImpersonation(userData, impersonatedUserSMTPAddress, null);
        }

        public static ExchangeService ConnectToServiceWithImpersonation(
          Account userData,
          string impersonatedUserSMTPAddress,
          ITraceListener listener)
        {
            ExchangeService service = new ExchangeService(userData.Version);

            if (listener != null)
            {
                service.TraceListener = listener;
                service.TraceFlags = TraceFlags.All;
                service.TraceEnabled = true;
            }

            service.Credentials = new NetworkCredential(userData.EMailAddress, userData.Password);

            ImpersonatedUserId impersonatedUserId =
              new ImpersonatedUserId(ConnectingIdType.SmtpAddress, impersonatedUserSMTPAddress);

            service.ImpersonatedUserId = impersonatedUserId;

            if (userData.AutodiscoverUrl == null)
            {
                service.AutodiscoverUrl(userData.EMailAddress, RedirectionUrlValidationCallback);
                userData.AutodiscoverUrl = service.Url;
            }
            else
            {
                service.Url = userData.AutodiscoverUrl;
            }

            return service;
        }
    }

    public class CertificateCallback
    {
        static CertificateCallback()
        {
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
        }

        public static void Initialize()
        {
        }

        private static bool CertificateValidationCallBack(
             object sender,
             System.Security.Cryptography.X509Certificates.X509Certificate certificate,
             System.Security.Cryptography.X509Certificates.X509Chain chain,
             System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}