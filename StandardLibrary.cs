using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interpreterv2
{
    public class StandardLibrary
    {
        public enum Visibility
        {
            Private,
            Public
        }

        public enum DataType
        {
            integer,
            str,
            doubl,
            flot,
            boolean,
            shrt,
            lon
        }

        public enum Namespace
        {
            std
        }

        public enum Library
        {
            iostream,
            str,
        }

        public enum MethodType
        {
            voidd,
            integer,
            str,
            doubl,
            flot,
            boolean,
            shrt,
            lon
        }

        public enum FileType
        {
            Header,
            Source
        }

        public static bool IsSupportedDataType(string type)
        {
            if (type == "int" || type == "string" || type == "double" || type == "float" || type == "bool" || type == "short" || type == "long")
            {
                return true;
            }
            return false;
        }

        public static bool IsSupportedMethodType(string type)
        {
            if (type == "void" || type == "int" || type == "string" || type == "double" || type == "float" || type == "bool" || type == "short" || type == "long")
            {
                return true;
            }
            return false;
        }
    }
}
