using UnityEngine;
using Munkur;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Sound Effect", menuName = "SoundEffectSO")]
public class SoundEffectSO : ScriptableObject
{
    public AudioClip audioClip;
    [FormerlySerializedAs("audioName")] public ESoundEffect soundEffectName;

    [Range(0f, 1f)]
    public float volume;

    public bool isLooping;
}
