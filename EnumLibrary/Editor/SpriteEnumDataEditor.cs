using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;
using static TC.EnumLibrary.ClassParser;
using static TC.EnumLibrary.GenerateEnums;
using static TC.EnumLibrary.LibraryHelpers;

namespace TC.EnumLibrary {
    public class EnumGenerator : OdinEditorWindow {
        [MenuItem("Tools/Enum Library")]
        static void OpenWindow() => GetWindow<EnumGenerator>().Show();
        [FolderPath] public string m_outputPath = "Assets/EnumLibrary/Generated";
        public string m_namespace = "TC.EnumLibrary.Generated";
        string SafeNamespace => string.IsNullOrEmpty(m_namespace) ? "EnumLibrary" : m_namespace.ConvertToAlphanumeric();

        #region EnumClassGeneration
        [Title("Enum Generator")]
        [FoldoutGroup("Generate Enum")]
        [Required]
        public string m_enumClassName = "EnumLibrary";
        [Space(10)]
        [FoldoutGroup("Generate Enum")]
        [InfoBox("Enter the names for the enum values and click 'Generate Enum'")]
        public List<string> m_enumNames = new();
        [FoldoutGroup("Generate Enum")]
        [Button("Generate Enum")]
        public void GenerateEnum() {
            if (ClassExists(m_enumClassName)) {
                if (!EditorUtility.DisplayDialog("Warning", 
                                                 "Class already exists. Do you want to continue?", 
                                                 "Yes", "No")) {
                    return;
                }
            }

            GenEnum(m_outputPath, m_enumClassName, m_enumNames);
        }
        #endregion

        #region ScriptableObjectGeneration
        [Title("Generate ScriptableObject")]
        [FoldoutGroup("Generate ScriptableObject")]
        [InfoBox("Enter the name for the ScriptableObject class." +
                  " Select the return type and corresponding value type." +
                  " For numeric types, choose from the dropdown." +
                  " For Unity objects, select the appropriate asset type. " +
                  "Click 'Generate Enum Library' to create the ScriptableObject.")]
        [Required]
        public Enum ExistingEnum;
        [Space(10)]
        [FoldoutGroup("Generate ScriptableObject")]
        [Tooltip("Enter the name for the ScriptableObject class.")]
        public string m_className = "EnumLibrary";
        string SafeClassName => string.IsNullOrEmpty(m_className) ? "EnumLibrary" : m_className.ConvertToAlphanumeric();

        [Space(20)]
        [FoldoutGroup("Generate ScriptableObject")]
        [Tooltip("Select the return type for the ScriptableObject.")]
        [SerializeField] ReturnType m_returnType;
        [Space(10)]
        [FoldoutGroup("Generate ScriptableObject")]
        [Tooltip("Select the numeric type for the ScriptableObject.")]
        [ShowIf("m_showNumeric")]
        [SerializeField] NumericType m_numericType;
        [Space(10)]
        [FormerlySerializedAs("m_unityObjectType")]
        [FoldoutGroup("Generate ScriptableObject")]
        [Tooltip("Select the Unity object type for the ScriptableObject.")]
        [HideIf("m_showNumeric")]
        [SerializeField] AssetObject m_assetObject;
        [Space(10)]

        #region Unity Events
        bool m_showNumeric;
        void OnBecameVisible() {
            SetReturnType();
        }
        void OnValidate() {
            SetReturnType();
        }
        void SetReturnType() {
            m_showNumeric = m_returnType switch {
                ReturnType.Numeric => true,
                ReturnType.UnityObject => false,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        #endregion
        [EnumPaging] [ShowInInspector] [HideLabel] [HideReferenceObjectPicker]
        enum ReturnType {
            UnityObject, Numeric
        }

        [Button("Generate Enum Library")]
        [FoldoutGroup("Generate ScriptableObject")]
        void Generate() {
            switch (m_returnType) {
                case ReturnType.Numeric:
                    GenerateNumeric();
                    break;
                case ReturnType.UnityObject:
                    GenerateUnityObject();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        [EnumPaging] [ShowInInspector] [HideLabel] [HideReferenceObjectPicker]
        enum NumericType {
            Float, Int, Double, Long, Short, Byte, UInt, ULong, UShort, SByte, Decimal
        }
        void GenerateNumeric() {
            Dictionary<NumericType, Type> typeMap = new() {
                { NumericType.Float, typeof(float) }, { NumericType.Int, typeof(int) },
                { NumericType.Double, typeof(double) }, { NumericType.Long, typeof(long) },
                { NumericType.Short, typeof(short) }, { NumericType.Byte, typeof(byte) },
                { NumericType.UInt, typeof(uint) }, { NumericType.ULong, typeof(ulong) },
                { NumericType.UShort, typeof(ushort) }, { NumericType.SByte, typeof(sbyte) },
                { NumericType.Decimal, typeof(decimal) },
            };

            if (typeMap.TryGetValue(m_numericType, out var type)) {
                CheckAndMakeClass(type);
            }
            else {
                throw new ArgumentOutOfRangeException(nameof(m_numericType), m_numericType, null);
            }
        }
        [EnumPaging] [ShowInInspector] [HideLabel] [HideReferenceObjectPicker]
        enum AssetObject {
            Sprite, TextAsset, AudioClip, Color, VideoClip, Font, AnimationClip, GameObject, Material,
            Texture, Mesh, Shader, PhysicMaterial, RenderTexture, Light, Camera, ScriptableObject,
        }
        void GenerateUnityObject() {
            Dictionary<AssetObject, Type> typeMap = new() {
                { AssetObject.Sprite, typeof(Sprite) }, { AssetObject.TextAsset, typeof(TextAsset) },
                { AssetObject.AudioClip, typeof(AudioClip) }, { AssetObject.Color, typeof(Color) },
                { AssetObject.VideoClip, typeof(VideoClip) }, { AssetObject.Font, typeof(Font) },
                { AssetObject.AnimationClip, typeof(AnimationClip) }, { AssetObject.GameObject, typeof(GameObject) },
                { AssetObject.Material, typeof(Material) }, { AssetObject.Texture, typeof(Texture) },
                { AssetObject.Mesh, typeof(Mesh) }, { AssetObject.Shader, typeof(Shader) },
                { AssetObject.PhysicMaterial, typeof(PhysicsMaterial) }, { AssetObject.RenderTexture, typeof(RenderTexture) },
                { AssetObject.Light, typeof(Light) }, { AssetObject.Camera, typeof(Camera) },
                { AssetObject.ScriptableObject, typeof(ScriptableObject) },
            };

            if (typeMap.TryGetValue(m_assetObject, out var type)) {
                CheckAndMakeClass(type);
            }
            else {
                throw new ArgumentOutOfRangeException(nameof(m_assetObject), m_assetObject, null);
            }
        }


        void GenerateScriptableObject(Type valueType) {
            if (IsNumericType(valueType)) {
                GenScriptableObject(valueType, ExistingEnum, SafeClassName, SafeNamespace, m_outputPath, true);
            } else {
                GenScriptableObject(valueType, ExistingEnum, SafeClassName, SafeNamespace, m_outputPath);
            }

            // Create the ScriptableObject asset
            CreateScriptableObjectAsset(SafeClassName, m_outputPath);
        }

        void CreateScriptableObjectAsset(string className, string outputPath) {
            string directoryPath = Path.GetDirectoryName(outputPath);
            if (!ValidateDirectoryPath(directoryPath)) return;
            
            if (!GenerateFolderStructureAt(directoryPath)) {
                SystemLogging.LogWarning("Failed to generate folder structure at: " + directoryPath);
                return;
            }

            if (directoryPath == null) return;
            string assetPath = Path.Combine(directoryPath, className + ".asset");
            var scriptableObject = CreateInstance(className);
            AssetDatabase.CreateAsset(scriptableObject, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(scriptableObject);
            SystemLogging.Log($"ScriptableObject asset created at: {assetPath}");
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