using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace ComLib.Compress
{
    public class Zip
    {
        public static bool ZipFile(string FileToZip, string ZipedFile, String Password)
        {
            if (!System.IO.File.Exists(FileToZip))
            {
                throw new System.IO.FileNotFoundException("NoFile: " + FileToZip + " Not Exist!");
            }
            System.IO.FileStream readFile = null;
            System.IO.FileStream zipFile = null;

            ZipOutputStream zipStream = null;
            ZipEntry zipEntry = null;

            bool res = true;
            try
            {
                if (System.IO.File.Exists(ZipedFile))
                {
                    System.IO.File.Delete(ZipedFile);
                }
                zipFile = System.IO.File.Create(ZipedFile);
                readFile = System.IO.File.OpenRead(FileToZip);
                zipEntry = new ZipEntry(System.IO.Path.GetFileName(FileToZip));
                zipStream = new ZipOutputStream(zipFile);
                zipEntry.DateTime = DateTime.Now;
                zipEntry.Size = readFile.Length;
                zipStream.PutNextEntry(zipEntry);
                zipStream.SetLevel(6);

                int stepLength = 40000000;
                int startPoint = 0;
                for (int i = 1; true; i++)
                {
                    try
                    {
                        if (startPoint >= readFile.Length)
                            break;
                        if (readFile.Length - startPoint <= stepLength)
                        {
                            stepLength = Convert.ToInt32(readFile.Length) - startPoint;
                        }
                        byte[] buffer = new byte[stepLength];
                        readFile.Read(buffer, 0, stepLength);

                        //ZipStream.Password = Password;
                        zipStream.Write(buffer, 0, stepLength);
                        startPoint += stepLength;

                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (System.Exception ex)
            {
                res = false;
            }
            finally
            {
                if (zipEntry != null)
                {
                    zipEntry = null;
                }
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (zipFile != null)
                {
                    zipFile.Close();
                    zipFile = null;
                }
                if (readFile != null)
                {
                    readFile.Close();
                }

                GC.Collect();
                GC.Collect(1);
            }

            return res;
        }

        public static bool ZipFile(List<String> FilesToZip, string ZipedFile, String Password)
        {
            for (int i = 0; i < FilesToZip.Count; i++)
            {
                if (!System.IO.File.Exists(FilesToZip[i]))
                {
                    throw new System.IO.FileNotFoundException("NoFile: " + FilesToZip[i] + " Not Exist!");
                }
            }

            FileStream readFile = null;
            FileStream zipFile = null;
            ZipOutputStream zipStream = null;
            ZipEntry zipEntry = null;

            bool res = true;
            try
            {
                zipFile = System.IO.File.Create(ZipedFile);
                zipStream = new ZipOutputStream(zipFile);
                for (int j = 0; j < FilesToZip.Count; j++)
                {
                    readFile = System.IO.File.OpenRead(FilesToZip[j]);
                    zipEntry = new ZipEntry(Path.GetFileName(FilesToZip[j]));
                    zipEntry.DateTime = DateTime.Now;
                    zipEntry.Size = readFile.Length;
                    zipStream.PutNextEntry(zipEntry);
                    zipStream.SetLevel(6);
                    int stepLength = 40000000;
                    int startPoint = 0;
                    for (int i = 1; true; i++)
                    {
                        try
                        {
                            if (startPoint >= readFile.Length)
                                break;
                            if (readFile.Length - startPoint <= stepLength)
                            {
                                stepLength = Convert.ToInt32(readFile.Length) - startPoint;
                            }
                            byte[] buffer = new byte[stepLength];
                            readFile.Read(buffer, 0, stepLength);

                            //ZipStream.Password = Password;
                            zipStream.Write(buffer, 0, stepLength);
                            startPoint += stepLength;

                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }
                    }
                    zipEntry = null;
                    readFile.Close();
                }

            }
            catch (System.Exception ex)
            {
                res = false;
            }
            finally
            {
                if (zipEntry != null)
                {
                    zipEntry = null;
                }
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (zipFile != null)
                {
                    zipFile.Close();
                    zipFile = null;
                }
                if (readFile != null)
                {
                    readFile.Close();
                }

                GC.Collect();
                GC.Collect(1);
            }

            return res;
        }
    }
}
