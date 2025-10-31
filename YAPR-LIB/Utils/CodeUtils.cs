using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using UndertaleModLib;
using UndertaleModLib.Decompiler;
using UndertaleModLib.Models;

namespace YAPR_LIB.Utils
{
    public static class CodeUtils
    {
        public static void AddGlobalVariables(UndertaleData gmData, GlobalDecompileContext decompileContext, Dictionary<string, object> variablesToAdd)
        {
            var variableStrings = variablesToAdd.Keys.Select(v => new UndertaleString(v)).ToArray();
            var varID = gmData.Variables.Count;
            var title_screen_other_4_code = gmData.Code.ByName("gml_Object_obj_Title_Screen_Other_4");
            var title_screen_other_4 = Decompiler.Decompile(title_screen_other_4_code, decompileContext);
            var default_value = string.Empty;

            foreach (var variableString in variableStrings)
            {
                gmData.Strings.Add(variableString);
                gmData.Variables.Add(new UndertaleVariable()
                {
                    NameStringID = gmData.Strings.IndexOf(variableString),
                    Name = variableString,
                    InstanceType = UndertaleInstruction.InstanceType.Global,
                    VarID = varID++
                });

                if (variablesToAdd[variableString.Content] is bool)
                    title_screen_other_4 = title_screen_other_4.Replace(
                        "    global.MOBILE = 0",
                      $$"""
                            global.{{variableString.Content}} = {{((bool)variablesToAdd[variableString.Content] ? 1 : 0)}}
                            global.MOBILE = 0
                        """
                    );
                else if (variablesToAdd[variableString.Content] is string)
                    title_screen_other_4 = title_screen_other_4.Replace(
                        "    global.MOBILE = 0",
                      $$"""
                            global.{{variableString.Content}} = "{{variablesToAdd[variableString.Content]}}"
                            global.MOBILE = 0
                        """
                    );
                else
                    title_screen_other_4 = title_screen_other_4.Replace(
                        "    global.MOBILE = 0",
                      $$"""
                            global.{{variableString.Content}} = {{variablesToAdd[variableString.Content]}}
                            global.MOBILE = 0
                        """
                    );
            }

            title_screen_other_4_code.ReplaceGML(title_screen_other_4, gmData);
        }

        public static void CreateFunction(UndertaleData gmData, string name, string code)
        {
            var func_code_str = new UndertaleString($"gml_Script_{name}");
            gmData.Strings.Add(func_code_str);

            var func_code = new UndertaleCode();
            func_code.Name = func_code_str;
            func_code.AppendGML(code, gmData);

            var code_locals = new UndertaleCodeLocals()
            {
                Name = func_code_str
            };

            func_code.FindReferencedVars().ToList().ForEach(var =>
            {
                var isArgument = Regex.Match(var.Name.Content, "^argument[0-9]+$").Success;
                if (!gmData.Variables.Any(v => v.Name.Content == var.Name.Content) && !isArgument)
                    gmData.Variables.Add(var);
                if (!gmData.Strings.Any(str => str.Content == var.Name.Content))
                    gmData.Strings.Add(var.Name);
                if (var.InstanceType == UndertaleInstruction.InstanceType.Local)
                {
                    code_locals.Locals.Add(new UndertaleCodeLocals.LocalVar()
                    {
                        Index = (uint)var.VarID,
                        Name = var.Name
                    });
                }
                if (isArgument)
                {
                    code_locals.Locals.Add(new UndertaleCodeLocals.LocalVar()
                    {
                        Index = 0,
                        Name = gmData.Strings.First(str => str.Content == "arguments")
                    });
                }
            });

            gmData.CodeLocals.Add(code_locals);
            func_code.ArgumentsCount = (ushort)func_code.FindReferencedVars().DistinctBy(var => var.Name.Content).Count(var => Regex.Match(var.Name.Content, "^argument[0-9]+$").Success);
            func_code.LocalsCount = (uint)(code_locals.Locals.Count - 1);
            func_code.UpdateAddresses();
            gmData.Code.Add(func_code);

            var func_script_str = new UndertaleString(name);
            gmData.Strings.Add(func_script_str);

            var func_script = new UndertaleScript();
            func_script.Name = func_script_str;
            func_script.Code = func_code;
            gmData.Scripts.Add(func_script);

            var func_func = new UndertaleFunction();
            func_func.Name = func_script_str;
            func_func.NameStringID = gmData.Strings.IndexOf(func_script_str);
        }
    }
}
