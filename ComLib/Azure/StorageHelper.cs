using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
namespace ComLib.Azure
{
    public class StorageHelper
    {
        /// <summary>
        /// Upload file to windows azure storage.
        /// </summary>
        /// <param name="folder">User folder in storage, couldn't have special charactor</param>
        /// <param name="inputStream">File stream</param>
        /// <param name="fileName">File name</param>
        /// <returns>Status of upoload</returns>
        public static bool UploadToStorge(string folder, Stream inputStream, string fileName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.AppSettings["AzureBlobStorage"]);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference(folder.ToLower().Replace("@", "at").Replace("_", "underscore").Replace("-", "dash").Replace(".", "dot"));
                container.CreateIfNotExists();

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.UploadFromStream(inputStream);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Download file from azure storage
        /// </summary>
        /// <param name="folder">User folder in storage, couldn't have special charactor</param>
        /// <param name="fileName"></param>
        public static void DownLoadFile(string folder, string fileName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.AppSettings["AzureBlobStorage"]);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference(folder.ToLower().Replace("@", "at").Replace("_", "underscore").Replace("-", "dash").Replace(".", "dot"));
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                if (blockBlob.Exists())
                {
                    using (var fileStream = System.IO.File.OpenWrite(fileName))
                    {
                        blockBlob.DownloadToStream(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Stream FileToStream(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        public static void StreamToFile(Stream stream, string fileName)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        } 
    }
}
