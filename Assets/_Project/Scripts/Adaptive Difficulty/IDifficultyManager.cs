public interface IDifficultyManager
{
    event System.Action<float> OnDifficultyChanged;
    float CurrentDifficulty { get; }
}