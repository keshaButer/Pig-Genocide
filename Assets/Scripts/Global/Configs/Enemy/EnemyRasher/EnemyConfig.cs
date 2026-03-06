using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRasherConfig", menuName = "Configs/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [field: SerializeField] public static int StartHealth { get; private set; }
    [field: Range(0, 5)]
    [field: SerializeField] public static int Damage { get; private set; }
}
