using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RussianDoll : Enemy
{
    [HideInInspector] public GameObject enemyPrefab;
    private readonly float scaleModifier = 1.6f;
    private readonly float speedModifier = 0.5f;

    private void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x * scaleModifier, transform.localScale.y * scaleModifier, 1f);
        moveSpeed *= speedModifier;
        ChangeColour("#FFB1B9");
    }

    private void LateUpdate()
    {
        // get direction to center
        Vector3 direction = Vector3.zero - transform.position;
        rotationToCenter = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotationToCenter -= 90f;
        Quaternion rotation = Quaternion.AngleAxis(rotationToCenter, Vector3.forward);
        transform.rotation = rotation;
    }

    private void OnMouseDown()
    {
        Die();
        for (int i = 0; i < 2; i++)
        {
            Straight enemy = Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation).AddComponent<Straight>();
            enemy.moveSpeed = moveSpeed / speedModifier;
            enemy.deathParticle = deathParticle;
            enemy.ChangeColour("#FFB1B9");
        }
    }
}
