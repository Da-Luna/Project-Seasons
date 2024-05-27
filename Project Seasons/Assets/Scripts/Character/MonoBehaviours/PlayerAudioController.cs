using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Footstep Audio Settings")]
    [SerializeField]
    AudioSource audioSourceMovement;
    [SerializeField]
    AudioClip audioClipJump;
    [SerializeField]
    AudioClip audioClipLand;
    [SerializeField]
    AudioClip[] footstepSounds;

    const float stepInterval = 0.275f;
    const float stepMultipier = 0.8f;
    [SerializeField]
    float stepSprintMultipier = 1.1f;

    float t_StepCycle = 0.0f;
    float t_NextStep;

    #region MOVEMENT SOUNDS

    public void PlayJumpSound()
    {
        audioSourceMovement.PlayOneShot(audioClipJump);
    }

    public void PlayLandSound()
    {
        audioSourceMovement.PlayOneShot(audioClipLand);
    }

    public void ProgressStepCycle(bool isSprint)
    {
        if (!isSprint)
        {
            t_StepCycle += stepMultipier * Time.deltaTime;
        }
        else
        {
            t_StepCycle += stepSprintMultipier * Time.deltaTime;
        }

        if (t_StepCycle > t_NextStep)
        {
            t_NextStep = stepInterval;
            t_StepCycle = 0.0f;

            PlayFootStepAudio();
        }
    }

    void PlayFootStepAudio()
    {
        int n = Random.Range(1, footstepSounds.Length);
        audioSourceMovement.PlayOneShot(footstepSounds[n]);
    }



    #endregion // MOVEMENT SOUNDS

}
