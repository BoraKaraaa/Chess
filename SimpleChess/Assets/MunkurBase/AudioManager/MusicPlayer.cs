using UnityEngine;
using Munkur;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private EMusic _musicToPlay;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic(_musicToPlay);
        }
    }
}
