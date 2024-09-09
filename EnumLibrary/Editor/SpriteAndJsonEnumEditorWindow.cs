using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace TC.EnumLibrary {
    public enum LibraryType {
        Sprite,
        Json
    }

    public class SpriteAndJsonEnumEditorWindow : OdinEditorWindow {
        [MenuItem("Tools/Enum Library Editor")]
        public static void ShowWindow() {
            GetWindow<SpriteAndJsonEnumEditorWindow>().Show();
        }

        [Title("Enum Settings")]
        [LabelText("Enum Name")]
        public string m_enumName = "NewEnum";

        [LabelText("Root Folder Path")]
        [FolderPath]
        public string m_rootFolderPath = "Assets/EnumLibrary";

        [Title("Editor Mode")]
        [EnumToggleButtons, LabelText("Select Enum LibraryType")]
        public LibraryType m_enumLibraryType = LibraryType.Sprite;

        [Title("Sprites")]
        [ShowIf("m_enumLibraryType", LibraryType.Sprite)]
        [ListDrawerSettings(ShowFoldout = true, DefaultExpandedState = true)]
        public List<Sprite> m_selectedSprites = new();

        [Title("Json Files")]
        [ShowIf("m_enumLibraryType", LibraryType.Json)]
        [ListDrawerSettings(ShowFoldout = true, DefaultExpandedState = true)]
        public List<TextAsset> m_selectedJsonFiles = new();

        [Button(ButtonSizes.Large), LabelText("Generate Enum and Static Class")]
        public void GenerateEnumAndStaticClass() {
            if (m_enumLibraryType == LibraryType.Sprite) {
                if (!EnumGeneratorUtility.ValidateInput(m_enumName, m_selectedSprites)) return;

                EnumGeneratorUtility.GenerateEnumAndStaticClass(
                    m_enumName, m_rootFolderPath, m_selectedSprites.Select(s => s.name),
                    m_selectedSprites, "Sprites", "Sprite"
                );
            }
            else if (m_enumLibraryType == LibraryType.Json) {
                if (!EnumGeneratorUtility.ValidateInput(m_enumName, m_selectedJsonFiles)) return;

                EnumGeneratorUtility.GenerateEnumAndStaticClass(
                    m_enumName, m_rootFolderPath, m_selectedJsonFiles.Select(j => j.name),
                    m_selectedJsonFiles, "Json", "TextAsset"
                );
            }

            Debug.Log($"Enum and static class generated at: {m_rootFolderPath}");
        }
    }
}