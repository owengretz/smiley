using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    private Image image;
    public Sprite on;
    public Sprite off;

    private AudioManager audioManager;
    private AudioSource source;

    private void Start()
    {
        audioManager = AudioManager.instance;
        source = audioManager.gameObject.GetComponent<AudioSource>();
        image = GetComponent<Image>();

        if (source.isPlaying)
        {
            image.sprite = on;
        }
    }

    public void ToggleAudio()
    {
        if (source.volume == 0f)
        {
            audioManager.isMuted = false;
            audioManager.FadeIn();
            image.sprite = on;
        }
        else
        {
            audioManager.isMuted = true;
            audioManager.FadeOut();
            image.sprite = off;
        }
    }
}
