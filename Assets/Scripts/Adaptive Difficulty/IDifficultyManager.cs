public interface IDifficultyManager
{
    event System.Action<float> OnDifficultyChanged;
    void UpdateDifficulty();
}