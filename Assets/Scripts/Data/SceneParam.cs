public enum LevelSelectType {
    Unknown,
    Freestyle,
    Race,
    Custom,
    Test,
    Leaderboard
}

public static class SceneParam {
    public static LevelSelectType selectType;
    public static string selectedLevel;
}
