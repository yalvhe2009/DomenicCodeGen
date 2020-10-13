using DM.DomenicCodeGen;
using DM.DomenicCodeGen.helper;
using System;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var gen = new DomenicCodeGen();
            string properties = ReadProperties();
            CodeGenInput codeGenInput = new CodeGenInput {
                Entity = "Formula", 
                Namespace = "YX.Quality.Quality.Formulas", 
                OutBasePath = "C:\\Users\\yalvh\\Desktop\\output", 
                Properties = properties, 
                RepositoryName = "_formulaRepository"
            };
            gen.Run(codeGenInput);
            Console.WriteLine("生成完毕，按任意键结束！");
            Console.ReadKey();
        }

        static string ReadProperties()
        {
            string properties = FileReaderAndWriter.ReadFile("E:\\Code\\netcore\\DomenicCodeGen\\Main\\Inputs\\Proprities.txt");
            if (string.IsNullOrWhiteSpace(properties))
            {
                throw new Exception("读取属性文件Proprities.txt出错！");
            }
            return properties;
        }
    }

}
