using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource moveAudioSource;
    [SerializeField] private AudioSource captureAudioSource;
    [SerializeField] private AudioSource checkMateAudioSource;
    [SerializeField] private AudioSource promotionAudioSource;
    
    public AudioSource MoveAudioSource => moveAudioSource;
    public AudioSource CaptureAudioSource => captureAudioSource;
    public AudioSource CheckMateAudioSource => checkMateAudioSource;
    public AudioSource PromotionAudioSource => promotionAudioSource;
}
