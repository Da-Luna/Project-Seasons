using UnityEngine;

public class AudioSinglePlayerTrigger : MonoBehaviour
{
    [Header("EVENTS")]

    public AudioSinglePlayerSO onSinglePlayer;

    [Header("BASE SETTINGS")]

    [SerializeField, Tooltip("")]
    string targetTag = "Player";

    [SerializeField, Tooltip("")]
    bool reduceBackgroundMusic = false;

    [SerializeField, Tooltip("")]
    bool disableBackgroundMusic = false;

    [Header("SINGLE AUDIO CLIP")]

    [SerializeField, Tooltip("")]
    AudioClip singlePlayerAudioClip;

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
        if (onSinglePlayer == null)
        {
            Debug.LogError("AudioSinglePlayerTrigger: onSinglePlayer (ScriptableObject) still NULL and can not be searched");
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
            Debug.LogError("AudioSinglePlayerTrigger: The target tag specified does not exist in the Unity tags list.");
            Debug.Break();
        }

        if (singlePlayerAudioClip == null)
        {
            Debug.LogError("AudioSinglePlayerTrigger: singlePlayerAudioClip - no audio clip");
            Debug.Break();
        }
    }
#endif

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            startPlaying = true;

            AudioSinglePlayerEventData eventData = new ()
            {
                startVol = startVolume,
                targetVol = targetVolume,
                upVol = volUpMultiplier,
                downVol = volDownMultiplier,
                reduceBGMVolume = reduceBackgroundMusic,
                disableBGMVolume = disableBackgroundMusic,
                audioClip = singlePlayerAudioClip,
                playSP = startPlaying
            };

            onSinglePlayer.Raise(eventData);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            startPlaying = false;

            AudioSinglePlayerEventData eventData = new()
            {
                startVol = startVolume,
                targetVol = targetVolume,
                upVol = volUpMultiplier,
                downVol = volDownMultiplier,
                reduceBGMVolume = false,
                disableBGMVolume = false,
                audioClip = null,
                playSP = startPlaying
            };
            
            onSinglePlayer.Raise(eventData);
        }
    }
}
