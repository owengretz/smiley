using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeHit : Enemy
{
    private int lives = 3;
    private readonly float scaleModifier = 1.45f;
    private readonly float scaleChangeOnHit = 0.75f;
    private readonly float speedModifier = 0.8f;

    private void Start()
    {
        canDie = false;

        transform.localScale = new Vector3(transform.localScale.x * scaleModifier, transform.localScale.y * scaleModifier, 1f);
        moveSpeed *= speedModifier;

        ChangeColour("#FFD2BB");
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

    // instead of dying, we get smaller until 3 clicks
    private void OnMouseDown()
    {
        Die();
        transform.localScale = new Vector3(transform.localScale.x * scaleChangeOnHit, transform.localScale.y * scaleChangeOnHit, 1f);
        lives--;
        if (lives == 1)
        {
            canDie = true;
        }
    }
}
