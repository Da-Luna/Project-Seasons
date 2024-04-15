using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class AudioSinglePlayerEventData
{
    public float startVol;
    public float targetVol;
    public float upVol;
    public float downVol;
    public bool reduceBGMVolume;
    public bool disableBGMVolume;
    public AudioClip audioClip;
    public bool playSP;
}

[Serializable]
public class CustomAudioSinglePlayerListener : UnityEvent<AudioSinglePlayerEventData> {}

public class AudioSinglePlayerListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    [SerializeField]
    AudioSinglePlayerSO audioSinglePlayer;

    [Tooltip("Response to invoke when Event with GameData is raised.")]
    [SerializeField]
    CustomAudioSinglePlayerListener response;

    private void OnEnable()
    {
        audioSinglePlayer.RegisterListener(this);
    }

    private void OnDisable()
    {
        audioSinglePlayer.UnregisterListener(this);
    }

    public void OnEventRaised(AudioSinglePlayerEventData eventData)
    {
        response.Invoke(eventData);
    }
}
