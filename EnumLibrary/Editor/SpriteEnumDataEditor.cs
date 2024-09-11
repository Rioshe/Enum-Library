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
        public Enum ExistingEnum;
        public string m_className = "EnumLibrary";
        enum ValueType { Float, Int, Sprite, TextAsset, AudioClip }
        [SerializeField] ValueType m_valueType;
        Dictionary<Enum, float> m_floatValues;
        Dictionary<Enum, int> m_intValues;
        Dictionary<Enum, Sprite> m_icons;
        Dictionary<Enum, TextAsset> m_files;
        Dictionary<Enum, AudioClip> m_audioFiles;
        Dictionary<Enum, Color> m_colors;
        Dictionary<Enum, Material> m_materials;
        Dictionary<Enum, GameObject> m_gameObjects;

        [Button("Populate List")]
        void PopulateByType() {
            switch (m_valueType) {
                case ValueType.Float:
                    PopulateEnumDict(m_floatValues);
                    CheckAndMakeClass(typeof(float));
                    break;
                case ValueType.Int:
                    PopulateEnumDict(m_intValues);
                    CheckAndMakeClass(typeof(int));
                    break;
                case ValueType.Sprite:
                    PopulateEnumDict(m_icons);
                    CheckAndMakeClass(typeof(Sprite));
                    break;
                case ValueType.TextAsset:
                    PopulateEnumDict(m_files);
                    CheckAndMakeClass(typeof(TextAsset));
                    break;
                case ValueType.AudioClip:
                    PopulateEnumDict(m_audioFiles);
                    CheckAndMakeClass(typeof(AudioClip));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(m_valueType), m_valueType, null);
            }
        }

        void PopulateEnumDict<T>(Dictionary<Enum, T> items) {
            if (ExistingEnum == null) {
                Debug.LogWarning("An existing enum must be selected to populate the dictionary.");
                return;
            }

            IItemPopulator<T> populator = PopulatorFactory.GetPopulator<T>();
            populator.Populate(items, ExistingEnum);
        }

        void GenerateScriptableObject(Type valueType) {
            if (IsNumericType(valueType)) {
                GenerateScriptableObjectForNumeric(valueType, ExistingEnum);
            }
            else {
                GenerateScriptableObjectForGeneric(valueType, ExistingEnum);
            }
        }
        
        void CheckAndMakeClass(Type type) {
            if (ClassExists(m_className)) {
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

        public static string GetNumericTypeName(Type type) {
            if (type == typeof(int)) return "int";
            if (type == typeof(float)) return "float";
            if (type == typeof(double)) return "double";
            if (type == typeof(decimal)) return "decimal";
            if (type == typeof(long)) return "long";
            if (type == typeof(short)) return "short";
            return type == typeof(byte) ? "byte" : "float"; // Fallback, though ideally this should never be reached
        }

        static bool ClassExists(string className) {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Any(type => type.Name == className);
        }
    }
}