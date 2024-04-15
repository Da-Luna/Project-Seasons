using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Game SFX Settings")]
    [SerializeField]
    private AudioSource audioSourceGame;
    [SerializeField]
    private AudioClip appearGameAudio;
    [SerializeField, Range(0.0f, 1.0f)]
    private float appearGameAudioVol;
    [SerializeField]
    private AudioClip victoryGameAudio;
    [SerializeField, Range(0.0f, 1.0f)]
    private float victoryGameAudioVol;
    [SerializeField]
    private AudioClip loseGameAudio;
    [SerializeField, Range(0.0f, 1.0f)]
    private float loseGameAudioVol;

    [Header("Player Voice Settings")]
    [SerializeField]
    private AudioSource audioSourceVoice;
    [SerializeField]
    private AudioClip victoryVoiceAudio;
    [SerializeField, Range(0.0f, 1.0f)]
    private float victoryVoiceAudioVol;
    [SerializeField]
    private AudioClip loseVoiceAudio;
    [SerializeField, Range(0.0f, 1.0f)]
    private float loseVoiceAudioVol;


    [Header("Player Action Audio Sources")]
    [SerializeField]
    private AudioSource audioSourceJump;

    [Header("Footstep Audio Settings")]
    [SerializeField]
    private AudioSource audioSourceFootStep;
    [SerializeField]
    private float stepInterval = 0.3f;
    [SerializeField]
    private float stepMultipier = 0.8f;
    [SerializeField]
    private float stepSprintMultipier = 1.75f;
    [SerializeField]
    private AudioClip[] footstepSounds;

    private float t_StepCycle = 0.0f;
    private float t_NextStep;

    private void OnEnable()
    {
        if (audioSourceGame == null)
        {
            audioSourceGame = transform.Find("SoundHolder/Game").GetComponent<AudioSource>();
#if UNITY_EDITOR
            Debug.Log("PlayerAudioController: audioSourceGame is NULL, local search with name **SoundHolder/Game** function was used");
#endif
        }
#if UNITY_EDITOR
        if (audioSourceGame.outputAudioMixerGroup == null)
            Debug.LogError($"PlayerAudioController: Audio Mixer Groupon of {audioSourceGame.name} is null");
        if (audioSourceGame.playOnAwake)
            Debug.LogError($"PlayerAudioController: Play On Awake of {audioSourceGame.name} is active");
#endif
        audioSourceGame.clip = appearGameAudio;

        if (audioSourceVoice == null)
        {
            audioSourceVoice = transform.Find("SoundHolder/Voice").GetComponent<AudioSource>();
#if UNITY_EDITOR
            Debug.Log("PlayerAudioController: audioSourceVoice is NULL, local search with name **SoundHolder/Voice** function was used");
#endif
        }
#if UNITY_EDITOR
        if (audioSourceVoice.outputAudioMixerGroup == null)
            Debug.LogError($"PlayerAudioController: Audio Mixer Groupon of {audioSourceVoice.name} is null");
        if (audioSourceVoice.playOnAwake)
            Debug.LogError($"PlayerAudioController: Play On Awake of {audioSourceVoice.name} is active");
#endif


        if (audioSourceJump == null)
        {
            audioSourceJump = transform.Find("SoundHolder/Jump").GetComponent<AudioSource>();
#if UNITY_EDITOR
            Debug.Log("PlayerAudioController: audioSourceJump is NULL, local search with name **SoundHolder/Jump** function was used");
#endif
        }
#if UNITY_EDITOR
        if (audioSourceJump.outputAudioMixerGroup == null)
            Debug.LogError($"PlayerAudioController: Audio Mixer Groupon of {audioSourceJump.name} is null");
        if (audioSourceJump.playOnAwake)
            Debug.LogError($"PlayerAudioController: Play On Awake of {audioSourceJump.name} is active");
#endif

        if (audioSourceFootStep == null)
        {
            audioSourceFootStep = transform.Find("SoundHolder/Footsteps").GetComponent<AudioSource>();
#if UNITY_EDITOR
            Debug.Log("PlayerAudioController: audioSourceFootStep is NULL, local search with name **SoundHolder/Footsteps** function was used");
#endif
        }
#if UNITY_EDITOR
        if (audioSourceFootStep.outputAudioMixerGroup == null)
            Debug.LogError($"PlayerAudioController: Audio Mixer Groupon of {audioSourceFootStep.name} is null");
        if (audioSourceFootStep.playOnAwake)
            Debug.LogError($"PlayerAudioController: Play On Awake of {audioSourceFootStep.name} is active");
#endif
    }

    #region Game Audio Source
    public void PlayAppearGameSound()
    {
        if (audioSourceGame.isPlaying)
            audioSourceGame.Stop();

        audioSourceGame.volume = appearGameAudioVol;
        audioSourceGame.clip = appearGameAudio;

        if (!audioSourceGame.isPlaying)
            audioSourceGame.Play();
    }
    public void PlayVictoryGameSound()
    {
        if (audioSourceGame.isPlaying)
            audioSourceGame.Stop();

        audioSourceGame.volume = victoryGameAudioVol;
        audioSourceGame.clip = victoryGameAudio;

        if (!audioSourceGame.isPlaying)
            audioSourceGame.Play();
    }
    public void PlayVictoryVoiceSound()
    {
        if (audioSourceVoice.isPlaying)
            audioSourceVoice.Stop();

        audioSourceVoice.volume = victoryVoiceAudioVol;
        audioSourceVoice.clip = victoryVoiceAudio;

        if (!audioSourceVoice.isPlaying)
            audioSourceVoice.Play();
    }
    public void PlayLoseGameSound()
    {
        if (audioSourceGame.isPlaying)
            audioSourceGame.Stop();

        audioSourceGame.volume = loseGameAudioVol;
        audioSourceGame.clip = loseGameAudio;

        if (!audioSourceGame.isPlaying)
            audioSourceGame.Play();
    }
    public void PlayLoseVoiceSound()
    {
        if (audioSourceVoice.isPlaying)
            audioSourceVoice.Stop();

        audioSourceVoice.volume = loseVoiceAudioVol;
        audioSourceVoice.clip = loseVoiceAudio;

        if (!audioSourceVoice.isPlaying)
            audioSourceVoice.Play();
    }
    #endregion

    /// <summary>
    /// Plays a sound on the specified audio source with the given audio clip and volume.
    /// </summary>
    /// <param name="audioSource">The audio source to play the sound on.</param>
    /// <param name="audioClip">The audio clip to play.</param>
    /// <param name="volume">The volume of the audio.</param>
    private void PlaySound(AudioSource audioSource, AudioClip audioClip, float volume)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.volume = volume;
        audioSource.clip = audioClip;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }


    #region Player Actions Sounds
    public void PlayJumpSound()
    {        
        if (!audioSourceJump.isPlaying)
            audioSourceJump.Play();
    }

    public void ProgressStepCycle(float input, bool isSprint)
    {
        if (!isSprint && input != 0 )
            t_StepCycle += stepMultipier * Time.deltaTime;
        else if (isSprint && input != 0)
            t_StepCycle += stepSprintMultipier * Time.deltaTime;

        if (t_StepCycle > t_NextStep)
        {
            t_NextStep = stepInterval;
            t_StepCycle = 0.0f;

            PlayFootStepAudio();
        }
    }
    private void PlayFootStepAudio()
    {
        int n = Random.Range(1, footstepSounds.Length);
        audioSourceFootStep.clip = footstepSounds[n];
        audioSourceFootStep.Play();
    }
    #endregion
}
