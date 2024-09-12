using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TC.EnumLibrary {
    public static class ClassParser {
        public static void GenScriptableObject(Type valueType, Enum existingEnum, string className, bool isNumeric = false) {
            if (existingEnum == null) {
                SystemLogging.LogWarning("An existing enum must be selected to generate a ScriptableObject.");
                return;
            }

            List<string> filteredEnumNames = Enum.GetNames(existingEnum.GetType()).ToList();
            string enumTypeName = existingEnum.GetType().Name;

            if (filteredEnumNames.Count == 0) {
                SystemLogging.LogWarning("Enum names list is empty or contains only invalid entries.");
                return;
            }

            string filePath = Path.Combine(Application.dataPath, className + ".cs");
            string valueTypeName = isNumeric ? GetNumericTypeName(valueType) : valueType.Name;

            using (var writer = new StreamWriter(filePath)) {
                writer.WriteLine("using System;");
                writer.WriteLine("using UnityEngine;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using Sirenix.OdinInspector;");
                writer.WriteLine();
                writer.WriteLine("namespace TC.EnumLibrary {");
                writer.WriteLine("    [CreateAssetMenu(fileName = \"" + className + "\", menuName = \"AssetLibrary/" + className + "\")]");
                writer.WriteLine("    public class " + className + " : ScriptableObject {");
                if (!isNumeric) {
                    writer.WriteLine("        [SerializeField] " + valueTypeName + " m_default;");
                }
                writer.WriteLine("        [ShowInInspector] Dictionary<" + enumTypeName + ", " + valueTypeName + "> m_items;");
                writer.WriteLine();
                writer.WriteLine("        public void Awake() => Init();");
                writer.WriteLine("        void Init() {");
                writer.WriteLine("            m_items = new Dictionary<" + enumTypeName + ", " + valueTypeName + ">();");
                writer.WriteLine("            foreach (" + enumTypeName + " value in Enum.GetValues(typeof(" + enumTypeName + "))) {");
                writer.WriteLine("                m_items.Add(value, " + (isNumeric ? "0" : "m_default") + ");");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        [Button(\"Reset\")]");
                writer.WriteLine("        public void Reset() => Init();");
                writer.WriteLine("        public " + valueTypeName + " GetItem(" + enumTypeName + " item) => m_items[item];");
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Debug.Log("ScriptableObject generated at: " + filePath);
        }

        static string GetDefaultValue(Type valueType, bool isNumeric) => isNumeric ? "0" : "default";

        static string GetNumericTypeName(Type type) {
            if (type == typeof(int)) return "int";
            if (type == typeof(float)) return "float";
            if (type == typeof(double)) return "double";
            if (type == typeof(decimal)) return "decimal";
            if (type == typeof(long)) return "long";
            if (type == typeof(short)) return "short";
            return type == typeof(byte) ? "byte" : "float"; // Fallback, though ideally this should never be reached
        }
    }
}