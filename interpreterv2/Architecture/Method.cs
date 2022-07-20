using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interpreterv2.Architecture
{
    public class Method
    {
        public StandardLibrary.Visibility Visibility; //public or private
        public string Name;
        public StandardLibrary.MethodType Type; // Void or etc.
        public List<Variable> Parameter;
        public List<Variable> Variables;
        public string SourceCode;
    }
}
