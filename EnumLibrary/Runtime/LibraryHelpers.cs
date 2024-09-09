using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
namespace TC.EnumLibrary {
    public static class LibraryHelpers {
        /// <summary>
        /// Creates an asset at the specified path, creating any necessary folders along the way.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="path"></param>
        /// <returns>False if the operation was unsuccessful or was cancelled, 
        /// True if an asset was created.</returns>
        public static bool CreateAssetSafe(Object asset, string path) {
            if (AssetDatabase.IsValidFolder(path)) {
                Debug.LogError("Error! Attempted to write an asset over a folder!");
                return false;
            }

            string folderPath = path[..path.LastIndexOf("/", StringComparison.Ordinal)];
            if (!GenerateFolderStructureAt(folderPath)) return false;
            AssetDatabase.CreateAsset(asset, path);
            return true;

        }

        /// <summary>
        /// Generates the folder structure to a specified path if it doesn't already exist. 
        /// Will perform the check itself first
        /// </summary>
        /// <param name="folderPath">The FOLDER path, this should NOT include any file names</param>
        /// <param name="ask">Asks if you want to generate the folder structure</param>
        /// <returns>False if the user cancels the operation, 
        /// True if there was no need to generate anything or if the operation was successful</returns>
        public static bool GenerateFolderStructureAt(string folderPath, bool ask = true) {
            // Convert slashes so we can use the Equals operator together with other file-system operations
            folderPath = folderPath.Replace("/", "\\");
            if (AssetDatabase.IsValidFolder(folderPath)) return true;
            var existingPath = "Assets";
            string unknownPath = folderPath.Remove(0, existingPath.Length + 1);
            // Remove the "Assets/" at the start of the path name
            string folderName = (unknownPath.Contains("\\")) ? unknownPath[..unknownPath.IndexOf("\\", StringComparison.Ordinal)] : unknownPath;
            do {
                string newPath = Path.Combine(existingPath, folderName);
                // Begin checking down the file path to see if it's valid
                if (!AssetDatabase.IsValidFolder(newPath)) {
                    var createFolder = true;
                    if (ask) {
                        createFolder = EditorUtility
                            .DisplayDialog("Path does not exist!",
                                "The folder "
                                + "\""
                                + newPath
                                + "\" does not exist! Would you like to create this folder?", "Yes", "No");
                    }

                    if (createFolder) {
                        AssetDatabase.CreateFolder(existingPath, folderName);
                    }
                    else return false;
                }

                existingPath = newPath;
                // Full path still doesn't exist
                if (existingPath.Equals(folderPath)) continue;
                unknownPath = unknownPath.Remove(0, folderName.Length + 1);
                folderName = (unknownPath.Contains("\\")) ? unknownPath[..unknownPath.IndexOf("\\", StringComparison.Ordinal)] : unknownPath;
            } while (!AssetDatabase.IsValidFolder(folderPath));

            return true;
        }

        public static List<T> ImportAssetsOrFoldersAtPath<T>(string filePath) where T : Object {
            var asset = AssetDatabase.LoadAssetAtPath<T>(filePath);
            if (!AssetDatabase.IsValidFolder(filePath)) {
                if (asset) {
                    return new List<T> { asset };
                }
            }
            else {
                List<T> imports = new List<T>();
                List<string> importTarget = new List<string>(Directory.GetDirectories(filePath));
                importTarget.AddRange(Directory.GetFiles(filePath));
                foreach (string t in importTarget) {
                    imports.AddRange(ImportAssetsOrFoldersAtPath<T>(t));
                }

                return imports;
            }

            return new List<T>();
        }
        
        /// <summary>
        /// Returns an enum type given it's name as a string
        /// https://stackoverflow.com/questions/25404237/how-to-get-enum-type-by-specifying-its-name-in-string
        /// </summary>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static System.Type GetEnumType(string enumName) {
            if (enumName.IsNullOrWhiteSpace()) return null;
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            return assemblies
                .Select(t =>
                    t.GetType(enumName))
                .Where(type => type != null)
                .FirstOrDefault(type => type.IsEnum);
        }

        /// <summary>
        /// Converts the input string to an alphanumeric string, optionally allowing periods.
        /// </summary>
        /// <param name="input">The input string to be converted.</param>
        /// <param name="allowPeriods">A boolean flag indicating whether periods should be allowed in the output string.</param>
        /// <returns>
        /// A new string containing only alphanumeric characters, underscores, and optionally periods.
        /// If the input string is null or empty, an empty string is returned.
        /// </returns>
        public static string ConvertToAlphanumeric(this string input, bool allowPeriods = false) {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            List<char> filteredChars = new List<char>();
            int lastValidIndex = -1;

            // Iterate over the input string, filtering and determining valid start/end indices
            foreach (char character in input
                         .Where(character => char
                             .IsLetterOrDigit(character) || character == '_' || (allowPeriods && character == '.'))
                         .Where(character => filteredChars.Count != 0 || (!char.IsDigit(character) && character != '.'))) {

                filteredChars.Add(character);
                lastValidIndex = filteredChars.Count - 1; // Update lastValidIndex for valid characters
            }

            // Remove trailing periods
            while (lastValidIndex >= 0 && filteredChars[lastValidIndex] == '.') {
                lastValidIndex--;
            }

            // Return the filtered string
            return lastValidIndex >= 0
                ? new string(filteredChars.ToArray(), 0, lastValidIndex + 1) : string.Empty;
        }
        
        /// <summary>
        /// Returns true if this AudioFile houses a .WAV
        /// </summary>
        /// <returns></returns>
        public static bool IsWavFile(this AudioClip audioClip) {
            string filePath = AssetDatabase.GetAssetPath(audioClip);
            string trueFilePath = Application.dataPath.Remove(Application.dataPath.LastIndexOf("/", StringComparison.Ordinal) + 1) + filePath;
            string fileExtension = trueFilePath[^4..];
            return fileExtension == ".wav";
        }
    }
}