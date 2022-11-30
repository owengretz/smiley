using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IEndGameObserver, IRestartGameObserver
{
    public static AudioManager instance;
    private GameManager gameManager;
    private AudioSource source;
    public AudioClip gameMusic;
    public AudioClip deathMusic;

    private bool changingVolume;
    [HideInInspector] public bool isMuted;
    //private readonly float fadeRate = 0.4f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            gameManager = FindObjectOfType<GameManager>();
            gameManager.AddEndGameObserver(this);
            gameManager.AddRestartGameObserver(this);
        }
        else
        {
            Destroy(gameObject);
        }

        source = GetComponent<AudioSource>();
        //source.Pause();
        
    }

    public void FadeIn()
    {
        if (!changingVolume)
            StartCoroutine(FadeInCoroutine());
    }
    private IEnumerator FadeInCoroutine(float rate = 3f)
    {
        changingVolume = true;
        if (!isMuted)
        {
            source.UnPause();
            while (source.volume < 1f)
            {
                source.volume += Time.deltaTime * rate;
                yield return null;
            }
            source.volume = 1f;
        }
        changingVolume = false;
    }

    public void FadeOut()
    {
        if (!changingVolume)
            StartCoroutine(FadeOutCoroutine());
    }
    private IEnumerator FadeOutCoroutine(float rate = 3f)
    {
        changingVolume = true;
        while (source.volume > 0f)
        {
            source.volume -= Time.deltaTime * rate;
            yield return null;
        }
        source.volume = 0f;
        source.Pause();
        changingVolume = false;
    }

    public void EndGameNotify()
    {
        StartCoroutine(SwapMusic(deathMusic, 0.29f, 0.5f));
    }

    public void RestartGameNotify()
    {
        StartCoroutine(SwapMusic(gameMusic, 0.45f, 1f));
    }

    private IEnumerator SwapMusic(AudioClip clip, float fadeRate, float pause)
    {
        yield return StartCoroutine(FadeOutCoroutine(fadeRate));

        yield return new WaitForSeconds(pause);

        source.clip = clip;
        source.Play();
        if (isMuted)
            source.Pause();
        yield return StartCoroutine(FadeInCoroutine());


        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.AddEndGameObserver(this);
            gameManager.AddRestartGameObserver(this);
        }
    }
}
