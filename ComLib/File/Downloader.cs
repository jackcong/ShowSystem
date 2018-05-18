using System.Web;

namespace ComLib.File
{
    public static class Downloader
    {
        public static void Download(this IDownloadable param, string fileName, HttpResponse response)
        {
            response.Clear();
            response.AppendHeader("Content-Type", param.MIMEType);
            response.AppendHeader
        ("Content-disposition", "attachment; filename=" + fileName);
            response.Write(param.DownloadStream);
            response.Flush();
            response.End();
        }

    }
}
