using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExportLogic
{
    public class FileHelper
    {        

        public static string ConvertToValidFileName(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName", "File name cannot be null.");
            }

            if (fileName.Length == 0)
            {
                throw new ArgumentException("File name cannot be empty.", "fileName");
            }

            fileName = RemoveInvalidFileNameChars(fileName);
            fileName = ChangeEmptyFileName(fileName);
            fileName = TrimToUrlLength(fileName);
            fileName = RemoveDoubleDots(fileName);
            return fileName;
        }

       

        private static string ChangeEmptyFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return
                "(no subject)" + Path.GetExtension(fileName);
            }
            if (fileName.StartsWith("."))
                fileName = fileName.Substring(1);

            return fileName;
        }

        private static string RemoveInvalidFileNameChars(string fileName)
        {
            int index = fileName.IndexOfAny(Path.GetInvalidFileNameChars());
            while (index != -1)
            {
                fileName = fileName.Remove(index, 1);
                index = fileName.IndexOfAny(Path.GetInvalidFileNameChars());
            }

            var finalChars = new List<char>();
            foreach (var c in fileName)
            {
                if (Char.IsControl(c)) continue;
                if (c == '~') continue;  
                if (c == ';') continue;
                finalChars.Add(c);
            }
            return new string(finalChars.ToArray());
        }

        private static string RemoveDoubleDots(string fileName)
        {
            int index = fileName.IndexOf("..");
            while (index != -1)
            {
                fileName = fileName.Remove(index, 1);
                index = fileName.IndexOf("..");
            }

            return fileName;
        }

        private static string TrimToUrlLength(string fileName)
        {
            //File or dir can have max 128 chars
            //Path for dir and files can have max 260 chars
            if (fileName.Length <= 128)
            {
                return fileName;
            }

            string fileExtension = Path.GetExtension(fileName);
            string fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);

            if (fileExtension.Length > 8)//no real extensions, just dot in name
                return fileName.Substring(0, 128);

            string newFileName = fileNameNoExtension.Substring(0, 128 - fileExtension.Length);
            newFileName += fileExtension;
            return newFileName;
        }

        

        
    }
}
