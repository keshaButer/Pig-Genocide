using UnityEngine;

public class ShooterEnemyCreator : EnemyCreator
{
    public override void CreateEnemy()
    {
        GameObject prefab = Resources.Load<GameObject>("EnemyShooter");
        GameObject.Instantiate(prefab);
        Debug.Log("Shooter");
    }
    public override void Func()
    {
        Debug.Log("Create Enemy Shooter");
    }
}
