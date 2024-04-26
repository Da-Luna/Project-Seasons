using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField, Tooltip("Reference to the player camera controller.")]
    PlayerCameraController camController;
    [SerializeField, Tooltip("Reference to the player controller GameObject.")]
    GameObject playerController;
    [SerializeField, Tooltip("Reference to the player controller script.")]
    PlayerController controller;
    [SerializeField, Tooltip("Reference to the player animation controller script.")]
    PlayerAnimationController animationController;
    [SerializeField, Tooltip("Reference to the player audio controller script.")]
    PlayerAudioController audioController;
    [SerializeField, Tooltip("Reference to the player particle controller script.")]
    PlayerParticleController particleController;
    [SerializeField, Tooltip("Reference to the player collectables controller script.")]
    PlayerCollectablesController collectablesController;
    #endregion // SCRIPT REFERENCES

    #region SPAWN SETTINGS
    [Header("SPAWN SETTINGS")]
    [SerializeField, Tooltip("Time delay before the player character spawns.")]
    float timeBeforeSpawn = 2.0f;
    [SerializeField, Tooltip("Time delay before the player character becomes controllable.")]
    float timeBeforeActiveControl = 1.0f;
    #endregion // SPAWN SETTINGS

    #region LEVEL COMPLETE SETTINGS
    [Header("LEVEL COMPLETE SETTINGS")]
    [SerializeField, Tooltip("Delay before the victory voice sound plays.")]
    float voiceStartDelay = 1.0f;
    #endregion // LEVEL COMPLETE SETTINGS

    public Transform Player { get; private set; }

    public bool playerCanStart;

    // Animation state names
    const string appear = "Appear";
    const string victory = "Victory";
    const string idle = "Idle";
    const string walk = "Walk";
    const string run = "Run";
    const string jump = "Jump";
    const string falling = "Falling";

    #region OnEnable Initialization
    private void InitReference()
    {
        if (camController == null)
        {
            camController = transform.Find("PlayerVCam").GetComponent<PlayerCameraController>();
#if UNITY_EDITOR
            Debug.Log("PlayerManager: camController is NULL, global search with name **PlayerVCam** function was used");
#endif
        }

        if (playerController == null)
        {
            playerController = transform.Find("PlayerController").gameObject;
#if UNITY_EDITOR
            Debug.Log("PlayerManager: playerCharacter is NULL, local search with name **PlayerController** function was used");
#endif
        }

        if (controller == null)
            controller = playerController.GetComponent<PlayerController>();

        if (animationController == null)
            animationController = playerController.GetComponent<PlayerAnimationController>();

        if (audioController == null)
            audioController = playerController.GetComponent<PlayerAudioController>();

        if (particleController == null)
            particleController = playerController.GetComponent<PlayerParticleController>();

        if (collectablesController == null)
            collectablesController = playerController.GetComponent<PlayerCollectablesController>();
    }

    private void OnEnable()
    {
        InitReference();

        if (Player == null)
            PlayerSpawn();
    }
    #endregion

    #region Player Spawn
    public void PlayerSpawn()
    {
        Player = transform;

        StartCoroutine(HanldePlayerAppears());
    }
    private IEnumerator HanldePlayerAppears()
    {
        yield return new WaitForSeconds(timeBeforeSpawn);

        playerController.SetActive(true);
        controller.enabled = false;

        animationController.PlayAnimation(appear);
        audioController.PlayAppearGameSound();

        yield return new WaitForSeconds(timeBeforeActiveControl);

        controller.enabled = true;
        playerCanStart = true;
    }
    #endregion

    #region Handle Player Executes
    private void HandlePlayerExecutes()
    {
        if (!controller.enabled) return;

        if (controller.IsGrounded)
        {
            if (controller.ExeJump)
            {
                PlayJump();

            }
            else if (!controller.IsMoving)
            {
                animationController.PlayAnimation(idle);

                particleController.StopFootstepParticle();
                particleController.StopTrailParticle();
            }
            else
            {
                PlayMovement();
            }
        }
        else if (controller.PlayerRb.velocity.y < -0.5f)
        {
            animationController.PlayAnimation(falling);

            particleController.StopFootstepParticle();
        }
    }

    private void PlayJump()
    {
        animationController.PlayAnimation(jump);
        audioController.PlayJumpSound();

        particleController.StopFootstepParticle();
        particleController.PlayTrailParticle();
    }
    private void PlayMovement()
    {
        if (!controller.ExeSprint)
        {
            animationController.PlayAnimation(walk);
        }
        else
        {
            animationController.PlayAnimation(run);
        }

        particleController.PlayFootstepParticle();
        particleController.PlayTrailParticle();

        audioController.ProgressStepCycle(controller.InputMoveValue, controller.ExeSprint);
    }
    #endregion

    #region Collectables
    /// <summary>
    /// Updates the number of collected coins.
    /// </summary>
    /// <returns>The number of collected coins.</returns>
    public int UpdateCollectedCoins()
    {
        return collectablesController.LocalCollectedCoins;
    }
    #endregion

    #region Trigger Level Complete
    public void TriggerLevelComplete()
    {
        controller.enabled = false;

        particleController.StopFootstepParticle();
        particleController.StopTrailParticle();
        
        animationController.PlayAnimation(victory);

        audioController.PlayVictoryGameSound();

        StartCoroutine(WaitBeforeVoicePlay());
    }
    private IEnumerator WaitBeforeVoicePlay()
    {
        yield return new WaitForSeconds(voiceStartDelay);
        audioController.PlayVictoryVoiceSound();
    }
    #endregion

    private void Update()
    {
        if (!playerCanStart) return;

        HandlePlayerExecutes();
        controller.HandleMovementSpeed();
        controller.SetSpriteDirection();
    }

    private void FixedUpdate()
    {
        if (!playerCanStart) return;

        controller.GroundCheck();
        controller.ExecuteMovement();
        camController.CameraBehavior();
    }
}
