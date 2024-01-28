using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _bgmSource = null;
    [SerializeField]
    private AudioSource _soundSource = null;
    public static SoundManager _Instance { get; private set; }
    private Dictionary<string, AudioClip> _loadedClip = new Dictionary<string, AudioClip>();

    private float _bgmVolume;
    private float _soundVolume;
    private void Start()
    {   //게임 시작할때 사용자가 설정해 놓은 설정에 따라서 배경,효과음 실행
        _bgmSource.volume = float.Parse(PlayerPrefs.GetString("bgmVolume"));
        _soundSource.volume = float.Parse(PlayerPrefs.GetString("soundVolume"));
    }
    public static void Create()
    {
        if (_Instance != null)
            return; // 이미 있는 상태로 생성시도해도 오류 아님

        SoundManager prefab = Resources.Load<SoundManager>(Define._soundManagerPrefabPath);
        if (prefab == null)
        {
            Debug.LogError($"[SoundManager.Create - Invalid preab path]{Define._soundManagerPrefabPath}");
            return;
        }

        _Instance = GameObject.Instantiate<SoundManager>(prefab);
        
        DontDestroyOnLoad(_Instance.gameObject);
    }

    public static void Clear()
    {
        if (_Instance != null)
            GameObject.Destroy(_Instance);

        _Instance = null;
    }

    private AudioClip LoadAudioClip(string fullPath)
    {
        AudioClip clip = null;
        if (_loadedClip.TryGetValue(fullPath, out clip))
            return clip;

        clip = Resources.Load<AudioClip>(fullPath);
        if (clip == null)
        {
            Debug.LogError($"[SoundManager.LoadAudioClip.InvalidPath]{fullPath}");
            return null;
        }

        _loadedClip.Add(fullPath, clip);
        return clip;
    }

    private static string GetBGMFullPath(string path) => Define._bgmRoot + "/" + path;

    public void LoadBGM(string path) => LoadAudioClip(GetBGMFullPath(path));

    public void PlayBGM(string path)
    {
        AudioClip clip = LoadAudioClip(GetBGMFullPath(path));
        if (clip == null)
            return;
        
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void StopBGM() => _bgmSource.Stop();

    private static string GetSoundFullPath(string path) => Define._soundRoot + "/" + path;

    public void LoadSound(string path) => LoadAudioClip(GetSoundFullPath(path));

    public void PlaySound(string path)
    {
        AudioClip clip = LoadAudioClip(GetSoundFullPath(path));
        if (clip == null)
            return;
        _soundSource.PlayOneShot(clip);
    }
   
    public void SetSoundVolume(float volume)
    {   //효과음 audiosource의 volume조절
        _soundVolume = volume;
        _soundSource.volume = _soundVolume;
    }
    public void SetBgmVolume(float volume)
    {   //배경음 audiosource의 volume 조절
        _bgmVolume = volume;
        _bgmSource.volume = _bgmVolume;
    }

    public void ClearLoadedAudioClip() => _loadedClip.Clear();
}