using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
namespace TC.EnumLibrary {
    [Serializable]
    public class GenerateEnums {
        [InfoBox("Enter the names for the enum values and click 'Generate Enum'")]
        public List<string> m_enumNames = new();
        public void GenerateEnum() {
            // Filter out null, empty, or whitespace strings and convert to alphanumeric
            List<string> filteredEnumNames = m_enumNames
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value.ConvertToAlphanumeric())
                .ToList();

            if (filteredEnumNames.Count == 0) {
                Debug.LogWarning("Enum names list is empty or contains only invalid entries.");
                return;
            }

            const string enumName = "GeneratedEnum";
            string filePath = Path.Combine(Application.dataPath, "GeneratedEnum.cs");

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
            Debug.Log("Enum generated at: " + filePath);
        }
    }
}