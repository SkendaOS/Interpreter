using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using interpreterv2.Architecture;
namespace interpreterv2
{
    internal class Program
    {
        static List<AppFile> exeFile = new List<AppFile>();
        static Runnable.STATE exeState = Runnable.STATE.SYSTEM_PARSING;
        static string errorMessage;

        static void Main(string[] args)
        {
            ExecutableParser parser = new ExecutableParser("executable.ske");
            Executable executable = parser.Deserialize<Executable>(parser.Read());
            string[] files = GetFilesInsideExecutable(executable);
            foreach (string content in files)
            {
                try
                {
                    AppFile appFile = new AppFile();
                    string[] file = content.Split('@');
                    string file_name = file[0];
                    string file_content = file[1];
                    appFile.Name = file_name;
                    if (parser.DecodeBase64(file_content) != "SKENDAOS_PARSING_ERROR")
                    {
                        appFile.OriginalCode = parser.DecodeBase64(file_content);
                    }
                    if (file_name.EndsWith("h") || file_name.EndsWith("hpp"))
                    {
                        appFile.Type = StandardLibrary.FileType.Header;
                    } else if (file_name.EndsWith("cpp"))
                    {
                        appFile.Type = StandardLibrary.FileType.Source;
                    }
                    ParseCode(appFile);
                }
                catch (Exception ex)
                {
                    errorMessage = "Application exited with error. Caused by: Build failed, unidentified code; " + ex.Message;
                    exeState = Runnable.STATE.STOP_WITH_ERROR;
                }
            }
            if (exeState != Runnable.STATE.STOP_WITH_ERROR)
            {
                exeState = Runnable.STATE.SYSTEM_BUILD;
             
            }


            Console.ReadLine();
            
        }

        public static bool ParseCode(AppFile file)
        {
            string[] code = file.OriginalCode.Split('\n');
            string code_without_header = "";
            foreach (string line in code)
            {
                if (line.Trim().StartsWith("#include"))
                {
                    bool canInsert = true;
                    bool isUserDefined = false;
                    string include_content = line.Trim().Replace("#include", "");
                    include_content = include_content.Trim();
                    if (include_content.StartsWith("\""))
                    {
                        isUserDefined = true;
                        include_content = include_content.Replace("\"", "");
                    }
                    include_content.Replace("<", "");
                    include_content.Replace(">", "");
                    include_content = include_content.Trim();
                    foreach (Include include in file.Includes)
                    {
                        if (include.Name == include_content)
                        {
                            if (include.UserDefined == isUserDefined)
                            {
                                canInsert = false;
                            }
                        }
                    }
                    if (!isUserDefined)
                    {
                        if (include_content == "string" || include_content == "iostream")
                        {

                        }
                        else
                        {
                            canInsert = false;
                            exeState = Runnable.STATE.STOP_WITH_ERROR;
                            errorMessage = "Application exited with error. Caused by: Unsupported runtime library. System cannot find specified method or SDK.";
                        }
                    }
                    if (canInsert)
                    {
                        Include include = new Include();
                        include.Name = include_content;
                        include.UserDefined = isUserDefined;
                        file.Includes.Add(include);
                    }
                } else if (line.Trim().StartsWith("using"))
                {
                    string using_content = line.Trim().Replace("using", "");
                    using_content.Replace(";", "");
                    using_content = using_content.Trim();
                    if (using_content != "namespace std")
                    {
                        exeState = Runnable.STATE.STOP_WITH_ERROR;
                        errorMessage = "Application exited with error. Caused by: Unsupported runtime library. System cannot find specified method or SDK.";
                    } else
                    {
                        file.Usings.Add(using_content);
                    }  
                } else 
                {
                    code_without_header += line.Trim();
                }
            }



            string currentClass = "main";
            string currentMethod = "";
            string currentVisibility = "public";
            string[] semicolon = code_without_header.Split(';');
            foreach (string line in semicolon)
            {
                string line_fixed = line;
                if (line.Contains("{"))
                {
                    string[] brackets = line.Split('{');
                    currentClass = brackets[0].Trim();
                    line_fixed = brackets[1].Trim();
                }

                if (line.Contains("}"))
                {
                    string[] brackets = line.Split('}');
                    currentClass = "main";
                    line_fixed = brackets[1].Trim();
                }
            }
            if (exeState == Runnable.STATE.STOP_WITH_ERROR)
            {
                return false;
            } else
            {
                return true;
            }
        }
        public static string[] GetFilesInsideExecutable(Executable executable)
        {
            try
            {
                string[] result = executable.Content.Split('#');
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
