class ConfigManager {
    public static string basePath {
        get {
            string s = UnityEngine.Application.persistentDataPath;
            s = s.Replace('/', '\\');
            if (s.EndsWith("\\"))
                return s;
            else
                return s + "\\";
        }
    }
}
