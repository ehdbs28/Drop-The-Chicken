using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, IManager
{
    [SerializeField] private AudioMixer _masterMixer;

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
        
    }

    private void Init(){

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
