using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using static TC.EnumLibrary.ClassParser;
using static TC.EnumLibrary.GenerateEnums;
using static TC.EnumLibrary.LibraryHelpers;

namespace TC.EnumLibrary {
    public class EnumGenerator : OdinEditorWindow {
        [MenuItem("Tools/Enum Generator")]
        static void OpenWindow() => GetWindow<EnumGenerator>().Show();

        #region EnumClassGeneration
        [FolderPath] public string m_outputPath = "Assets/EnumLibrary/Generated";
        [Title("Enum Generator")]
        [FoldoutGroup("Generate Enum")]
        [InfoBox("Enter the names for the enum values and click 'Generate Enum'")]
        public List<string> m_enumNames = new();
        [FoldoutGroup("Generate Enum")]
        public string m_enumClassName = "EnumLibrary";
        [FoldoutGroup("Generate Enum")]
        [Button("Generate Enum")]
        public void GenerateEnum() {
            if (ClassExists(m_enumClassName)) {
                if (!EditorUtility.DisplayDialog("Warning", "Class already exists. Do you want to continue?", "Yes", "No")) {
                    return;
                }
            }

            GenEnum(m_outputPath, m_enumClassName, m_enumNames);
        }
        #endregion

        #region ScriptableObjectGeneration
        [Title("Generate ScriptableObject")]
        [FoldoutGroup("Generate ScriptableObject")]
        [InfoBox("This will generate a ScriptableObject with a dictionary of the selected type.")]
        public Enum ExistingEnum;
        [FoldoutGroup("Generate ScriptableObject")]
        [InfoBox("This will generate a ScriptableObject with a dictionary of the selected type.")]
        public string m_className = "EnumLibrary";
        string SafeClassName => m_className.ConvertToAlphanumeric();
        enum ValueType { Float, Int, Sprite, TextAsset, AudioClip, Color }
        [FoldoutGroup("Generate ScriptableObject")]
        [InfoBox("This will generate a ScriptableObject with a dictionary of the selected type.")]
        [SerializeField] ValueType m_valueType;

        [Button("Populate List")]
        void PopulateByType() {
            Dictionary<ValueType, Type> typeMap = new() {
                { ValueType.Float, typeof(float) },
                { ValueType.Int, typeof(int) },
                { ValueType.Sprite, typeof(Sprite) },
                { ValueType.TextAsset, typeof(TextAsset) },
                { ValueType.AudioClip, typeof(AudioClip) },
                { ValueType.Color, typeof(Color) }
            };

            if (typeMap.TryGetValue(m_valueType, out var type)) {
                CheckAndMakeClass(type);
            }
            else {
                throw new ArgumentOutOfRangeException(nameof(m_valueType), m_valueType, null);
            }
        }


        void GenerateScriptableObject(Type valueType) {
            if (IsNumericType(valueType)) {
                GenScriptableObject(valueType, ExistingEnum, SafeClassName, m_outputPath,true);
            }
            else {
                GenScriptableObject(valueType, ExistingEnum, SafeClassName, m_outputPath);
            }
        }

        void CheckAndMakeClass(Type type) {
            if (ClassExists(SafeClassName)) {
                if (!EditorUtility.DisplayDialog("Warning", "Class already exists. Do you want to continue?", "Yes", "No")) {
                    return;
                }
            }

            GenerateScriptableObject(type);
        }
        #endregion
    }
}