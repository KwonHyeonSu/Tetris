public enum GameState
{
    None,
    Ready,
    Play,
    End,
    Pause
}

public static class T
{
    //점수
    public static int SCORE = 0;
    public static int MAX_SCORE = 0;
    
    public static GameState CurrentGameState = GameState.None;

}
