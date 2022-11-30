using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mothership : Enemy
{
    [HideInInspector] public GameObject enemyPrefab;
    [HideInInspector] public int dir;
    private float turnAngle = 25f;

    private readonly float scaleModifier = 1.5f;

    private float targetDistFromSmiley = 6f;

    private void Start()
    {
        // dont collide with other enemies
        gameObject.layer = 9;

        // random circling direction
        dir = Random.Range(0, 2);
        if (dir == 0)
            dir = -1;

        transform.localScale = new Vector3(transform.localScale.x * scaleModifier, transform.localScale.y * scaleModifier, 1f);

        targetDistFromSmiley += Random.Range(-0.5f, 0.5f);
        ChangeColour("#D3FFCC");
        StartCoroutine(GetIntoPosition());
    }

    private void LateUpdate()
    {
        // get rotation to the center
        Vector3 vectorToCenter = Vector3.zero - transform.position;
        rotationToCenter = Mathf.Atan2(vectorToCenter.y, vectorToCenter.x) * Mathf.Rad2Deg;
        // compensate, couldnt get it to work doing other thing hahahahahh
        rotationToCenter -= 90f;
        // move at an angle a little less than 90
        rotationToCenter += (90f - turnAngle) * dir;

        Quaternion rotation = Quaternion.AngleAxis(rotationToCenter, Vector3.forward);
        transform.rotation = rotation;
    }

    private IEnumerator GetIntoPosition()
    {
        // wait until we at the right distance
        float distToSmiley = (transform.position - Vector3.zero).magnitude;
        while (distToSmiley > targetDistFromSmiley)
        {
            distToSmiley = (transform.position - Vector3.zero).magnitude;
            yield return null;
        }

        // smoothly even out to perpendicular to player, circling
        while (turnAngle > 0.5f)
        {
            turnAngle = Mathf.Lerp(turnAngle, 0f, 0.01f);

            yield return null;
        }

        turnAngle = 0f;
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        // spawn lil enemies every 3 sec
        Straight enemy = Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation).AddComponent<Straight>();
        enemy.moveSpeed = moveSpeed;
        enemy.transform.localScale = new Vector3(transform.localScale.x * 0.6f, transform.localScale.y * 0.6f, 1f);
        enemy.deathParticle = deathParticle;
        enemy.ChangeColour("#D3FFCC");

        yield return new WaitForSeconds(3f);
        StartCoroutine(SpawnEnemy());
    }
}
