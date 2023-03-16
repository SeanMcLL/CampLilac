using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioClip campLilacTheme;
    public AudioClip battleTheme;

    public AudioSource sfxSource;
    public AudioClip hitSfx;
    public AudioClip pointerSfx;
    public AudioClip selectSfx;
    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = campLilacTheme;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
