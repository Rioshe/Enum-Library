using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace TC.EnumLibrary
{
    public static class EnumGeneratorUtility
    {
        public static void GenerateEnumAndStaticClass<T>(string enumName, string folderPath, 
            IEnumerable<string> enumValues, IEnumerable<T> items, string resourcePath, string type)
        {
            GenerateEnum(enumName, folderPath, enumValues);
            GenerateStaticClass(enumName, folderPath, items, resourcePath, type);
        }

        public static bool ValidateInput<T>(string enumName, List<T> items)
        {
            if (string.IsNullOrEmpty(enumName))
            {
                Debug.LogWarning("Enum name is empty. Please provide a valid name.");
                return false;
            }

            if (items == null || items.Count == 0)
            {
                Debug.LogWarning("No items selected. Please add items.");
                return false;
            }

            return true;
        }

        public static void GenerateEnum(string enumName, string folderPath, IEnumerable<string> enumValues)
        {
            string enumFolderPath = Path.Combine(folderPath, "Enums");
            string enumFilePath = Path.Combine(enumFolderPath, $"{enumName}.cs");

            var enumCode = new StringBuilder();
            enumCode.AppendLine("// Auto-generated enum from EnumEditor");
            enumCode.AppendLine($"namespace TC.EnumLibrary {{");
            enumCode.AppendLine($"    public enum {enumName}");
            enumCode.AppendLine("    {");
            enumCode.AppendLine("        None, // Default option when nothing is selected");

            foreach (string value in enumValues)
            {
                string sanitizedValue = value.Replace(" ", "_");
                enumCode.AppendLine($"        {sanitizedValue},");
            }

            enumCode.AppendLine("    }");
            enumCode.AppendLine("}");

            Directory.CreateDirectory(enumFolderPath);
            File.WriteAllText(enumFilePath, enumCode.ToString());
            AssetDatabase.Refresh();
        }

        public static void GenerateStaticClass<T>(string enumName, string folderPath, 
            IEnumerable<T> items, string resourcePath, string type)
        {
            string classFolderPath = Path.Combine(folderPath, "Librarys");
            string classFilePath = Path.Combine(classFolderPath, $"{enumName}Library.cs");

            var staticClassCode = new StringBuilder();
            staticClassCode.AppendLine("// Auto-generated static class for dictionary");
            staticClassCode.AppendLine("using System.Collections.Generic;");
            staticClassCode.AppendLine("using UnityEngine;");
            staticClassCode.AppendLine($"namespace TC.EnumLibrary {{");
            staticClassCode.AppendLine($"    public static class {enumName}Library");
            staticClassCode.AppendLine("    {");
            staticClassCode.AppendLine($"        public static readonly Dictionary<{enumName}, {type}> Items = new() {{");

            foreach (var item in items)
            {
                string sanitizedValue = item.ToString().Replace(" ", "_");
                staticClassCode.AppendLine($"            {{ {enumName}.{sanitizedValue}, Resources.Load<{type}>(\"{resourcePath}/{item}\") }},");
            }

            staticClassCode.AppendLine("        };");

            // Add Get method
            staticClassCode.AppendLine($"        public static {type} GetItem({enumName} enumValue) {{");
            staticClassCode.AppendLine("            if (Items.TryGetValue(enumValue, out var item)) {");
            staticClassCode.AppendLine("                return item;");
            staticClassCode.AppendLine("            }");
            staticClassCode.AppendLine($"            Debug.LogWarning($\"{type} for {{enumValue}} not found.\");");
            staticClassCode.AppendLine("            return null;");
            staticClassCode.AppendLine("        }");

            staticClassCode.AppendLine("    }");
            staticClassCode.AppendLine("}");

            Directory.CreateDirectory(classFolderPath);
            File.WriteAllText(classFilePath, staticClassCode.ToString());
            AssetDatabase.Refresh();
        }
    }
}