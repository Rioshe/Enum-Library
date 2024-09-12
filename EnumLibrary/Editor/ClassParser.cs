using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace TC.EnumLibrary {
    public static class ClassParser {
        public static void GenScriptableObject(Type valueType, Enum targetEnum, string className, string namespaceName, string filePath, bool isNumeric = false) {
            if (!ValidateEnum(targetEnum)) return;

            List<string> enumNames = Enum.GetNames(targetEnum.GetType()).ToList();
            if (!ValidateEnumNames(enumNames)) return;

            string valueTypeName = GetValueTypeName(valueType, isNumeric);
            string enumTypeName = targetEnum.GetType().Name;

            string directoryPath = Path.GetDirectoryName(filePath);
            if (!LibraryHelpers.ValidateDirectoryPath(directoryPath)) return;

            if (!LibraryHelpers.GenerateFolderStructureAt(directoryPath)) {
                SystemLogging.LogWarning($"Failed to generate folder structure at: {directoryPath}");
                return;
            }

            if (directoryPath == null) return;
            string scriptFilePath = Path.Combine(directoryPath, $"{className}.cs");

            GenerateScriptFile(scriptFilePath, className, namespaceName, valueTypeName, enumTypeName, isNumeric);

            RefreshAssets(filePath);
        }

        static bool ValidateEnum(Enum targetEnum) {
            if (targetEnum != null) return true;
            SystemLogging.LogWarning("An existing enum must be provided to generate a ScriptableObject.");
            return false;
        }

        static bool ValidateEnumNames(List<string> enumNames) {
            if (enumNames.Any()) return true;
            SystemLogging.LogWarning("Enum names list is empty or contains invalid entries.");
            return false;
        }

        static string GetValueTypeName(Type valueType, bool isNumeric) {
            return isNumeric ? GetNumericTypeName(valueType) : valueType.Name;
        }

        static void GenerateScriptFile(string scriptFilePath, string className, string namespaceName, string valueTypeName, string enumTypeName, bool isNumeric) {
            using var writer = new StreamWriter(scriptFilePath);
            writer.WriteLine("using System;");
            writer.WriteLine("using UnityEngine;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using Sirenix.OdinInspector;");
            writer.WriteLine("using UnityEngine.Video;");
            writer.WriteLine();
            writer.WriteLine($"namespace {namespaceName} {{");
            writer.WriteLine($"    [CreateAssetMenu(fileName = \"{className}\", menuName = \"AssetLibrary/{className}\")]");
            writer.WriteLine($"    public class {className} : ScriptableObject {{");

            if (!isNumeric) {
                writer.WriteLine($"        [SerializeField] private {valueTypeName} defaultValue;");
            }

            writer.WriteLine($"        [ShowInInspector] private Dictionary<{enumTypeName}, {valueTypeName}> items = new();");
            writer.WriteLine();
            writer.WriteLine("        private void Awake() => Initialize();");
            writer.WriteLine();
            writer.WriteLine("        private void Initialize()");
            writer.WriteLine("        {");
            writer.WriteLine($"            items = new Dictionary<{enumTypeName}, {valueTypeName}>();");
            writer.WriteLine($"            foreach ({enumTypeName} enumValue in Enum.GetValues(typeof({enumTypeName})))");
            writer.WriteLine("            {");
            writer.WriteLine($"                items.Add(enumValue, {(isNumeric ? "0" : "defaultValue")});");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine();
            writer.WriteLine("        [Button(\"Reset\")]");
            writer.WriteLine("        public void Reset() => Initialize();");
            writer.WriteLine();
            writer.WriteLine($"        public {valueTypeName} GetItem({enumTypeName} enumValue) => items[enumValue];");
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        static void RefreshAssets(string filePath) {
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            SystemLogging.Log($"ScriptableObject generated at: {filePath}");
        }

        static string GetNumericTypeName(Type type) {
            return type switch {
                not null when type == typeof(int) => "int",
                not null when type == typeof(float) => "float",
                not null when type == typeof(double) => "double",
                not null when type == typeof(decimal) => "decimal",
                not null when type == typeof(long) => "long",
                not null when type == typeof(short) => "short",
                not null when type == typeof(byte) => "byte",
                _ => "float", // Default fallback
            };
        }
    }
}
