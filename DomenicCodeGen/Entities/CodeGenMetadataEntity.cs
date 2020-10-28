using System;
using System.Collections.Generic;
using System.Text;

namespace DM.DomenicCodeGen.Entities
{
    public class CodeGenMetadataEntity
    {
        /// <summary>
        /// 模板的名字（文件名）
        /// </summary>
        public string TemplateFileName { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        public string FileContent { get; set; }

        /// <summary>
        /// 输出的文件内容
        /// </summary>
        public string OutFileContent { get; set; }

        /// <summary>
        /// 变量,key为变量名字，value为：要把该变量替换为什么内容。
        /// </summary>
        public Dictionary<string,string> Variables { get; set; }

        /// <summary>
        /// 输出的全路径
        /// </summary>
        public string OutputPath { get; set; }
    }
}
