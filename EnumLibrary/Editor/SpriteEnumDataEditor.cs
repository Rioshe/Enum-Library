using System;
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
        
        [Title("Enum Settings")]
        [LabelText("Enum Name")]
        public string m_enumName = "NewSpriteEnum";
        
        [LabelText("Folder Path")]
        public string m_folderPath = "Assets/EnumLibrary";
        
        [Title("Sprites")]
        [ListDrawerSettings(ShowFoldout = true, DefaultExpandedState = true)]
        public List<Sprite> m_selectedSprites = new();

        [Button(ButtonSizes.Large)]
        public void GenerateEnumAndStaticClass() {
            if (string.IsNullOrEmpty(m_enumName)) {
                Debug.LogWarning("Enum name is empty. Please provide a valid name.");
                return;
            }

            if (m_selectedSprites.Count == 0) {
                Debug.LogWarning("No sprites selected. Please add sprites.");
                return;
            }

            GenerateEnum();
            GenerateStaticClass();

            Debug.Log($"Enum and static class generated at: {m_folderPath}");
        }

        void GenerateEnum() {
            var enumFilePath = $"{m_folderPath}/{m_enumName}.cs";

            var enumCode = new StringBuilder();
            enumCode.AppendLine("// Auto-generated enum from SpriteEnumEditor");
            enumCode.AppendLine("namespace TC.EnumLibrary {");
            enumCode.AppendLine("    public enum " + m_enumName);
            enumCode.AppendLine("    {");
            enumCode.AppendLine("        None, // Default option when nothing is selected");

            foreach (string sanitizedSpriteName in m_selectedSprites.Select(sprite => sprite.name.Replace(" ", "_"))) {
                enumCode.AppendLine($"        {sanitizedSpriteName},");
            }

            enumCode.AppendLine("    }");
            enumCode.AppendLine("}");

            Directory.CreateDirectory(Path.GetDirectoryName(enumFilePath) ?? throw new InvalidOperationException());
            File.WriteAllText(enumFilePath, enumCode.ToString());
            AssetDatabase.Refresh();
        }

        void GenerateStaticClass() {
            var classFilePath = $"{m_folderPath}/{m_enumName}Library.cs";
            var staticClassCode = new StringBuilder();
            staticClassCode.AppendLine("// Auto-generated static class for sprite dictionary");
            staticClassCode.AppendLine("using System.Collections.Generic;");
            staticClassCode.AppendLine("using UnityEngine;");
            staticClassCode.AppendLine("namespace TC.EnumLibrary {");
            staticClassCode.AppendLine($"    public static class {m_enumName}Library");
            staticClassCode.AppendLine("    {");
            staticClassCode.AppendLine($"        public static readonly Dictionary<{m_enumName}, Sprite> Sprites = new() {{");

            foreach (var sprite in m_selectedSprites) {
                string sanitizedSpriteName = sprite.name.Replace(" ", "_");
                staticClassCode.AppendLine($"            {{ {m_enumName}.{sanitizedSpriteName}, Resources.Load<Sprite>(\"Sprites/{sprite.name}\") }},");
            }

            staticClassCode.AppendLine("        };");

            // Add GetSprite method
            staticClassCode.AppendLine($"        public static Sprite GetSprite({m_enumName} enumValue) {{");
            staticClassCode.AppendLine("            if (Sprites.TryGetValue(enumValue, out var sprite)) {");
            staticClassCode.AppendLine("                return sprite;");
            staticClassCode.AppendLine("            }");
            staticClassCode.AppendLine("            Debug.LogWarning($\"Sprite for {enumValue} not found.\");");
            staticClassCode.AppendLine("            return null;");
            staticClassCode.AppendLine("        }");

            staticClassCode.AppendLine("    }");
            staticClassCode.AppendLine("}");

            Directory.CreateDirectory(Path.GetDirectoryName(classFilePath) ?? throw new InvalidOperationException());
            File.WriteAllText(classFilePath, staticClassCode.ToString());
            AssetDatabase.Refresh();
        }
    }
}