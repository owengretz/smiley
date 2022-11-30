using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [Header("particle systems")]
    public ParticleSystem bigExplosionParticle;
    public static bool makeClickParticleThisFrame;
    public ParticleSystem clickParticle;

    [Header("animations")]
    public Sprite[] faceSprites;
    private SpriteRenderer sRend;
    public Animator anim;

    private GameManager gameManager;
    private EnemySpawner spawner;
    private int damageTaken;
    //public static bool justTookDamage;

    private void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
        sRend.sprite = faceSprites[0];
        spawner = FindObjectOfType<EnemySpawner>();
        gameManager = FindObjectOfType<GameManager>();

        // disable animator after face fades in so that the face can change later when you take damage prolly
        Invoke("DisableAnim", 3f);
    }

    private void DisableAnim()
    {
        anim.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //justTookDamage = true;

        // when taking damage find all bad guys and blow em up
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            // passing true as param makes it so these dont give the player points
            enemy.GetComponent<Enemy>().Die(true);
        }
        // make biiiiig explosion in the middle
        Instantiate(bigExplosionParticle, transform.position, bigExplosionParticle.transform.rotation);

        damageTaken++;
        // reduce enemy spawn interval & move speed, & make it reduce more the better the player is doing
        // this makes it so that each life has a big impact as opposed to basically losing all your lives at once
        spawner.spawnInterval += ((1 - spawner.spawnInterval) / 2f);
        spawner.averageMoveSpeed -= spawner.averageMoveSpeed / 6f;

        // if we havent lost just change the face to less happy :(
        if (damageTaken < 3)
        {
            sRend.sprite = faceSprites[damageTaken];
        }
        else
        {
            gameManager.GameOver();
            anim.enabled = true;
            anim.Play("fade away");
        }

        //Invoke("SetJustTookDamageFalse", 3f);
    }
    //private void SetJustTookDamageFalse()
    //{
    //    justTookDamage = false;
    //}

    // make a little particle effect when you click, but dont play it if you kill an enemy since it already plays the death effect
    // make click particle is static & is set to false in DestroyOnMouseClick
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && makeClickParticleThisFrame)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;
            Instantiate(clickParticle, worldPosition, clickParticle.transform.rotation);
        }
        makeClickParticleThisFrame = true;
    }
}
