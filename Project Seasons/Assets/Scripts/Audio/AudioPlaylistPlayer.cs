using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlaylistPlayer : MonoBehaviour
{
    #region AUDIO PLAYER SETTINGS

    [Header("AUDIO PLAYER SETTINGS")]
    [SerializeField, Tooltip("The AudioSource component responsible for playing background music.")]
    AudioSource audioPlayer;

    [SerializeField, Tooltip("Determines whether to play a random AudioClip from the playlist.")]
    bool playRandomMusic = false;

    [SerializeField, Tooltip("List of AudioClips to be played in sequence or randomly.")]
    AudioPlaylistSO audioPlaylist;
    int _clipCounter = -1;

    #endregion // AUDIO PLAYER SETTINGS

    void InitReferences()
    {
        if (audioPlayer == null)
        {
            audioPlayer = GetComponent<AudioSource>();
#if UNITY_EDITOR

            if (audioPlayer != null)
                Debug.Log("AudioPlaylistPlayer: audioPlayer is NULL, get component from GameObject");
            else if (audioPlayer == null)
            {
                Debug.LogError("AudioPlaylistPlayer: audioPlayer still NULL after try to get component from GameObject");
                Debug.Break();
            }
#endif
        }
#if UNITY_EDITOR
        if (audioPlayer.outputAudioMixerGroup == null)
            Debug.LogError($"AudioPlaylistPlayer: Audio Mixer Group of {audioPlayer.gameObject.name} is null");

        if (audioPlaylist == null)
        {
            Debug.LogError("AudioPlaylistPlayer: audioPlaylist (ScriptableObject) still NULL and can not be searched");
            Debug.Break();
        }
#endif
    }

    void OnEnable()
    {
        InitReferences();
    }

    public void PlayAudioPlayer()
    {
        if (audioPlayer.isPlaying) return;

        if (playRandomMusic)
        {
            var randomSelect = Random.Range(0, audioPlaylist.audioPlaylist.Length);
            audioPlayer.clip = audioPlaylist.audioPlaylist[randomSelect];
        }
        else audioPlayer.clip = GetNextClip();

        audioPlayer.Play();

#if UNITY_EDITOR
        Debug.Log($"New AudioClip from AudioBGMPlayer.cs STARTS. Title is: {audioPlayer.clip.name}");
#endif
    }
    AudioClip GetNextClip()
    {
        if (_clipCounter >= audioPlaylist.audioPlaylist.Length - 1)
            _clipCounter = 0;
        else
            _clipCounter++;

        return audioPlaylist.audioPlaylist[_clipCounter];
    }

    public void StopAudioPlayer()
    {
        audioPlayer.Stop();

#if UNITY_EDITOR
        Debug.Log($"AudioClip from AudioBGMPlayer.cs scripts STOPS. Title was: {audioPlayer.clip.name}");
#endif
    }

    public void SetActionPlayerClip(AudioPlaylistSO playlist)
    {
        if (audioPlayer.isPlaying) return;

        if (audioPlaylist != playlist)
        {
            audioPlaylist = playlist;
            _clipCounter = -1;
        }

        audioPlayer.clip = GetNextClip();
    }


    public void APSetVolume(float volume, float changeMultiplier)
    {
        if (audioPlayer.volume == volume) return;

        audioPlayer.volume = Mathf.MoveTowards(audioPlayer.volume, volume, changeMultiplier * Time.deltaTime);
    }

    public void SPSetActionPlayerVolume(float startVol)
    {
        if (audioPlayer.volume != startVol)
            audioPlayer.volume = startVol;
    }

    public bool APVolumeHasTargetValue(float tagerVolume)
    {
        if (audioPlayer.volume == tagerVolume)
            return true;

        return false;
    }
}
