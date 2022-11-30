using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : Enemy
{
    [HideInInspector] public int dir;
    [HideInInspector] public float turnAngle;

    private void Start()
    {
        // make em move faster
        moveSpeed *= 2f;
        // dont collide with other enemies
        gameObject.layer = 9;
        // random circling direction
        dir = Random.Range(0, 2);
        if (dir == 0)
            dir = -1;

        turnAngle = Random.Range(5f, 20f);
        ChangeColour("#D1FDFF");
    }

    private void LateUpdate()
    {
        // get rotation to the center
        Vector3 vectorToCenter = Vector3.zero - transform.position;
        rotationToCenter = Mathf.Atan2(vectorToCenter.y, vectorToCenter.x) * Mathf.Rad2Deg;
        // compensate, couldnt get it to work doing other thing hahahahahh
        rotationToCenter -= 90f;
        // move at an angle a little less than 90
        rotationToCenter += (90 - turnAngle) * dir;

        Quaternion rotation = Quaternion.AngleAxis(rotationToCenter, Vector3.forward);
        transform.rotation = rotation;
    }
}
