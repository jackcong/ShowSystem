using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ComLib.File.Csv
{
    public static class CSVProcessor
    {
        public static List<string> CSVParseRow(StreamReader sr)
        {
            bool stringStart = false, stringEnd = false;
            const char separator = ',', quote = '\"', whiteSpace = ' ', cr = '\r', lf = '\n';
            int c1, c2;
            StringBuilder sb = new StringBuilder();
            string buff = null;
            List<string> col = new List<string>();

            c2 = sr.Read();

            while (c2 != -1)
            {
                c1 = sr.Read();
                switch (c1)
                {
                    case separator:
                        switch (c2)
                        {
                            case separator:
                                //,,
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;

                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    col.Add(buff);
                                    buff = null;
                                    sb = new StringBuilder();
                                    stringStart = false;
                                    stringEnd = false;
                                }
                                c2 = c1;
                                break;
                            case quote:
                                //,"
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        stringEnd = true;
                                        buff = sb.ToString();
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        stringStart = true;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                }
                                c2 = c1;
                                break;
                            case whiteSpace:
                                //,_
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        //ignore
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                }
                                c2 = c1;
                                break;
                            case cr:
                                throw new InvalidDataException("invalid csv file");
                            case lf:
                                throw new InvalidDataException("invalid csv file");
                            default:
                                //,a
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                }
                                c2 = c1;
                                break;
                        }
                        break;
                    case quote:
                        switch (c2)
                        {
                            case separator:
                                //",
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    col.Add(buff);
                                    buff = null;
                                    sb = new StringBuilder();
                                    stringStart = false;
                                    stringEnd = false;
                                }
                                c2 = c1;
                                break;
                            case quote:
                                //""
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                        c1 = sr.Read();
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        stringStart = true;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                            case whiteSpace:
                                //"_
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        //ignore
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                            case cr:
                                throw new InvalidDataException("invalid csv file");
                            case lf:
                                throw new InvalidDataException("invalid csv file");
                            default:
                                //"a
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append((char)c2);
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                        }
                        break;
                    case whiteSpace:
                        switch (c2)
                        {
                            case separator:
                                //_,
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    col.Add(buff);
                                    buff = null;
                                    sb = new StringBuilder();
                                    stringStart = false;
                                    stringEnd = false;
                                }
                                c2 = c1;
                                break;
                            case quote:
                                //_"
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        buff = sb.ToString();
                                        stringEnd = true;
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        stringStart = true;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                            case whiteSpace:
                                //__
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        //ignore
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        //ignore
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                            case cr:
                                throw new InvalidDataException("invalid csv file");
                            case lf:
                                throw new InvalidDataException("invalid csv file");
                            default:
                                //_a
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append((char)c2);
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                        }
                        break;
                    case cr:
                        switch (c2)
                        {
                            case separator:
                                //cr,
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    col.Add(buff);
                                    buff = null;
                                    sb = new StringBuilder();
                                    stringStart = false;
                                    stringEnd = false;
                                }
                                c2 = c1;
                                break;
                            case quote:
                                //cr"
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        buff = sb.ToString();
                                        stringEnd = true;
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        stringStart = true;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                            case whiteSpace:
                                //cr_
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        //ignore
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                }
                                c2 = c1;
                                break;
                            case cr:
                                throw new InvalidDataException("invalid csv file");
                                break;
                            case lf:
                                //crlf
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                        return col;
                                    }
                                    else
                                    {
                                        sb.Append("\r\n");
                                        c1 = sr.Read();
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                        return col;
                                    }
                                    else
                                    {
                                        buff = sb.ToString();
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                        return col;
                                    }
                                }
                                c2 = c1;
                                break;
                            default:
                                //cra
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                }
                                c2 = c1;
                                break;
                        }
                        break;
                    case lf:
                        switch (c2)
                        {
                            case separator:
                                //lf,
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    col.Add(buff);
                                    buff = null;
                                    sb = new StringBuilder();
                                    stringStart = false;
                                    stringEnd = false;
                                }
                                c2 = c1;
                                break;
                            case quote:
                                //lf"
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        buff = sb.ToString();
                                        stringEnd = true;
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        stringStart = true;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                            case whiteSpace:
                                //lf_
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        //ignore
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                }
                                c2 = c1;
                                break;
                            case cr:
                                //lfcr
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                        return col;
                                    }
                                    else
                                    {
                                        sb.Append("\r\n");
                                        c1 = sr.Read();
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                        return col;
                                    }
                                    else
                                    {
                                        buff = sb.ToString();
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                        return col;
                                    }
                                }
                                c2 = c1;
                                break;
                            case lf:
                                throw new InvalidDataException("invalid csv file");
                            default:
                                //lfa
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                        buff = sb.ToString();
                                    }
                                }
                                c2 = c1;
                                break;
                        }
                        break;
                    default:
                        switch (c2)
                        {
                            case separator:
                                //a,
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        col.Add(buff);
                                        buff = null;
                                        sb = new StringBuilder();
                                        stringStart = false;
                                        stringEnd = false;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    col.Add(buff);
                                    buff = null;
                                    sb = new StringBuilder();
                                    stringStart = false;
                                    stringEnd = false;
                                }
                                c2 = c1;
                                break;
                            case quote:
                                //a"
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        buff = sb.ToString();
                                        stringEnd = true;
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        stringStart = true;
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                            case whiteSpace:
                                //a_
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        //ignore
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        //ignore
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                            case cr:
                                throw new InvalidDataException("invalid csv file");
                            case lf:
                                throw new InvalidDataException("invalid csv file");
                            default:
                                //aa
                                if (stringStart)
                                {
                                    if (stringEnd)
                                    {
                                        throw new InvalidDataException("invalid csv file");
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append((char)c2);
                                    }
                                    else
                                    {
                                        sb.Append((char)c2);
                                    }
                                }
                                c2 = c1;
                                break;
                        }
                        break;
                }
            }
            return col;
        }
    }
}
