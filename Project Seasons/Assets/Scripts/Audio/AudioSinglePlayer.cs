using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSinglePlayer : MonoBehaviour
{
    #region ACTION PLAYER SETTINGS

    [Header("ACTION PLAYER SETTINGS")]

    [SerializeField, Tooltip("")]
    AudioSource singleAudioPlayer;

    [SerializeField, Tooltip("List of AudioClips to be played in sequence or randomly.")]
    AudioClip singleAudioClip;

    #endregion

    void InitReferences()
    {
        if (singleAudioPlayer == null)
        {
            singleAudioPlayer = GetComponent<AudioSource>();
#if UNITY_EDITOR

            if (singleAudioPlayer != null)
                Debug.Log("AudioSinglePlayer: singleAudioPlayer is NULL, get component from GameObject");
            else if (singleAudioPlayer == null)
            {
                Debug.LogError("AudioSinglePlayer: singleAudioPlayer still NULL after try to get component from GameObject");
                Debug.Break();
            }
#endif
        }
#if UNITY_EDITOR
        if (singleAudioPlayer.outputAudioMixerGroup == null)
            Debug.LogError($"AudioSinglePlayer: Audio Mixer Group of {singleAudioPlayer.gameObject.name} is null");
#endif
    }

    void OnEnable()
    {
        InitReferences();
    }

    public void PlaySinglePlayer()
    {
        if (singleAudioPlayer.isPlaying) return;

        singleAudioPlayer.clip = singleAudioClip;
        singleAudioPlayer.Play();

#if UNITY_EDITOR
        Debug.Log($"New AudioClip from AudioSinglePlayer.cs scripts STARTS. Title is: {singleAudioPlayer.clip.name}");
#endif
    }
    public void StopSinglePlayer()
    {
        singleAudioPlayer.Stop();

#if UNITY_EDITOR
        Debug.Log($"AudioClip from AudioSinglePlayer.cs scripts STOPS. Title was: {singleAudioPlayer.clip.name}");
#endif
    }

    public void SetSinglePlayerClip(AudioClip audioClip)
    {
        if (singleAudioClip != audioClip)
            singleAudioClip = audioClip;
    }

    public void SPSetVolume(float tagerVolume, float volumeChangeMultiplier)
    {
        if (singleAudioPlayer.volume == tagerVolume) return;

        singleAudioPlayer.volume = Mathf.MoveTowards(singleAudioPlayer.volume, tagerVolume, volumeChangeMultiplier * Time.deltaTime);
    }
    public void SPSetStartVolume(float startVol)
    {
        if (singleAudioPlayer.volume != startVol)
            singleAudioPlayer.volume = startVol;
    }

    public bool SPVolumeHasTargetValue(float tagerVolume)
    {
        if (singleAudioPlayer.volume == tagerVolume)
            return true;

        return false;
    }
}
