using System;
using System.Collections.Generic;
using System.Text;

namespace DM.DomenicCodeGen
{
    public class CodeGenInput
    {
        public string OutBasePath { get; set; }
        public string Entity { get; set; }
        public string Namespace { get; set; }
        public string RepositoryName { get; set; }
        public string Properties { get; set; }
    }
}
