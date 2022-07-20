using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using interpreterv2.Architecture;
namespace interpreterv2.Architecture
{
    public class AppFile
    {
        public bool MainFile;
        public StandardLibrary.FileType Type; //Header or Source
        public string Name;
        public string OriginalCode;
        public List<Include> Includes = new List<Include>();
        public List<String> Usings;
        public List<AppClass> AppClasses;
    }
}
