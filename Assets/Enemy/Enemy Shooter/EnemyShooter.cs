using UnityEngine;
using System.Collections;

public class EnemyShooter : EnemyRasher
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireInterval;
    [SerializeField] float minFireInterval = 0.2f, maxFireInterval = 3.0f;
    [SerializeField] float queueInterval;
    [SerializeField] int countComboBullets;
    [SerializeField] Transform spawnBullet;
    [SerializeField] Transform weapon;
    [SerializeField] bool randomizeValues;

    private Coroutine shootCoroutine;
    private float startFireInterval;
    private float timer;
    private int currentCountShots;
    private bool isSeePlayer;

    protected override void Awake()
    {
        base.Awake();
        startFireInterval = fireInterval;

        if (randomizeValues)
            RandomizeValues();
    }

    private void RandomizeValues()
    {
        fireInterval = (int)Random.Range(1, fireInterval + 1);
        queueInterval = Random.Range(3, 6);
        queueInterval /= 10;
        countComboBullets = Random.Range(1, 6);
    }

    protected void Attack()
    {
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            shootCoroutine = StartCoroutine(OrderShoot(countComboBullets));
            timer = 0;
        }
    }
    private IEnumerator OrderShoot(int _countShots)
    {
        currentCountShots = 0;
        while (currentCountShots < _countShots)
        {
            GameObject _bullet = Instantiate(bulletPrefab, spawnBullet.position, spawnBullet.rotation);
            _bullet.transform.localRotation = spawnBullet.rotation;

            currentCountShots++;

            yield return new WaitForSeconds(queueInterval);
        }
        StopCoroutine(shootCoroutine);
    }
    private void Update()
    {
        if (playerTransform != null)
        {
            LookAt(weapon, playerTransform.position);
            // if (isSeePlayer) // РАЗОБРАТЬСЯ
            Attack();
        }
    }
    private void LookAt(Transform _weapon, Vector3 target)
    {
        Vector3 lookAt = _weapon.InverseTransformPoint(target);
        float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg;
        _weapon.Rotate(0, 0, angle - 180);
    }

    public override void ChangeDifficulty(float playerSkill)
    {
        base.ChangeDifficulty(playerSkill);
        fireInterval = Mathf.Clamp(startFireInterval / Mathf.Max(playerSkill, 0.1f), minFireInterval, maxFireInterval);
        Debug.Log($"Change fire interval to: {fireInterval}");

    }
}
