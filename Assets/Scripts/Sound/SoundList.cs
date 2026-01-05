using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "SoundList", menuName = "Scriptable Objects/SoundList")]
public class SoundList : ScriptableObject
{
    public List<SoundData> _sounds;
    public SoundData GetSound(SoundType type)
    {
        foreach (var sound in _sounds)
        {
            if (sound.SoundType == type)
                return sound;
        }

        return null;
    }
}
