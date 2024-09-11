using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace TC.EnumLibrary {
    public class EnumGenerator : OdinEditorWindow {
        [MenuItem("Tools/Enum Generator")]
        static void OpenWindow() => GetWindow<EnumGenerator>().Show();

        [Title("Enum Generator")]
        [InfoBox("Enter the names for the enum values and click 'Generate Enum'")]
        public List<string> m_enumNames = new();
        [ShowInInspector] public Dictionary<Enum, float> Prices;
        public Enum ExistingEnum;
        const float DEFAULT_PRICE = 10.00f;

        [Button("Populate Prices")]
        void PopulatePrices() {
            if (ExistingEnum == null) {
                Debug.LogWarning("An existing enum must be selected to populate the prices.");
                return;
            }

            Prices = new Dictionary<Enum, float>();
            foreach (Enum value in Enum.GetValues(ExistingEnum.GetType())) {
                Prices[value] = DEFAULT_PRICE;
            }

            Debug.Log("Prices populated for the selected enum.");
        }

        [Button("Generate Enum")]
        void GenerateEnum() {
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

                writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
            Debug.Log("Enum generated at: " + filePath);
        }

        [Button("Generate ScriptableObject")]
void GenerateScriptableObject() {
    if (ExistingEnum == null) {
        Debug.LogWarning("An existing enum must be selected to generate a ScriptableObject.");
        return;
    }

    List<string> filteredEnumNames = Enum.GetNames(ExistingEnum.GetType()).ToList();
    string enumTypeName = ExistingEnum.GetType().Name;

    if (filteredEnumNames.Count == 0) {
        Debug.LogWarning("Enum names list is empty or contains only invalid entries.");
        return;
    }

    const string className = "DrugTypePrices";
    string filePath = Path.Combine(Application.dataPath, className + ".cs");

    using (var writer = new StreamWriter(filePath)) {
        writer.WriteLine("using System.Collections.Generic;");
        writer.WriteLine("using UnityEngine;");
        writer.WriteLine();
        writer.WriteLine("namespace TC.EnumLibrary {");
        writer.WriteLine("    public class " + className + " : ScriptableObject {");
        writer.WriteLine("        public float m_defaultPrice = 10.00f;");
        writer.WriteLine("        Dictionary<" + enumTypeName + ", float> m_prices;");
        writer.WriteLine();
        writer.WriteLine("        public void Init() {");
        writer.WriteLine("            m_prices = new Dictionary<" + enumTypeName + ", float> {");

        for (var i = 0; i < filteredEnumNames.Count; i++) {
            string enumValue = filteredEnumNames[i];
            float price = Prices.ContainsKey((Enum)Enum.Parse(ExistingEnum.GetType(), enumValue)) ? Prices[(Enum)Enum.Parse(ExistingEnum.GetType(), enumValue)] : DEFAULT_PRICE;
            writer.WriteLine("                { " + enumTypeName + "." + enumValue + ", " + price + " }" + (i < filteredEnumNames.Count - 1 ? "," : ""));
        }

        writer.WriteLine("            };");
        writer.WriteLine("        }");
        writer.WriteLine();
        writer.WriteLine("        public float GetPrice(" + enumTypeName + " drugType) => m_prices[drugType];");
        writer.WriteLine("    }");
        writer.WriteLine("}");
    }

    AssetDatabase.Refresh();
    Debug.Log("ScriptableObject generated at: " + filePath);
}
    }
}