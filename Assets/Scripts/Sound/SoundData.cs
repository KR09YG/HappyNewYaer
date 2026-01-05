using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundType")]
public class SoundData : ScriptableObject
{
    public SoundType SoundType;
    public AudioClip AudioClip;
    [Range(0f, 1f)] public float Volume = 1f;
    public bool Loop = false;
}
