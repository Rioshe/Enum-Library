using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace TC.EnumLibrary
{
    public class SpriteEnumEditorWindow : OdinEditorWindow
    {
        [MenuItem("Tools/Sprite Enum Library Editor")]
        public static void ShowWindow()
        {
            GetWindow<SpriteEnumEditorWindow>().Show();
        }

        [Title("Enum Settings")]
        [LabelText("Enum Name")]
        public string m_enumName = "NewSpriteEnum";

        [LabelText("Root Folder Path")]
        [FolderPath]
        public string m_rootFolderPath = "Assets/EnumLibrary";

        [Title("Sprites")]
        [ListDrawerSettings(ShowFoldout = true, DefaultExpandedState = true)]
        public List<Sprite> m_selectedSprites = new();

        [Button(ButtonSizes.Large)]
        public void GenerateEnumAndStaticClass()
        {
            if (!EnumGeneratorUtility.ValidateInput(m_enumName, m_selectedSprites)) return;

            EnumGeneratorUtility.GenerateEnumAndStaticClass(
                m_enumName, m_rootFolderPath, m_selectedSprites.Select(s => s.name), 
                m_selectedSprites, "Sprites", "Sprite"
            );

            Debug.Log($"Enum and static class generated at: {m_rootFolderPath}");
        }
    }
}