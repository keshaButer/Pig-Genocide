using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyConfig", menuName = "Configs/Difficulty")]
public class DifficultyConfig : ScriptableObject
{
    public float ChangeRate = 2f;
    public float KillMeaning = 0.3f;
    public float TimeMeaning = 0.15f;
}
