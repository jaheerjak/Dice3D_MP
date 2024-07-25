using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioManager Instance;
    [SerializeField] AudioSource bgmAudio;
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip diceSound;
    [SerializeField] AudioClip diceSound1;
    public AudioSource audioSource;
   
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartBGM()
    {
        if(!bgmAudio.isPlaying)
            bgmAudio.Play();
        SetBgmVolume(CommonData.musicVolume);
    }
    public void StopBGM()
    {
        bgmAudio.Stop();
    }
    public void SetBgmVolume(float vol)
    {
        bgmAudio.volume = vol;
    }
    public void PlayClickSound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(clickSound, CommonData.soundVolume);
    }
    public void PlayDiceSound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(diceSound, CommonData.soundVolume);
    } 
    public void PlayDiceSound1()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(diceSound1, CommonData.soundVolume);
    }
}
