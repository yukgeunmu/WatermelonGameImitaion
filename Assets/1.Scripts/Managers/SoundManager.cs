using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : IManager
{
    private AudioMixer mixer;

    private AudioSource bgmSource;

    private AudioSource sfxSource;


    private float masterVolume = 1f;

    private float bgmVolume = 1f;

    private float sfxVolume = 1f;

    public float MasterVolume => masterVolume;
    public float BGMVolume => bgmVolume;
    public float SFXVolume => sfxVolume;

    private readonly Dictionary<SoundType, AudioClip> clips = new();

    private GameObject soundRoot;

    private const string MASTER_VOLUME_KEY = "MASTER_VOLUME";

    private const string BGM_VOLUME_KEY = "BGM_VOLUME";

    private const string SFX_VOLUME_KEY = "SFX_VOLUME";

    public void Initialize()
    {
        CreateAudioSources();
        GameEventBus.Subscribe<FruitMergedEvent>(OnFruitMerged);

    }

    public void Dispose()
    {
        clips.Clear();

        if (soundRoot != null)
        {
            Object.Destroy(soundRoot);
            GameEventBus.Unsubscribe<FruitMergedEvent>(OnFruitMerged);
        }
    }

    public void SetStartSound()
    {
        SetAudioMixer();

        SetAudioClip();

        LoadVolume();
    }

    public void SetAudioClip()
    {
        List<SoundData> soundList = Game.Get<ResourceManager>().GetAllResource<SoundData, SoundType>();

        foreach (SoundData data in soundList)
        {
            AddClip(data.Key, data.Clip);
        }

        PlayBGM(SoundType.StartBGM);
    }

    public void StartGamSceneBGM()
    {
        PlayBGM(SoundType.MainBGM);
    }


    private void CreateAudioSources()
    {
        soundRoot = new GameObject("SoundManager");

        soundRoot.transform.SetParent(Manager.Instance.transform);

        bgmSource = soundRoot.AddComponent<AudioSource>();

        sfxSource = soundRoot.AddComponent<AudioSource>();


        bgmSource.loop = true;

        bgmSource.playOnAwake = false;

        sfxSource.playOnAwake = false;

    }

    public void SetAudioMixer()
    {
        mixer = Game.Get<ResourceManager>().GetAsset<AudioMixer>("Audio", "AudioMixer");

        AudioMixerGroup[] bgmGroups = mixer.FindMatchingGroups("BGM");
        AudioMixerGroup[] sfxGroups = mixer.FindMatchingGroups("SFX");

        // 3. 찾은 그룹이 존재한다면 각각의 AudioSource에 연결합니다.
        if (bgmGroups.Length > 0)
        {
            bgmSource.outputAudioMixerGroup = bgmGroups[0];
        }
        else
        {
            Debug.LogWarning("SoundManager: Mixer에서 'BGM' 그룹을 찾을 수 없습니다.");
        }

        if (sfxGroups.Length > 0)
        {
            sfxSource.outputAudioMixerGroup = sfxGroups[0];
        }
        else
        {
            Debug.LogWarning("SoundManager: Mixer에서 'SFX' array를 찾을 수 없습니다.");
        }

    }

    public void AddClip(SoundType key, AudioClip clip)
    {
        if (clips.ContainsKey(key))
            return;

        clips.Add(key, clip);
    }

    public AudioClip GetClip(SoundType key)
    {
        if (clips.TryGetValue(key, out AudioClip clip))
        {
            return clip;
        }

        Debug.LogWarning($"AudioClip Not Found : {key}");

        return null;
    }

    public void PlayBGM(SoundType key, float volume = 1f)
    {
        AudioClip clip = GetClip(key);

        if (clip == null)
            return;

        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }


    public void PlaySFX(SoundType key, float volume = 1f)
    {
        AudioClip clip = GetClip(key);

        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip, volume);
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("Master", ConvertToDecibel(volume));

        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);

        PlayerPrefs.Save();
    }

    public void SetBGMVolume(float volume)
    {
        mixer.SetFloat("BGM", ConvertToDecibel(volume));

        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);

        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFX", ConvertToDecibel(volume));

        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);

        PlayerPrefs.Save();
    }


    private float ConvertToDecibel(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }

    private void OnFruitMerged(FruitMergedEvent evt)
    {
        PlaySFX(SoundType.MergeSFX);
    }


    private void LoadVolume()
    {
        float master = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);

        float bgm = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);

        float sfx = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

        ApplyVolume(master, bgm, sfx);
    }

    public void ApplyVolume(float master, float bgm, float sfx)
    {
        mixer.SetFloat("Master", Mathf.Log10(master) * 20f);

        mixer.SetFloat("BGM", Mathf.Log10(bgm) * 20f);

        mixer.SetFloat("SFX", Mathf.Log10(sfx) * 20f);
    }

    public float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
    }

    public float GetBGMVolume()
    {
        return PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
    }

    public void ResetVolume()
    {
        SetMasterVolume(1f);
        SetBGMVolume(1f);
        SetSFXVolume(1f);
    }
}