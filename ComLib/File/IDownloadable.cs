using System.IO;

namespace ComLib.File
{
    public interface IDownloadable
    {
        string MIMEType { get; }
        MemoryStream DownloadStream { get; }
    }
}
