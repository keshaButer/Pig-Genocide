using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStandart", menuName = "Enemy Configs")]

public class EnemyConfig : ScriptableObject
{
    public int maxHealth;
    public float delayToDestroy;
    public int collisionDamage;
    public float massOnDeath;
    public AudioClip deathSound;
}
