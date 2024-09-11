using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using static TC.EnumLibrary.ClassParser;

namespace TC.EnumLibrary {
    public class EnumGenerator : OdinEditorWindow {
        [MenuItem("Tools/Enum Generator")]
        static void OpenWindow() => GetWindow<EnumGenerator>().Show();

        [Title("Enum Generator")]
        [FoldoutGroup("Generate Enum")]
        public GenerateEnums m_generateEnums = new();
        [FoldoutGroup("Generate Enum")]
        [Button("Generate Enum")]
        public void GenerateEnum() => m_generateEnums.GenerateEnum();
        
        [Title("Generate ScriptableObject")]
        [FoldoutGroup("Generate ScriptableObject")]
        [InfoBox("This will generate a ScriptableObject with a dictionary of the selected type.")]
        public Enum ExistingEnum;
        [FoldoutGroup("Generate ScriptableObject")]
        [InfoBox("This will generate a ScriptableObject with a dictionary of the selected type.")]
        public string m_className = "EnumLibrary";
        string SafeClassName => m_className.ConvertToAlphanumeric();
        enum ValueType { Float, Int, Sprite, TextAsset, AudioClip }
        [FoldoutGroup("Generate ScriptableObject")]
        [InfoBox("This will generate a ScriptableObject with a dictionary of the selected type.")]
        [SerializeField] ValueType m_valueType;

        [Button("Populate List")]
        void PopulateByType() {
            switch (m_valueType) {
                case ValueType.Float:
                    CheckAndMakeClass(typeof(float));
                    break;
                case ValueType.Int:
                    CheckAndMakeClass(typeof(int));
                    break;
                case ValueType.Sprite:
                    CheckAndMakeClass(typeof(Sprite));
                    break;
                case ValueType.TextAsset:
                    CheckAndMakeClass(typeof(TextAsset));
                    break;
                case ValueType.AudioClip:
                    CheckAndMakeClass(typeof(AudioClip));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(m_valueType), m_valueType, null);
            }
        }

        void GenerateScriptableObject(Type valueType) {
            if (IsNumericType(valueType)) {
                GenScriptableObject(valueType, ExistingEnum, SafeClassName, true);
            }
            else {
                GenScriptableObject(valueType, ExistingEnum, SafeClassName);
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
        static bool IsNumericType(Type type) {
            return type == typeof(int) || type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal) || type == typeof(long) || type == typeof(short) ||
                   type == typeof(byte);
        }

        static bool ClassExists(string className) {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Any(type => type.Name == className);
        }
    }
}