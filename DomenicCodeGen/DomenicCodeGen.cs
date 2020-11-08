using DM.DomenicCodeGen.Entities;
using DM.DomenicCodeGen.helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DM.DomenicCodeGen
{
    public class DomenicCodeGen
    {
        
        private readonly string _basePath = "";
        private readonly string _templatesPath = "";
        private readonly List<CodeGenMetadataEntity> _codeGenMetadataEntities;
        public DomenicCodeGen()
        {
            _basePath = System.AppDomain.CurrentDomain.BaseDirectory; //E:\Code\netcore\DomenicCodeGen\Main\bin\Debug\netcoreapp3.1\
            string assemblyName = typeof(DomenicCodeGen).Assembly.GetName().Name;//DomenicCodeGen
            int idx = _basePath.IndexOf("DomenicCodeGen");
            _basePath = _basePath.Substring(0, idx + 14);
            _basePath = Path.Combine(_basePath, assemblyName);
            _templatesPath = Path.Combine(_basePath, "Templates");//上述所有操作就是为了获取到存放模板的目录
            _codeGenMetadataEntities = GetCodeGenMetadataEntities();
        }

        public void Run(CodeGenInput input)
        {
            foreach (var codeGenMetadataEntity in _codeGenMetadataEntities)
            {
                string codeContent = codeGenMetadataEntity.FileContent;                    
                codeContent = codeContent
                    .Replace("${namespace}", input.Namespace)
                    .Replace("${entity}", input.Entity)
                    .Replace("${lowerCaseEntity}", input.LowerCaseEntity)
                    .Replace("${properties}", input.Properties)
                    .Replace("${areaName}", input.AreaName)
                    .Replace("${moduleName}", input.ModuleName)
                    ;

                string outFileName = codeGenMetadataEntity.TemplateFileName.Replace("XX", input.Entity).Replace(".template", "");
                codeGenMetadataEntity.OutputPath = Path.Combine(input.OutBasePath, outFileName);
                codeGenMetadataEntity.OutFileContent = codeContent;//赋值输出内容。
            }
            //生成文件
            foreach (var item in _codeGenMetadataEntities)
            {
                FileReaderAndWriter.WriteFile(item.OutFileContent, item.OutputPath);
            }
        }

        /// <summary>
        /// 获取CodeGen需要的元数据的 实体列表
        /// </summary>
        private List<CodeGenMetadataEntity> GetCodeGenMetadataEntities()
        {            
            List<CodeGenMetadataEntity> codeGenMetadataEntities = new List<CodeGenMetadataEntity>();
            List<string> templateFullFilePaths = FileReaderAndWriter.GetFileList(_templatesPath);

            foreach (var tempFullPath in templateFullFilePaths)
            {
                //获取文件名
                string tempFileName = Path.GetFileName(tempFullPath);
                Dictionary<string, string> variables = GetTemplateVariables(tempFullPath);
                CodeGenMetadataEntity codeGenMetadataEntity = new CodeGenMetadataEntity {
                    TemplateFileName = tempFileName, 
                    Variables = variables,
                    FileContent = FileReaderAndWriter.ReadFile(tempFullPath)
            };
                codeGenMetadataEntities.Add(codeGenMetadataEntity);
            }

            return codeGenMetadataEntities;
        }

        /// <summary>
        /// 获取某个模板中所有的 变量
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTemplateVariables(string filePath)
        {
            Dictionary<string, string> vars = new Dictionary<string, string>();
            string fileContent = FileReaderAndWriter.ReadFile(filePath);
            //解析代码中的变量，规则为找出所有类似于： {xx} 的字符串。
            Regex reg = new Regex(@"(?is)(?<=\$\{)[^\}]+(?=\})");
            MatchCollection mc = reg.Matches(fileContent);
            HashSet<string> noRepeatVars = new HashSet<string>();
            foreach (var item in mc)
            {
                noRepeatVars.Add(item.ToString());
            }
            foreach (var item in noRepeatVars)
            {
                vars.Add(item, "");
            }
            return vars;
        }

        /// <summary>
        /// 获取程序运行必须传入的变量
        /// </summary>
        private List<string> GetProgramRunNeedVars(List<CodeGenMetadataEntity> codeGenMetadataEntities)
        {
            //List<string> lists = new List<string>();
            HashSet<string> noRepeatVars = new HashSet<string>();
            foreach (CodeGenMetadataEntity entity in codeGenMetadataEntities)
            {
                foreach (var item in entity.Variables)
                {
                    noRepeatVars.Add(item.Key);
                }
            }

            return new List<string>(noRepeatVars);
        }

    }
}
