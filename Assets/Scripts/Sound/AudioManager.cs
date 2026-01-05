using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _bgm;
    [SerializeField] private AudioSource _se;
    [SerializeField] private SoundList _soundList;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(SoundType type)
    {
        var soundData = _soundList.GetSound(type);
        if (soundData != null)
        {
            _bgm.clip = soundData.AudioClip;
            _bgm.loop = true;
            _bgm.Play();
        }
    }

    public void PlaySE(SoundType type)
    {
        var soundData = _soundList.GetSound(type);
        if (soundData != null)
        {
            _se.PlayOneShot(soundData.AudioClip);
        }
    }
}
