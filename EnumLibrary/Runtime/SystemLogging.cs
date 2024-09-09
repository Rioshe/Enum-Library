using UnityEngine;

namespace TC.EnumLibrary {
    public static class SystemLogging {
        const string PREFIX = "EnumLibrary: ";
        public static void Log(string message) => Debug.Log(PREFIX + message);
        public static void LogWarning(string message) => Debug.LogWarning(PREFIX + message);
        public static void LogError(string message) => Debug.LogError(PREFIX + message);
    }
}