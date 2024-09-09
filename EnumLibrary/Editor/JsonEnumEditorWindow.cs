using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace TC.EnumLibrary
{
    public class JsonEnumEditorWindow : OdinEditorWindow
    {
        [MenuItem("Tools/Json Enum Library Editor")]
        public static void ShowWindow()
        {
            GetWindow<JsonEnumEditorWindow>().Show();
        }

        [Title("Enum Settings")]
        [LabelText("Enum Name")]
        public string m_enumName = "NewJsonEnum";

        [LabelText("Root Folder Path")]
        [FolderPath]
        public string m_rootFolderPath = "Assets/EnumLibrary";

        [Title("Json Files")]
        [ListDrawerSettings(ShowFoldout = true, DefaultExpandedState = true)]
        public List<TextAsset> m_selectedJsonFiles = new();

        [Button(ButtonSizes.Large)]
        public void GenerateEnumAndStaticClass()
        {
            if (!EnumGeneratorUtility.ValidateInput(m_enumName, m_selectedJsonFiles)) return;

            EnumGeneratorUtility.GenerateEnumAndStaticClass(
                m_enumName, m_rootFolderPath, m_selectedJsonFiles.Select(j => j.name), 
                m_selectedJsonFiles, "Json", "TextAsset"
            );

            Debug.Log($"Enum and static class generated at: {m_rootFolderPath}");
        }
    }
}