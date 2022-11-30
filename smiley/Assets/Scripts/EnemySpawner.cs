using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject enemyPrefab;
    public ParticleSystem enemyDeathParticlePrefab;
    private readonly int[] odds = new[] { 8, 12, 5, 4, 4, 3, 3 };

    private readonly float spawnDistance = 15f;
    private readonly float spawnReductionRate = 0.01f;
    private readonly float speedIncrementRate = 0.01f;
    private readonly float minSpawnRate = 0.4f;

    // public so they can be accessed by other scripts such as TakeDamage & EnemySpawner
    [HideInInspector] public float spawnInterval = 1f;
    [HideInInspector] public float averageMoveSpeed;
    [HideInInspector] public int enemiesKilled;

    private void Start()
    {
        spawnInterval = 1.2f;
        averageMoveSpeed = 2f;

        // pass optional true param to make it wait an extra 2s before spawning first enemy
        StartCoroutine(SpawnEnemy(true));

        gameManager = FindObjectOfType<GameManager>();

        EventBroker.EnemyKilled += EnemyKilled;
    }

    private IEnumerator SpawnEnemy(bool firstEnemy = false)
    {
        // wait 2s if this is the first enemy
        if (firstEnemy)
            yield return new WaitForSeconds(2f);

        // get random point 15 units away
        Vector2 spawnDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Vector2 spawnPos = spawnDir * spawnDistance;

        // spawn enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);

        // choose a behaviour
        if (odds.Sum() != 39)
            Debug.LogError("Enemy spawn odds do not equal 21.");
        int behaviour = Random.Range(0, 39);
        //behaviour = 38;

        // go straight to center; just set rotation (possibility of bumping into another & missing)
        if (behaviour < odds[0])
        {
            enemy.AddComponent<Straight>().moveSpeed = averageMoveSpeed;
        }
        // move in curvy sine wave 
        else if (behaviour < odds[0] + odds[1])
        {
            enemy.AddComponent<Sinusoidal>().moveSpeed = averageMoveSpeed;
        }
        // circling sharks >:D
        else if (behaviour < odds[0] + odds[1] + odds[2])
        {
            enemy.AddComponent<Circle>().moveSpeed = averageMoveSpeed;
        }
        // russian doll
        else if (behaviour < odds[0] + odds[1] + odds[2] + odds[3])
        {
            RussianDoll enemyScript = enemy.AddComponent<RussianDoll>();
            enemyScript.moveSpeed = averageMoveSpeed;
            enemyScript.enemyPrefab = enemyPrefab;
        }
        // 3 hit
        else if (behaviour < odds[0] + odds[1] + odds[2] + odds[3] + odds[4])
        {
            enemy.AddComponent<ThreeHit>().moveSpeed = averageMoveSpeed;
        }
        // mothership
        else if (behaviour < odds[0] + odds[1] + odds[2] + odds[3] + odds[4] + odds[5])
        {
            Mothership enemyScript = enemy.AddComponent<Mothership>();
            enemyScript.moveSpeed = averageMoveSpeed;
            enemyScript.enemyPrefab = enemyPrefab;
        }
        // charge up
        else if (behaviour < odds[0] + odds[1] + odds[2] + odds[3] + odds[4] + odds[5] + odds[6])
        {
            ChargeUp enemyScript = enemy.AddComponent<ChargeUp>();
            enemyScript.moveSpeed = averageMoveSpeed;
        }
        enemy.GetComponent<Enemy>().deathParticle = enemyDeathParticlePrefab;

        yield return new WaitForSeconds(spawnInterval);

        if (!gameManager.gameOver)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        // raise enemy speed & spawn rate
        averageMoveSpeed += speedIncrementRate;
        if (spawnInterval > minSpawnRate)
        {
            spawnInterval -= spawnReductionRate;
        }

        gameManager.UpdateScore(enemiesKilled);
    }

    private void OnDisable()
    {
        EventBroker.EnemyKilled -= EnemyKilled;
    }
}
