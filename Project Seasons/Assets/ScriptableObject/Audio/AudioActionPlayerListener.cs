using UnityEngine;
using UnityEngine.Events;
using System;

public class AudioPlaylistPlayerEventData
{
    public float startVol;
    public float targetVol;
    public float upVol;
    public float downVol;
    public bool disableBackgroundMusic;
    public AudioPlaylistSO audioPlaylist;
    public bool playSP;
}

[Serializable]
public class CustomActionPlaylistListener : UnityEvent<AudioPlaylistPlayerEventData> {}

public class AudioActionPlayerListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    [SerializeField]
    AudioActionPlayerSO actionPlayer;

    [Tooltip("Response to invoke when Event with GameData is raised.")]
    [SerializeField]
    CustomActionPlaylistListener response;

    private void OnEnable()
	{
        actionPlayer.RegisterListener(this);
    }

    private void OnDisable()
	{
        actionPlayer.UnregisterListener(this);
    }

    public void OnEventRaised(AudioPlaylistPlayerEventData eventData)
	{
        response.Invoke(eventData);
    }
}
