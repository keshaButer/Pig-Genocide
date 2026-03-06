using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float timePeriod;
    public EnemyCreator enemyCreator;
    //private float timer;
    private void Start()
    {
        enemyCreator = new ShooterEnemyCreator();
        enemyCreator.CreateEnemy();
        EventManager.EnemyDied += SpawnEnemy;
    }
    // private void Update()
    // {
    //     // timer += Time.deltaTime;
    //     // if (timer >= timePeriod)
    //     // {

    //     //     timer = 0;
    //     // }
    // }
    public void SpawnEnemy()
    {
        enemyCreator.CreateEnemy();
    }
}
