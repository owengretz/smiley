using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUp : Enemy
{
    private readonly float turnSpeed = 360f;
    private readonly float distFromEdge = 1.4f;
    private readonly float launchSpeedMultiplier = 7f;
    private int spinsDone = 0;

    private void Start()
    {
        moveSpeed = 2f;
        ChangeColour("#ECD1FF");
        StartCoroutine(GetIntoPosition());
    }

    // empty update to override parent
    private void Update()
    {
        
    }

    private IEnumerator GetIntoPosition()
    {
        // just go straight until we're on screen
        while (ScreenBounds.OutOfBounds(transform.position))
        {
            Vector3 direction = Vector3.zero - transform.position;
            rotationToCenter = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rotationToCenter -= 90f;
            Quaternion rotation = Quaternion.AngleAxis(rotationToCenter, Vector3.forward);
            transform.rotation = rotation;
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);

            yield return null;
        }

        // keep going a lil bit more
        Vector3 startingPos = transform.position;
        while ((startingPos - transform.position).magnitude < distFromEdge)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);

            yield return null;
        }

        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        // spin in circle twice to show player ~ they're in danger! ~
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float targetRotation = rb.rotation + 360f;
        while (rb.rotation < targetRotation)
        {
            yield return new WaitForFixedUpdate();

            rb.SetRotation(rb.rotation + turnSpeed * Time.fixedDeltaTime);
        }
        spinsDone++;

        if (spinsDone < 2)
        {
            StartCoroutine(Spin());
        }
        else
        {
            StartCoroutine(Launch());
        }
    }

    // yeet at the player
    private IEnumerator Launch()
    {
        Vector3 direction = Vector3.zero - transform.position;
        rotationToCenter = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotationToCenter -= 90f;
        Quaternion rotation = Quaternion.AngleAxis(rotationToCenter, Vector3.forward);
        transform.rotation = rotation;

        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            transform.Translate(Vector2.up * moveSpeed * launchSpeedMultiplier * Time.deltaTime);

            yield return null;
        }
    }
}
