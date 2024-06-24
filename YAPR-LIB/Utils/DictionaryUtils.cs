using System.ComponentModel;

namespace YAPR_LIB.Utils
{
    public static class DictionaryUtils
    {
        public static string ToGMLDictionary(this Dictionary<dynamic, dynamic> __instance, string varName, bool isGlobal=false, int depth=1)
        {
            var nextDepthVarName = (depth > 1 ? varName.Substring(0, varName.Length - 1) : varName).ToLower();
            nextDepthVarName += $"{(depth + 1)}";
            var globalPrefix = isGlobal ? "global." : string.Empty;
            var codeToInsert = new List<string>()
            {
                isGlobal ? $"global.{varName} = ds_map_create()" : $"var {varName} = ds_map_create()"
            };
            foreach ((dynamic k, dynamic v) in __instance)
            {
                if (!(k is String || k is Int32))
                    throw new Exception($"Key is expected to be String or Int32 (Found type {k.GetType().FullName})");
                if (v.GetType().Name == "Dictionary`2")
                {
                    codeToInsert.AddRange(((object)v).ToDictionary().ToGMLDictionary(nextDepthVarName, false, depth + 1).Split("\n"));
                    codeToInsert.Add($"ds_map_add({globalPrefix}{varName}, \"{k}\", {nextDepthVarName})");
                }
                else if (v is float || v is double)
                    codeToInsert.Add($"ds_map_add({globalPrefix}{varName}, \"{k}\", {$"{v:0.000000}".Replace(",", ".")})");
                else if (v is string)
                    codeToInsert.Add($"ds_map_add({globalPrefix}{varName}, \"{k}\", \"{v}\")");
                else
                    codeToInsert.Add($"ds_map_add({globalPrefix}{varName}, \"{k}\", {v})");
            }
            return String.Join("\n", codeToInsert);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/1619518/a-dictionary-where-value-is-an-anonymous-type-in-c-sharp
        /// Turn anonymous object to dictionary
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Dictionary<dynamic, dynamic> ToDictionary(this object data)
        {
            var dict = new Dictionary<dynamic, dynamic>();
            var i = 0;
            foreach (var prop in TypeDescriptor.GetProperties(data)
                                              .OfType<PropertyDescriptor>())
            {
                if (prop.Name == "Keys")
                    foreach(var k in (dynamic)prop.GetValue(data))
                        dict.Add(k, null);
                if (prop.Name == "Values")
                    foreach (var v in (dynamic)prop.GetValue(data))
                        dict[dict.Keys.ToArray()[i++]] = v;
            }
            return dict;
        }
    }
}
