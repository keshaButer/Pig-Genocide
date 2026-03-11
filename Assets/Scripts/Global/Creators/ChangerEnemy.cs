using System.Collections;
using UnityEngine;

public class ChangerEnemy : MonoBehaviour
{
    [SerializeField] float changeEnemyInterval = 5;
    [SerializeField] GameObject enemySpawnerObj;
    //private EnemySpawner enemySpawner;
    //private float timer;
    void Start()
    {
        //enemySpawner = enemySpawnerObj.GetComponent<EnemySpawner>();

        //StartCoroutine(nameof(ChangeEnemy));
        //EventManager.EnemyDied += ChangeEnemy;
    }
    // private IEnumerator ChangeEnemy()
    // {
    //     while (true)
    //     {
    //         timer += Time.deltaTime;
    //         if (timer >= changeEnemyInterval)
    //         {
    //             ChangeEnemy();
    //             timer = 0;
    //         }
    //         yield return null;
    //     }
    // }
    private void Update()
    {
        // timer += Time.deltaTime;
        // if (timer >= timePeriod)
        // {

        //     timer = 0;
        // }
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     EventManager.OnEnemyDied();
        // }
    }
    // private void ChangeEnemy()
    // {
    //     int n = Random.Range(0, 2);
    //     if (n == 1) enemySpawnerObj.GetComponent<EnemySpawner>().enemyCreator = new ShooterEnemyCreator();
    //     else enemySpawnerObj.GetComponent<EnemySpawner>().enemyCreator = new KamikazeEnemyCreator();
    // }
}
