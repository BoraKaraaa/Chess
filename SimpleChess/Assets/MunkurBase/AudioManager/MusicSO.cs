using UnityEngine;
using Munkur;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New MusicSO", menuName = "MusicSO")]
public class MusicSO : ScriptableObject
{
    public AudioClip audioClip;
    public EMusic soundEffectName;
    
    [Range(0f, 1f)]
    public float volume;

    [Range(-3f, 3f)]
    public float pitch;

    public bool isLooping = false;

    public float delay = 0;
}
