using System;
using System.Collections.Generic;
using System.Text;

namespace DM.DomenicCodeGen
{
    /// <summary>
    /// 生成代码的input
    /// </summary>
    public class CodeGenInput
    {
        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutBasePath { get; set; }
        
        /// <summary>
        /// 实体名称
        /// </summary>
        public string Entity { get; set; }
        
        /// <summary>
        /// 小写的实体名称
        /// </summary>
        public string LowerCaseEntity { get; set; }
        
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// 实体属性(Properties)代码正文
        /// </summary>
        public string Properties { get; set; }

        /// <summary>
        /// 模块名
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Area名
        /// </summary>
        public string AreaName { get; set; }
}
}
