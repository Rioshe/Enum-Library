using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TC.EnumLibrary {
    public static class GenerateEnums {
        public static void GenEnum(string path, string enumName, List<string> enumNames) {
            List<string> filteredEnumNames = enumNames
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => char.IsDigit(value[0]) ? value[1..] : value)
                .Select(value => value.ConvertToAlphanumeric())
                .ToList();

            if (filteredEnumNames.Count == 0) {
                SystemLogging.LogWarning("Enum names list is empty or contains only invalid entries.");
                return;
            }

            string directoryPath = Path.GetDirectoryName(path);
            if (!LibraryHelpers.GenerateFolderStructureAt(directoryPath)) {
                SystemLogging.LogWarning("Failed to generate folder structure at: " + directoryPath);
                return;
            }

            if (directoryPath == null) return;
            string filePath = Path.Combine(directoryPath, enumName + ".cs");

            using (var writer = new StreamWriter(filePath)) {
                writer.WriteLine("public enum " + enumName);
                writer.WriteLine("{");

                for (var i = 0; i < filteredEnumNames.Count; i++) {
                    writer.WriteLine("    " + filteredEnumNames[i] + (i < filteredEnumNames.Count - 1 ? "," : ""));
                }
            }

            // Check for duplicates and remove one of them
            List<string> enumValues = File.ReadAllLines(filePath)
                .Where(line => line.Contains(",") || line.Contains("}"))
                .Select(line => line.Trim().TrimEnd(','))
                .ToList();

            HashSet<string> uniqueEnumValues = new();
            List<string> duplicates = enumValues.Where(value => !uniqueEnumValues.Add(value)).ToList();

            if (duplicates.Count > 0) {
                enumValues = enumValues.Distinct().ToList();

                using var writer = new StreamWriter(filePath);
                writer.WriteLine("public enum " + enumName);
                writer.WriteLine("{");

                for (var i = 0; i < enumValues.Count; i++) {
                    writer.WriteLine("    " + enumValues[i] + (i < enumValues.Count - 1 ? "," : ""));
                }
            }

            using (var writer = new StreamWriter(filePath, true)) {
                writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
            SystemLogging.Log("Enum generated at: " + filePath);
        }
    }
}