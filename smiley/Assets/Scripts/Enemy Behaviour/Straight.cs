using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straight : Enemy
{
    private void LateUpdate()
    {
        // get direction to center
        Vector3 direction = Vector3.zero - transform.position;
        rotationToCenter = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotationToCenter -= 90f;
        Quaternion rotation = Quaternion.AngleAxis(rotationToCenter, Vector3.forward);
        transform.rotation = rotation;
    }
}
