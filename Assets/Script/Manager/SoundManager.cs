using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    protected SoundManager() { }
    public enum Sound
    {
        BGM,
        Effect,
        MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가. (0, 1, '2' 이렇게 2개) 
    }

    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];

    public List<AudioClip> _audioClipsBGM = new List<AudioClip>();
    public List<AudioClip> _audioClipsEffect = new List<AudioClip>();

    private float _bgmPitch = 1.0f;
    private float _effectPitch = 1.0f;
    public float getBGMPitch { get { return _bgmPitch; } set { _bgmPitch = value; _audioSources[(int)Sound.BGM].pitch = value; } }
    public float getEffectPitch { get { return _effectPitch; } set { _effectPitch = value; _audioSources[(int)Sound.Effect].pitch = value; } }

    public Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("SoundManager (Singleton)");
        if (root == null)
        {
            Debug.Log("SoundManager null.");
            return;
        }

        string[] soundNames = System.Enum.GetNames(typeof(Sound)); // "Bgm", "Effect"
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
        }

        _audioSources[(int)Sound.BGM].loop = true; // bgm 재생기는 무한 반복 재생

        AudioClip[] tmpAudioClipBGM = Resources.LoadAll<AudioClip>("Sounds/BGM");
        AudioClip[] tmpAudioClipEffect = Resources.LoadAll<AudioClip>("Sounds/Effect");
        for (int i = 0; i < tmpAudioClipBGM.Length; i++)
        {
            _audioClipsBGM.Add(tmpAudioClipBGM[i]);
            Debug.Log("audio : " + tmpAudioClipBGM[i].name);
        }
        for (int i = 0; i < tmpAudioClipEffect.Length; i++)
        {
            _audioClipsEffect.Add(tmpAudioClipEffect[i]);
            Debug.Log("audio : " + tmpAudioClipEffect[i].name);
        }

        //Play("LobbyBG", Sound.BGM);
    }

    public void Clear()
    {
        // 재생기 전부 재생 스탑, 음반 빼기
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // 효과음 Dictionary 비우기
        _audioClips.Clear();
    }
    public void Play(string filename, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        if (type == Sound.BGM) // BGM 배경음악 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            AudioClip tmpClip = _audioClipsBGM.Find(x => x.name == filename);
            if(tmpClip)
            {
                audioSource.pitch = _bgmPitch;
                audioSource.clip = tmpClip;
                audioSource.Play();
            }
            else
            {
                Debug.Log(filename + "이란 파일은 BGM 폴더에 없습니다.");
            }            
        }
        else // Effect 효과음 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.Effect];
            AudioClip tmpClip = _audioClipsEffect.Find(x => x.name == filename);
            if (tmpClip)
            {
                audioSource.pitch = _effectPitch;
                audioSource.clip = tmpClip;
                audioSource.PlayOneShot(tmpClip);
            }
            else
            {
                Debug.Log(filename + "이란 파일은 Effect 폴더에 없습니다.");
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
