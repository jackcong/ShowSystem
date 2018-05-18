using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System.IO;

namespace ComLib.PDF
{
    public static class ReadPDF
    {
        private static string parseUsingPDFBox(string input)
        {
            PDDocument doc = PDDocument.load(input);
            PDFTextStripper stripper = new PDFTextStripper();
            return stripper.getText(doc);
        }
        private static string parseUsingPDFBox(FileStream input)
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            var byts = ms.ToArray();
            ms.Dispose();
            java.io.InputStream ins = new java.io.ByteArrayInputStream(byts);
            PDDocument doc = PDDocument.load(ins);
            PDFTextStripper stripper = new PDFTextStripper();
            return stripper.getText(doc);
        }
    }
}
