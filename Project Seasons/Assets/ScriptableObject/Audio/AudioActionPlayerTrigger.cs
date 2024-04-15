using UnityEngine;

public class AudioActionPlayerTrigger : MonoBehaviour
{
    [Header("EVENTS")]

    public AudioActionPlayerSO onActionPlayer;

    [Header("BASE SETTINGS")]

    [SerializeField, Tooltip("")]
    string targetTag = "Player";

    [SerializeField, Tooltip("")]
    bool diableBackgroundMusic = false;

    [Header("PLAYLIST")]

    [SerializeField, Tooltip("")]
    AudioPlaylistSO actionPlaylist;

    [Header("VOLUME SETTINGS")]

    [SerializeField, Range(0.0f, 1.0f), Tooltip("")]
    float startVolume = 0.0f;

    [SerializeField, Range(0.0f, 1.0f), Tooltip("")]
    float targetVolume = 0.5f;

    [SerializeField, Tooltip("")]
    float volUpMultiplier = 0.25f;

    [SerializeField, Tooltip("")]
    float volDownMultiplier = 0.25f;

    bool startPlaying = false;

#if UNITY_EDITOR
    void OnEnable()
    {
        if (onActionPlayer == null)
        {
            Debug.LogError("ActionPlaylistTrigger: onActionPlayer (ScriptableObject) still NULL and can not be searched");
            Debug.Break();
        }
        
        bool tagExists = false;
        foreach (string tag in UnityEditorInternal.InternalEditorUtility.tags)
        {
            if (tag == targetTag)
            {
                tagExists = true;
                break;
            }
        }

        if (!tagExists)
        {
            Debug.LogError("ActionPlaylistTrigger: The target tag specified does not exist in the Unity tags list.");
            Debug.Break();
        }

        if (actionPlaylist == null)
        {
            Debug.LogError("ActionPlaylistTrigger: actionPlaylist (ScriptableObject) still NULL and can not be searched.");
            Debug.Break();
        }
    }
#endif

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            startPlaying = true;

            AudioPlaylistPlayerEventData eventData = new()
            {
                startVol = startVolume,
                targetVol = targetVolume,
                upVol = volUpMultiplier,
                downVol = volDownMultiplier,
                disableBackgroundMusic = diableBackgroundMusic,
                audioPlaylist = actionPlaylist,
                playSP = startPlaying
            };

            onActionPlayer.Raise(eventData);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            startPlaying = false;

            AudioPlaylistPlayerEventData eventData = new()
            {
                startVol = startVolume,
                targetVol = targetVolume,
                upVol = volUpMultiplier,
                downVol = volDownMultiplier,
                disableBackgroundMusic = false,
                audioPlaylist = null,
                playSP = startPlaying
            };

            onActionPlayer.Raise(eventData);
        }
    }
}
