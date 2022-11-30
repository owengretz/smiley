using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float rotationToCenter;

    public ParticleSystem deathParticle;
    [HideInInspector] public bool canDie = true;

    private void Awake()
    {
        canDie = true;

        // get direction to center
        Vector3 direction = Vector3.zero - transform.position;
        rotationToCenter = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotationToCenter -= 90f;

        // randomize size slightly
        float scale = Random.Range(0.8f, 1.2f);
        transform.localScale = new Vector3(scale, scale, 1f);

        // randomize move speed slightly
        moveSpeed += Random.Range(-1f, 1f);
    }

    private void Update()
    {
        // move towards center
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
    }



    private void OnMouseDown()
    {
        Die();
    }

    // public function so that TakeDamage can kill all enemies
    public void Die(bool fromTakingDamage = false)
    {
        if (!fromTakingDamage && !canDie)
            return;

        Destroy(gameObject);

        // only add score if this is from clicking (not from taking damage cls)
        if (!fromTakingDamage)
        {
            EventBroker.CallEnemyKilled();
            //EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
            //spawner.EnemyKilled();
        }

        // spawn particle effect & make it so that the little effect from clicking on nothing doesnt play
        Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
        TakeDamage.makeClickParticleThisFrame = false;
    }

    // set colour
    public void ChangeColour(string colour)
    {
        if (ColorUtility.TryParseHtmlString(colour, out Color newCol))
            gameObject.GetComponent<SpriteRenderer>().color = newCol;
        //gameObject.GetComponent<SpriteRenderer>().color = newCol;
    }
}
