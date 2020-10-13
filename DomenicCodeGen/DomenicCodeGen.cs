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
            _basePath = Path.Combine(_basePath.Split("Main")[0], assemblyName);//TODO去掉Main
            _templatesPath = Path.Combine(_basePath, "Templates");
            _codeGenMetadataEntities = GetCodeGenMetadataEntities();
        }

        public void Run(CodeGenInput input)
        {
            foreach (var codeGenMetadataEntity in _codeGenMetadataEntities)
            {
                string codeContent = codeGenMetadataEntity.FileContent;
                if (codeGenMetadataEntity.TemplateFileName.Contains("Manager"))
                {
                    
                    codeContent = codeContent
                        .Replace("${namespace}", input.Namespace)
                        .Replace("${entity}", input.Entity)
                        .Replace("${repositoryName}", input.RepositoryName);

                    codeGenMetadataEntity.OutputPath = Path.Combine(input.OutBasePath, $"{input.Entity}Manager.cs");
                }
                else if(codeGenMetadataEntity.TemplateFileName.Contains("Dto") || codeGenMetadataEntity.TemplateFileName.Contains("Input"))
                {
                    //Dtos
                    codeContent = codeContent
                        .Replace("${namespace}", $"{input.Namespace}.Dto")
                        .Replace("${entity}", input.Entity)
                        .Replace("${properties}", input.Properties);
                    string templateFileName = codeGenMetadataEntity.TemplateFileName;
                    templateFileName = templateFileName.Replace(".cs.template", "").Replace("XX", input.Entity);
                    templateFileName += ".cs";
                    codeGenMetadataEntity.OutputPath = Path.Combine(input.OutBasePath, "Dto");
                    codeGenMetadataEntity.OutputPath = Path.Combine(codeGenMetadataEntity.OutputPath, templateFileName);
                }
                else if (codeGenMetadataEntity.TemplateFileName.Contains("AppService"))
                {
                    codeContent = codeContent
                        .Replace("${namespace}", input.Namespace)
                        .Replace("${entity}", input.Entity);
                    string templateFileName = codeGenMetadataEntity.TemplateFileName;
                    templateFileName = templateFileName.Replace(".cs.template", "").Replace("XX", input.Entity);
                    templateFileName += ".cs";
                    codeGenMetadataEntity.OutputPath = Path.Combine(input.OutBasePath, templateFileName);
                }
                else if (codeGenMetadataEntity.TemplateFileName.Contains("Model"))
                {
                    codeContent = codeContent
                        .Replace("${namespace}", input.Namespace)
                        .Replace("${entity}", input.Entity);
                    string templateFileName = codeGenMetadataEntity.TemplateFileName;
                    templateFileName = templateFileName.Replace(".cs.template", "").Replace("XX", input.Entity);
                    templateFileName += ".cs";
                    codeGenMetadataEntity.OutputPath = Path.Combine(input.OutBasePath, templateFileName);
                }
                else if (codeGenMetadataEntity.TemplateFileName.Contains("Controller"))
                {
                    codeContent = codeContent
                        .Replace("${namespace}", input.Namespace)
                        .Replace("${entity}", input.Entity);
                    string templateFileName = codeGenMetadataEntity.TemplateFileName;
                    templateFileName = templateFileName.Replace(".cs.template", "").Replace("XX", input.Entity);
                    templateFileName += ".cs";
                    codeGenMetadataEntity.OutputPath = Path.Combine(input.OutBasePath, templateFileName);
                }
                codeGenMetadataEntity.OutFileContent = codeContent;
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
