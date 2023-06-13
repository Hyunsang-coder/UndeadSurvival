using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;

    AudioSource bgmPlayer;
    AudioHighPassFilter audioFilter;

    [Header("SFX")]

    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx {Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win }

    private void Awake() {
        Instance = this;

        Init();
    }

    void Init()
    {
        // initialize BGM 
        GameObject bgmObject = new GameObject("BGMPalyer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        audioFilter = Camera.main.GetComponent<AudioHighPassFilter>();


        // initialize SFX
        GameObject sfxObject = new GameObject("SFXPalyer");
        sfxObject.transform.parent = transform;

        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index=0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;


            // 해당 index가 play중이라면 다음 index로 점프
            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            int randomIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                randomIndex = Random.Range(0,2);
            }

            // 비어 있는 audio source를 찾아서 저장 후 플레이 
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + randomIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }

    }

    public void PlayBGM(bool shouldPlay)
    {
        if (shouldPlay){
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void ApplyAudioFilter(bool shouldPlay)
    {
        audioFilter.enabled = shouldPlay;
    }

}
