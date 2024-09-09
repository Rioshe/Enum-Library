using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(SpriteEnumData))]
public class SpriteEnumDataEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var spriteEnumData = (SpriteEnumData)target;

        if (GUILayout.Button("Generate Enum from Sprites")) {
            GenerateEnum(spriteEnumData);
        }
    }

    void GenerateEnum(SpriteEnumData spriteEnumData) {
        List<Sprite> spriteList = spriteEnumData.GetSpriteList();
        var enumName = "SpriteEnum";
        string enumFilePath = "Assets/Scripts/Generated/" + enumName + ".cs"; // Creates enum in Generated folder

        var enumCode = new StringBuilder();
        enumCode.AppendLine("// Auto-generated enum from SpriteEnumData");
        enumCode.AppendLine("public enum " + enumName);
        enumCode.AppendLine("{");
        enumCode.AppendLine("    None, // Default option when nothing is selected");

        foreach (string enumNameSafe in spriteList.Select(t => t.name.Replace(" ", "_"))) {
            enumCode.AppendLine($"    {enumNameSafe},");
        }

        enumCode.AppendLine("}");

        // Ensure directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(enumFilePath) ?? throw new InvalidOperationException());

        // Save the enum to a file
        File.WriteAllText(enumFilePath, enumCode.ToString());
        AssetDatabase.Refresh();

        Debug.Log("Enum generated and saved to " + enumFilePath);
    }
}