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

    float _pitchBGM = 1.0f;
    float _pitchEffect = 1.0f;
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

        AudioClip[] tmpBGM = Resources.LoadAll<AudioClip>("Sounds/BGM");
        AudioClip[] tmpEffect = Resources.LoadAll<AudioClip>("Sounds/Effect");
        for (int i = 0; i < tmpBGM.Length; i++)
        {
            _audioClipsBGM.Add(tmpBGM[i]);
        }
        for (int i = 0; i < tmpEffect.Length; i++)
        {
            _audioClipsEffect.Add(tmpEffect[i]);
        }

        AudioClip result = _audioClipsBGM.Find(x => x.name == "");
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
        //_audioClipsBGM.Clear();
    }

    AudioClip GetOrAddAudioClip(string filename, Sound type = Sound.Effect)
    {
        AudioClip audioClip = null;
        if (type == Sound.BGM)
        {

        }
        else
        {

        }
        //if (path.Contains("Sounds/") == false)
        //    path = $"Sounds/{path}"; // 📂Sound 폴더 안에 저장될 수 있도록

        /*
        if (type == Sound.BGM) // BGM 배경음악 클립 붙이기
        {
            audioClip = Resources.Load<AudioClip>(path);
        }
        else // Effect 효과음 클립 붙이기
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }
        */
        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {filename}");

        return audioClip;
    }
    public void Play(string fileName, Sound type = Sound.Effect)
    {
        AudioClip result = _audioClipsBGM.Find(x => x.name == fileName);
        if (null == result)
        {
            Debug.LogError(fileName + "이란 파일명이 존재하지 않습니다(" + type + ")");
            return;
        }

        if (type == Sound.BGM)
        {
            AudioSource audioSource = _audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = _pitchBGM;
            audioSource.clip = result;
            audioSource.Play();
        }
        else
        {

        }
    }
    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Sound.BGM) // BGM 배경음악 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // Effect 효과음 재생
        { 
            AudioSource audioSource = _audioSources[(int)Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    /*
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}"; // 📂Sound 폴더 안에 저장될 수 있도록

        AudioClip audioClip = null;

        if (type == Sound.Bgm) // BGM 배경음악 클립 붙이기
        {
            audioClip = Resource.Load<AudioClip>(path);
        }
        else // Effect 효과음 클립 붙이기
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }
    */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
