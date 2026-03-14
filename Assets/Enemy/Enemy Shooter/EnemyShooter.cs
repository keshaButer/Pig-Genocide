using UnityEngine;
using System.Collections;

public class EnemyShooter : EnemyRasher
{
    // [SerializeField] GameObject bulletPrefab;
    // [SerializeField] float fireInterval;
    // [SerializeField] float queueInterval;
    // [SerializeField] int countComboBullets;
    // [SerializeField] Transform spawnBullet;
    // [SerializeField] Transform weapon;
    // [SerializeField] bool randomizeValues;
    // private Coroutine shootCoroutine;
    // private float timer;
    // private int currentCountShots;
    // void Awake()
    // {
    //     switch (direction)
    //     {
    //         case 1:
    //             transform.rotation = Quaternion.Euler(0, 180, 0);
    //             break;
    //         case -1:
    //             transform.rotation = Quaternion.Euler(0, 0, 0);
    //             break;
    //     }
    //     if (randomizeValues)
    //         RandomizeValues();
    // }
    // void Start() => Init();
    // public override void Init() => base.Init();
    // private void RandomizeValues()
    // {
    //     fireInterval = (int)Random.Range(1, fireInterval + 1);
    //     queueInterval = Random.Range(3, 6);
    //     queueInterval /= 10;
    //     countComboBullets = Random.Range(1, 6);
    // }
    //
    // protected override void Attack()
    // {
    //     timer += Time.deltaTime;
    //     if (timer >= fireInterval)
    //     {
    //         shootCoroutine = StartCoroutine(OrderShoot(countComboBullets));
    //         timer = 0;
    //     }
    // }
    // private IEnumerator OrderShoot(int _countShots)
    // {
    //     currentCountShots = 0;
    //     while (currentCountShots < _countShots)
    //     {
    //         GameObject _bullet = Instantiate(bulletPrefab, spawnBullet.position, spawnBullet.rotation);
    //         _bullet.transform.localRotation = spawnBullet.rotation;
    //
    //         currentCountShots++;
    //
    //         yield return new WaitForSeconds(queueInterval);
    //     }
    //     StopCoroutine(shootCoroutine);
    // }
    // private void Update()
    // {
    //     if (CurrrentState == States.fight && playerPos != null)
    //     {
    //         LookAt(weapon, playerPos.position);
    //         if (isSeePlayer)
    //             Attack();
    //     }
    // }
    // private void LookAt(Transform _weapon, Vector3 target)
    // {
    //     Vector3 lookAt = _weapon.InverseTransformPoint(target);
    //     float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg;
    //     _weapon.Rotate(0, 0, angle - 180);
    // }
    //
    // public void HandleSound(Transform origin)
    // {
    //     if (CurrrentState == States.chill)
    //     {
    //         if (chillCoroutine != null)
    //         {
    //             StopCoroutine(chillCoroutine);
    //             chillCoroutine = null;
    //         }
    //         if (startChillAlgorithm != null)
    //         {
    //             StopCoroutine(startChillAlgorithm);
    //             startChillAlgorithm = null;
    //         }
    //         if (getToPointCoroutine != null)
    //         {
    //             StopCoroutine(getToPointCoroutine);
    //             getToPointCoroutine = null;
    //         }
    //
    //         getToPointCoroutine = StartCoroutine(GetToPoint(origin.position, 5, fightSpeed));
    //         print($"Развернулся к звуку - {origin.position}");
    //     }
    // }
}
