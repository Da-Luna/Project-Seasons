using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the introductory sequence of the game, allowing players to skip it via inputs or automatically transitioning to the next scene after a set time.
/// </summary>
public class IntroSequence : MonoBehaviour
{
    [Header("INPUT SETTINGS")]

    [SerializeField, Tooltip("Reference to the InputReader scriptable object for handling player inputs.")]
    InputReaderSO inputReader;

    [SerializeField, Tooltip("Determines whether the intro sequence can be skipped by player inputs.")]
    bool introCanBeSkipped = true;

    [Header("SCENE TRANSITION SETTINGS")]

    [SerializeField, Tooltip("Time until the scene automatically changes if intro sequence is not skipped.")]
    float timeUntilChangeScene = 6.5f;

    WaitForSeconds waitTime;
    int nextSceneIndex;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        waitTime = new WaitForSeconds(timeUntilChangeScene);
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        StartCoroutine(AutoSkipIntroSequence());

        if (introCanBeSkipped)
        {
            EnableInputs();
        }
    }
    
    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (introCanBeSkipped)
        {
            DisableInputs();
        }
    }

    /// <summary>
    /// Enables player inputs for UI actions.
    /// </summary>
    void EnableInputs()
    {
        // Enable UI actions for player inputs
        inputReader.EnableUIActions();

        // Subscribe to input events
        inputReader.UISubmit += ReadSubmitInput;
        inputReader.UICancel += ReadCancelInput;
    }

    /// <summary>
    /// Disables player inputs for UI actions.
    /// </summary>
    void DisableInputs()
    {
        // Disable UI actions for player inputs
        inputReader.DisableUIActions();

        // Unsubscribe from input events
        inputReader.UISubmit -= ReadSubmitInput;
        inputReader.UICancel -= ReadCancelInput;
    }

    /// <summary>
    /// Event handler for submit input action.
    /// </summary>
    void ReadSubmitInput()
    {
        // Stop any ongoing coroutine and load the next scene
        StopAllCoroutines();
        SceneManager.LoadScene(nextSceneIndex);
    }

    /// <summary>
    /// Event handler for cancel input action.
    /// </summary>
    void ReadCancelInput()
    {
        // Stop any ongoing coroutine and load the next scene
        StopAllCoroutines();
        SceneManager.LoadScene(nextSceneIndex);
    }

    /// <summary>
    /// Coroutine to automatically skip the intro sequence after a specified time.
    /// </summary>
    IEnumerator AutoSkipIntroSequence()
    {
        // Wait for specified time then load the next scene
        yield return waitTime;
        SceneManager.LoadScene(nextSceneIndex);
    }
}