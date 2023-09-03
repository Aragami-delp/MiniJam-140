using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> audioClips; 
    [SerializeField]
    private List<float> minSpeedList;

    private AudioSource audioSource;

    [SerializeField]
    private float volume = 1f;
    
    private void Awake()
    {
        GameObject[] musicObject = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObject.Length > 1)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject);
        AdjustVolume(volume);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = FindOutAudioToPlay();
            audioSource.Play();
        }
    }

    private AudioClip FindOutAudioToPlay()
    {
        float currentSpeed = Scrolling.Instance.GetSpeed();
        AudioClip selectedClip = null;

        for (int i = 0; i < audioClips.Count; i++)
        {
            var minSpeed = minSpeedList[i];
            if (currentSpeed >= minSpeed)
            {
                selectedClip = audioClips[i];
            }
            else
            {
                break; // The dictionary is assumed to be sorted by speed.
            }
        }

        return selectedClip;
    }
    
    public void AdjustVolume (float newVolume) {
        AudioListener.volume = newVolume;
        volume = newVolume;
    }
}
