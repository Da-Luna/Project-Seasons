using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("SCRIPT REFERENCES")]

    [SerializeField, Tooltip("")]
    AudioPlaylistPlayer audioBGMPlayer;
    
    [SerializeField, Tooltip("")]
    AudioSinglePlayer audioSinglePlayer;

    [SerializeField, Tooltip("")]
    AudioPlaylistPlayer audioActionPlayer;

    #region BACKGROUND MUSIC VOLUME SETTINGS

    [Header("BASE VOLUME SETTING")]

    [SerializeField, Tooltip("")]
    float bGMTargetVolume = 0.5f;

    [SerializeField, Tooltip("")]
    float bGMVolUpMultiplier = 0.25f;

    [SerializeField, Tooltip("")]
    float bGMVolDownMultiplier = 0.25f;

    [Header("VOLUME SETTING BY SINGLE PLAYER")]

    [SerializeField, Tooltip("")]
    float bGMTargetVolumeBySingle = 0.5f;

    #endregion // BACKGROUND MUSIC VOLUME SETTINGS

    #region EVENT VOLUME SETTINGS

    // SINLGE PLAYER VOLUME SETTINGS
    float sPStartVolume = 0.0f;
    float sPTargetVolume = 0.5f;
    float sPVolUpMultiplier = 0.25f;
    float sPVolDownMultiplier = 0.25f;

    bool startSinglePlayer = false;
    bool hasStartSinglePlayer = false;
    bool singlePlayerReduceBGM = false;
    bool singlePlayerDisableBGM = false;

    // ACTION PLAYER VOLUME SETTINGS
    float aPStartVolume = 0.0f;
    float aPTargetVolume = 0.5f;
    float aPVolUpMultiplier = 0.25f;
    float aPVolDownMultiplier = 0.25f;

    bool startActionPlayer = false;
    bool actionPlayerDisableBGM = false;

    #endregion // VOLUME SETTINGS

    void InitScriptReferences()
    {
        if (audioBGMPlayer == null)
        {
            audioBGMPlayer = transform.Find("AudioBGMPlayer").GetComponent<AudioPlaylistPlayer>();
#if UNITY_EDITOR
            if (audioBGMPlayer != null)
                Debug.Log("AudioManager: audioBGMPlayer is NULL, local search with name **AudioBGMPlayer** function was used");
            else if (audioBGMPlayer == null)
            {
                Debug.LogError("AudioManager: audioBGMPlayer still NULL, after local search with name **AudioBGMPlayer**");
                Debug.Break();
            }
#endif
        }

        if (audioSinglePlayer == null)
        {
            audioSinglePlayer = transform.Find("AudioSinglePlayer").GetComponent<AudioSinglePlayer>();
#if UNITY_EDITOR
            if (audioSinglePlayer != null)
                Debug.Log("AudioManager: audioSinglePlayer is NULL, local search with name **AudioSinglePlayer** function was used");
            else if (audioSinglePlayer == null)
            {
                Debug.LogError("AudioManager: audioSinglePlayer still NULL, after local search with name **AudioSinglePlayer**");
                Debug.Break();
            }
#endif
        }

        if (audioActionPlayer == null)
        {
            audioActionPlayer = transform.Find("AudioActionPlayer").GetComponent<AudioPlaylistPlayer>();
#if UNITY_EDITOR
            if (audioActionPlayer != null)
                Debug.Log("AudioManager: audioActionPlayer is NULL, local search with name **AudioActionPlayer** function was used");
            else if (audioActionPlayer == null)
            {
                Debug.LogError("AudioManager: audioActionPlayer still NULL, after local search with name **AudioActionPlayer**");
                Debug.Break();
            }
#endif
        }
    }

    void OnEnable()
    {
        InitScriptReferences();
    }

    void HandleBGMPlayer()
    {
        if (singlePlayerReduceBGM)
        {
            if (!audioBGMPlayer.APVolumeHasTargetValue(bGMTargetVolumeBySingle))
            {
                audioBGMPlayer.APSetVolume(bGMTargetVolumeBySingle, bGMVolDownMultiplier);
            }

            audioBGMPlayer.PlayAudioPlayer();
        }
        else if(!singlePlayerDisableBGM && !actionPlayerDisableBGM)
        {
            if (!audioBGMPlayer.APVolumeHasTargetValue(bGMTargetVolume))
            {
                audioBGMPlayer.APSetVolume(bGMTargetVolume, bGMVolUpMultiplier);
            }

            audioBGMPlayer.PlayAudioPlayer();
        }
        else if (!audioBGMPlayer.APVolumeHasTargetValue(0))
        {
            audioBGMPlayer.APSetVolume(0f, bGMVolDownMultiplier);

            if (audioBGMPlayer.APVolumeHasTargetValue(0))
            {
                audioBGMPlayer.StopAudioPlayer();
            }
        }
    }

    public void InitSinglePlayer(AudioSinglePlayerEventData eventData)
    {
        sPStartVolume = eventData.startVol;
        sPTargetVolume = eventData.targetVol;
        sPVolUpMultiplier = eventData.upVol;
        sPVolDownMultiplier = eventData.downVol;

        singlePlayerReduceBGM = eventData.reduceBGMVolume;
        singlePlayerDisableBGM = eventData.disableBGMVolume;

        startSinglePlayer = eventData.playSP;

        if (!startSinglePlayer) return;

        audioSinglePlayer.SetSinglePlayerClip(eventData.audioClip);
        audioSinglePlayer.SPSetStartVolume(sPStartVolume);

        hasStartSinglePlayer = false;
    }
    void HandleSinglePlayer()
    {
        if (startSinglePlayer)
        {
            if (!audioSinglePlayer.SPVolumeHasTargetValue(sPTargetVolume))
            {
                audioSinglePlayer.SPSetVolume(sPTargetVolume, sPVolUpMultiplier);
            }

            if (!hasStartSinglePlayer)
                audioSinglePlayer.PlaySinglePlayer();
            
            hasStartSinglePlayer = true;
        }
        else if (!audioSinglePlayer.SPVolumeHasTargetValue(0))
        {
            audioSinglePlayer.SPSetVolume(0, sPVolDownMultiplier);

            if (audioSinglePlayer.SPVolumeHasTargetValue(0))
            {
                audioSinglePlayer.StopSinglePlayer();
            }
        }
    }

    public void InitActionPlayer(AudioPlaylistPlayerEventData eventData)
    {
        aPStartVolume = eventData.startVol;
        aPTargetVolume = eventData.targetVol;
        aPVolUpMultiplier = eventData.upVol;
        aPVolDownMultiplier = eventData.downVol;

        actionPlayerDisableBGM = eventData.disableBackgroundMusic;
        startActionPlayer = eventData.playSP;

        if (!startActionPlayer) return;

        audioActionPlayer.SetActionPlayerClip(eventData.audioPlaylist);
        audioActionPlayer.SPSetActionPlayerVolume(aPStartVolume);
    }
    void HandleActionPlayer()
    {
        if (startActionPlayer)
        {
            if (!audioActionPlayer.APVolumeHasTargetValue(aPTargetVolume))
            {
                audioActionPlayer.APSetVolume(aPTargetVolume, aPVolUpMultiplier);
            }

            audioActionPlayer.PlayAudioPlayer();
        }
        else if (!audioActionPlayer.APVolumeHasTargetValue(0))
        {
            audioActionPlayer.APSetVolume(0, aPVolDownMultiplier);

            if (audioActionPlayer.APVolumeHasTargetValue(0))
            {
                audioActionPlayer.StopAudioPlayer();
            }
        }
    }

    void Update()
    {
        HandleBGMPlayer();
        HandleSinglePlayer();
        HandleActionPlayer();
    }
}
