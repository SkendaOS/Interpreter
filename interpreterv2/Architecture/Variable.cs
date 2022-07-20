using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interpreterv2.Architecture
{
    public class Variable
    {
        public StandardLibrary.Visibility Visibility;
        public string Name;
        public StandardLibrary.DataType Type;
        public string[] Value;
    }
}
