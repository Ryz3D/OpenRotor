using System.IO;

class ConfigManager {
    public static string basePath {
        get {
            string s = UnityEngine.Application.persistentDataPath;
            if (s.EndsWith(Path.DirectorySeparatorChar.ToString()))
                return s;
            else
                return s + Path.DirectorySeparatorChar;
        }
    }
}
