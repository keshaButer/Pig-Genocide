using UnityEngine;

public class KamikazeEnemyCreator : EnemyCreator
{
    public override void CreateEnemy()
    {
        GameObject prefab = Resources.Load<GameObject>("EnemyKamikaze");
        GameObject.Instantiate(prefab);
        Debug.Log("Kamikaze");
    }
    public override void Func()
    {
        Debug.Log("Create Enemy Kamikaze");
    }
}
