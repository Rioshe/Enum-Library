using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace TC.EnumLibrary {
    public class SpriteEnumEditorWindow : OdinEditorWindow {
        [MenuItem("Tools/Sprite Enum Library Editor")]
        public static void ShowWindow() {
            GetWindow<SpriteEnumEditorWindow>().Show();
        }
        [LabelText("Root Folder Path")]
        [FolderPath]
        public string m_rootFolderPath = "Assets/EnumLibrary";
        
        [LabelText("Enum Class Name")]
        [ReadOnly]
        public string m_enumName = "SpriteEnum";

        [LabelText("Namespace")]
        [ReadOnly]
        public string m_namespace = "TC.EnumLibrary";

        [Tooltip("Allows you to customize the enum and namespace names for your generated Files. For advanced users.")]
        public bool m_useCustomNames = false;

        [Title("Enum Settings")]
        [ShowIf("m_useCustomNames")]
        [LabelText("Custom Enum Class Name")]
        public string m_customEnumName = "CustomSpriteEnum";

        [ShowIf("m_useCustomNames")]
        [LabelText("Custom Namespace")]
        public string m_customNamespace = "Custom.Namespace";


        [Title("Sprites")]
        [ListDrawerSettings(ShowFoldout = true, DefaultExpandedState = true)]
        public List<Sprite> m_selectedSprites = new();

        [Button(ButtonSizes.Large)]
        public void GenerateEnumAndStaticClass() {
            if (string.IsNullOrEmpty(m_enumName)) {
                SystemLogging.LogWarning("Enum name is empty. Please provide a valid name.");
                return;
            }

            if (m_selectedSprites.Count == 0) {
                SystemLogging.LogWarning("No sprites selected. Please add sprites.");
                return;
            }

            GenerateEnum();
            GenerateStaticClass();

            SystemLogging.Log($"Enum and static class generated at: {m_rootFolderPath}");
        }

        void GenerateEnum() {
            string enumFolderPath = Path.Combine(m_rootFolderPath, "Enums");
            string enumFilePath = Path.Combine(enumFolderPath, $"{(m_useCustomNames ? m_customEnumName : m_enumName)}.cs");

            if (!LibraryHelpers.GenerateFolderStructureAt(enumFolderPath)) {
                SystemLogging.LogError("Failed to create folder structure for enums.");
                return;
            }

            var enumCode = new StringBuilder();
            enumCode.AppendLine("// Auto-generated enum from SpriteEnumEditor");
            enumCode.AppendLine($"namespace {(m_useCustomNames ? m_customNamespace : m_namespace)} {{");
            enumCode.AppendLine("    public enum " + (m_useCustomNames ? m_customEnumName : m_enumName));
            enumCode.AppendLine("    {");
            enumCode.AppendLine("        None,");

            foreach (string sanitizedSpriteName in m_selectedSprites.Select(sprite => sprite.name.ConvertToAlphanumeric())) {
                enumCode.AppendLine($"        {sanitizedSpriteName},");
            }

            enumCode.AppendLine("    }");
            enumCode.AppendLine("}");

            File.WriteAllText(enumFilePath, enumCode.ToString());
            AssetDatabase.Refresh();
        }

        void GenerateStaticClass() {
            string classFolderPath = Path.Combine(m_rootFolderPath, "Librarys");
            string classFilePath = Path.Combine(classFolderPath, $"{(m_useCustomNames ? m_customEnumName : m_enumName)}Library.cs");

            if (!LibraryHelpers.GenerateFolderStructureAt(classFolderPath)) {
                Debug.LogError("Failed to create folder structure for static classes.");
                return;
            }

            var staticClassCode = new StringBuilder();
            staticClassCode.AppendLine("// Auto-generated static class for sprite dictionary");
            staticClassCode.AppendLine("using System.Collections.Generic;");
            staticClassCode.AppendLine("using UnityEngine;");
            staticClassCode.AppendLine($"namespace {(m_useCustomNames ? m_customNamespace : m_namespace)} {{");
            staticClassCode.AppendLine($"    public static class {(m_useCustomNames ? m_customEnumName : m_enumName)}Library");
            staticClassCode.AppendLine("    {");
            staticClassCode.AppendLine($"        public static readonly Dictionary<{(m_useCustomNames ? m_customEnumName : m_enumName)}, Sprite> Sprites = new() {{");

            foreach (var sprite in m_selectedSprites) {
                string sanitizedSpriteName = sprite.name.ConvertToAlphanumeric();
                staticClassCode.AppendLine($"            {{ {(m_useCustomNames ? m_customEnumName : m_enumName)}.{sanitizedSpriteName}, Resources.Load<Sprite>(\"Sprites/{sprite.name}\") }},");
            }

            staticClassCode.AppendLine("        };");

            // Add GetSprite method
            staticClassCode.AppendLine($"        public static Sprite GetSprite({(m_useCustomNames ? m_customEnumName : m_enumName)} enumValue) {{");
            staticClassCode.AppendLine("            if (Sprites.TryGetValue(enumValue, out var sprite)) {");
            staticClassCode.AppendLine("                return sprite;");
            staticClassCode.AppendLine("            }");
            staticClassCode.AppendLine("            Debug.LogWarning($\"Sprite for {enumValue} not found.\");");
            staticClassCode.AppendLine("            return null;");
            staticClassCode.AppendLine("        }");

            staticClassCode.AppendLine("    }");
            staticClassCode.AppendLine("}");

            File.WriteAllText(classFilePath, staticClassCode.ToString());
            AssetDatabase.Refresh();
        }
    }
}