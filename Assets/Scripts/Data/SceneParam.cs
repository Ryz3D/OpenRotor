public enum LevelSelectType {
    Unknown,
    Freestyle,
    Race,
    Custom,
    Test,
    Leaderboard
}

public static class SceneParam {
    public static LevelSelectType selectType = LevelSelectType.Unknown;
    public static string selectedLevel = "";
    public static string lastScene = "Start";
}
