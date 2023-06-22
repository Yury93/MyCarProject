using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [System.Serializable]
    public class SoundLibrary
    {
        public AudioClip clickButton, deadPlayer, upgradePlayer, startKickPlayer, kickPlayer, rolling, levelUp, fire, burp, fart, kickTotarget,
            electro, batman,isGround,startJump,swordKick,hpUp,saw,deadEnemy1,deadEnemy2,deadEnemy3,deadEnemy4, lasers;
    }
    public static SoundSystem instance;
    
    public SoundLibrary soundLibrary;
    [SerializeField] private Sound soundPrefab;
    [SerializeField] private AudioSource backgroundAudioSource;
    
    //public AudioListener audioListener;
    public bool isAudioPlay;
    public bool isShowAdv;

    public void Init()
    {
        InitSingleton();
        
    }
    private void InitSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            isAudioPlay = true;
          
        }
        else
        {
            Destroy(gameObject);
        }
        Yandex.instance.onShowFullscreenAdv += OnShowFullScreenADV;
       
    }

    public void OnShowFullScreenADV(bool active)
    {
        if (active == true)
        {
            isShowAdv = true;
            SetActiveSound(false);
            Debug.Log("method STOP audio");
        }
        if (active == false)
        {
            isShowAdv = false;
            SetActiveSound(true);
            Debug.Log("method PLAY audio");
        }
    }
    private void Start()
    {
        Init();
    }
    public void SetActiveSound(bool active)
    {
        isAudioPlay = active;
        if(isAudioPlay == false || isShowAdv == true)
        {
            backgroundAudioSource.enabled = false;
            backgroundAudioSource.Stop();
         
           // if(audioListener)
           //audioListener.enabled = false;
        }
        else
        {
            if (isShowAdv == false)
            {
            
                backgroundAudioSource.enabled = true;
                backgroundAudioSource.Play();
                //if (audioListener)
                //    audioListener.enabled = true;

            }
        }
    }
    public void CreateSound(AudioClip audioClip)
    {
        if (isAudioPlay)
        {
            var sound = Instantiate(soundPrefab, transform);
            sound.PlaySound(audioClip);
        }

    }
    public void CreateSound(AudioClip audioClip, float startTime,float volume)
    {
        if (isAudioPlay)
        {
            var sound = Instantiate(soundPrefab, transform);

            sound.AudioSource.clip = audioClip;
            sound.AudioSource.volume = volume;
            int startSample = (int)(startTime * audioClip.frequency);
            sound.AudioSource.timeSamples = startSample;
            //audioSource.Play();

            sound.PlaySound(sound.AudioSource.clip);
        }

    }
    public void CreateSound(AudioClip audioClip, float startTime)
    {
        if (isAudioPlay)
        {
            var sound = Instantiate(soundPrefab, transform);

            sound.AudioSource.clip = audioClip;
            int startSample = (int)(startTime * audioClip.frequency);
            sound.AudioSource.timeSamples = startSample;
            //audioSource.Play();

            sound.PlaySound(sound.AudioSource.clip);
        }

    }
    public void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            SetActiveSound(false);
        }
        else
        {
  
            SetActiveSound(true);
        }
    }
    public void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            SetActiveSound(false);
        }
        else
        {
            SetActiveSound(true);
        } 
    }
   
}
