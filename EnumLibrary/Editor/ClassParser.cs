using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace TC.EnumLibrary {
    public static class ClassParser {
        public static void GenerateScriptableObjectForNumeric(Type valueType, Enum existingEnum, string className = "EnumLibrary") {
            if (existingEnum == null) {
                Debug.LogWarning("An existing enum must be selected to generate a ScriptableObject.");
                return;
            }

            List<string> filteredEnumNames = Enum.GetNames(existingEnum.GetType()).ToList();
            string enumTypeName = existingEnum.GetType().Name;

            if (filteredEnumNames.Count == 0) {
                Debug.LogWarning("Enum names list is empty or contains only invalid entries.");
                return;
            }
            
            string filePath = Path.Combine(Application.dataPath, className + ".cs");

            string numericTypeName = EnumGenerator.GetNumericTypeName(valueType);

            using (var writer = new StreamWriter(filePath)) {
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using UnityEngine;");
                writer.WriteLine();
                writer.WriteLine("namespace TC.EnumLibrary {");
                writer.WriteLine("    [CreateAssetMenu(fileName = \"" + className + "\", menuName = \"AssetLibrary/" + className + "\")]");
                writer.WriteLine("    public class " + className + " : ScriptableObject {");
                writer.WriteLine("        Dictionary<" + enumTypeName + ", " + numericTypeName + "> m_items;");
                writer.WriteLine();
                writer.WriteLine("        public void Init() {");
                writer.WriteLine("            m_items = new Dictionary<" + enumTypeName + ", " + numericTypeName + "> {");

                for (var i = 0; i < filteredEnumNames.Count; i++) {
                    string enumValue = filteredEnumNames[i];
                    object value = valueType == typeof(int) ? 0 : 0f;

                    writer.WriteLine("                { " + enumTypeName + "." + enumValue + ", " + value + " }" + (i < filteredEnumNames.Count - 1 ? "," : ""));
                }

                writer.WriteLine("            };");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        public " + numericTypeName + " GetItem(" + enumTypeName + " drugType) => m_items[drugType];");
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Debug.Log("ScriptableObject generated at: " + filePath);
        }
        public static void GenerateScriptableObjectForGeneric(Type valueType, Enum existingEnum, string className = "EnumLibrary") {
            if (existingEnum == null) {
                Debug.LogWarning("An existing enum must be selected to generate a ScriptableObject.");
                return;
            }

            List<string> filteredEnumNames = Enum.GetNames(existingEnum.GetType()).ToList();
            string enumTypeName = existingEnum.GetType().Name;

            if (filteredEnumNames.Count == 0) {
                Debug.LogWarning("Enum names list is empty or contains only invalid entries.");
                return;
            }
            
            string filePath = Path.Combine(Application.dataPath, className + ".cs");

            using (var writer = new StreamWriter(filePath)) {
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using UnityEngine;");
                writer.WriteLine();
                writer.WriteLine("namespace TC.EnumLibrary {");
                writer.WriteLine("    [CreateAssetMenu(fileName = \"" + className + "\", menuName = \"AssetLibrary/" + className + "\")]");
                writer.WriteLine("    public class " + className + " : ScriptableObject {");
                writer.WriteLine("        Dictionary<" + enumTypeName + ", " + valueType.Name + "> m_items;");
                writer.WriteLine();
                writer.WriteLine("        public void Init() {");
                writer.WriteLine("            m_items = new Dictionary<" + enumTypeName + ", " + valueType.Name + "> {");

                for (var i = 0; i < filteredEnumNames.Count; i++) {
                    string enumValue = filteredEnumNames[i];
                    writer.WriteLine("                { "
                                     + enumTypeName + "." + enumValue + ", " + "null" + " }"
                                     + (i < filteredEnumNames.Count - 1 ? "," : ""));
                }

                writer.WriteLine("            };");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        public " + valueType.Name + " GetItem(" + enumTypeName + " drugType) => m_prices[drugType];");

                writer.WriteLine("    }");
                writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Debug.Log("ScriptableObject generated at: " + filePath);
        }
    }
}