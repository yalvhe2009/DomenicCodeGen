using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DM.DomenicCodeGen.helper
{
    public static class FileReaderAndWriter
    {
        public static string ReadFile(string filePath)
        {
            string all = "";
            try
            {
                // 创建一个 StreamReader 的实例来读取文件 
                // using 语句也能关闭 StreamReader
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;                    
                    // 从文件读取并显示行，直到文件的末尾 
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("//"))
                        {
                            //Console.WriteLine($"该行注释已被程序去除,不会读取！行：{line}");
                            continue;
                        }
                        all += line + "\r\n";
                    }
                }
            }
            catch (Exception e)
            {
                // 向用户显示出错消息
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return all;
        }

        public static void WriteFile(string content, string filePath)
        {
            string basePath = filePath.Replace(Path.GetFileName(filePath), "");
            if (false == System.IO.Directory.Exists(basePath))
            {
                //创建pic文件夹
                System.IO.Directory.CreateDirectory(basePath);
            }
            UTF8Encoding utf8BOM = new UTF8Encoding(true);
            using (StreamWriter sw = new StreamWriter(filePath, false, utf8BOM))
            {
                sw.Write(content);
            }
        }

        public static List<string> GetFileList(string path)
        {
            List<string> fileList = new List<string>();

            if (Directory.Exists(path) == true)
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    fileList.Add(file);
                }
                //暂时不需要读取子目录
                //foreach (string directory in Directory.GetDirectories(path))
                //{
                //    fileList.AddRange(GetFileList(directory));
                //}
            }

            return fileList;
        }
    }
}
