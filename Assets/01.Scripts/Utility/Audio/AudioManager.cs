using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, IManager
{
    [SerializeField] private AudioMixer _masterMixer;

    private AudioSource _bgmSource;
    private AudioSource _sfxSource;

    public bool IsMuteBGM {
        get{
            _masterMixer.GetFloat("BGM", out float bgmVolume);
            return bgmVolume == -40;
        }
    }

    public bool IsMuteSFX {
        get{
            _masterMixer.GetFloat("SFX", out float sfxVolume);
            return sfxVolume == -40;
        }
    }

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                break;
        }
    }

    public void PlayBGM(AudioClip clip){

    }

    public void PlayOneShot(AudioClip clip){
        
    }

    private void Init(){
        //_bgmSource = transform.Find("AudioSource_BGM").GetComponent<AudioSource>();
        //_sfxSource = transform.Find("AudioSource_SFX").GetComponent<AudioSource>();
    }

    public void AudioMute(AudioType type, bool mute){
        switch(type){
            case AudioType.BGM:
                _masterMixer.SetFloat("BGM", mute ? -40 : 0);
                break;
            case AudioType.SFX:
                _masterMixer.SetFloat("SFX", mute ? -40 : 0);
                break;
        }
    }
}
