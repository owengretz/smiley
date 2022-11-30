using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sinusoidal : Enemy
{
    [HideInInspector] public float turnAngleLimit = 30f;
    [HideInInspector] public float turnSpeed = 2f;
    [HideInInspector] public float currentTurnAmount = 0;

    private void Start()
    {
        currentTurnAmount = rotationToCenter;

        // to make it vary more so they never look like theyre curving in a similar way
        int dir = Random.Range(0, 2);
        if (dir == 0)
            dir = -1;
        // amplitude & period
        turnAngleLimit += Random.Range(-10f, 10f);
        turnAngleLimit *= dir;
        turnSpeed += Random.Range(-1f, 1f);

        ChangeColour("#BFBFBF");
    }

    private void LateUpdate()
    {
        // get direction to center
        Vector3 direction = Vector3.zero - transform.position;
        rotationToCenter = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotationToCenter -= 90f;

        currentTurnAmount = turnAngleLimit * Mathf.Sin(Time.time * turnSpeed) + rotationToCenter;
        Quaternion rotation = Quaternion.AngleAxis(currentTurnAmount, Vector3.forward);
        transform.rotation = rotation;
    }
}
