using System.Collections;
using Cinemachine;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [Header("PREFABS")]

    [SerializeField]
    GameObject prefabPlayerCharacter;

    [SerializeField]
    GameObject prefabPlayerVCam;

    [SerializeField]
    GameObject prefabPlayerCanvas;

    [SerializeField]
    GameObject startVCam;

    [Header("CAMARA SETTINGS")]

    [SerializeField]
    bool HasChangeCamDelay = false;

    [SerializeField]
    float timeBeforeChangeCam = 2f;

    [Header("SPAWN SETTINGS")]

    [SerializeField]
    Vector2 playerSpawnPosition;

    [SerializeField]
    float timeBeforeSpawn = 2f;

    [Header("PLAYER INPUT SETTINGS")]

    [SerializeField]
    bool enableInputByStart = false;


    protected GameObject m_PlayerCharacter;
    protected PlayerInput m_PlayerInput;
    protected CinemachineVirtualCamera m_PlayerVCam;
    protected Canvas m_PlayerCanvas;

    protected WaitForSeconds m_TimeBeforeSpawn;
    protected WaitForSeconds m_TimeBeforeChangeCam;

    void Start()
    {
        m_TimeBeforeSpawn = new WaitForSeconds(timeBeforeSpawn);
        m_TimeBeforeChangeCam = new WaitForSeconds(timeBeforeChangeCam);

        StartCoroutine(PlayerSpawnCoroutine());

        if (!HasChangeCamDelay) startVCam.SetActive(false);
        else StartCoroutine(CameraControlCoroutine());
    }

    void PlayerSpawn()
    {
        var playerCanvas = Instantiate(prefabPlayerCanvas, prefabPlayerCanvas.transform.position, Quaternion.identity);
        m_PlayerCanvas = playerCanvas.GetComponent<Canvas>();

        Vector3 spawnPosition = new(playerSpawnPosition.x, playerSpawnPosition.y, 0f);
        Debug.Log($"Spawning player at position: {spawnPosition}");
        m_PlayerCharacter = Instantiate(prefabPlayerCharacter, spawnPosition, Quaternion.identity);

        var playerVCam = Instantiate(prefabPlayerVCam, prefabPlayerVCam.transform.position, Quaternion.identity);
        m_PlayerVCam = playerVCam.GetComponent<CinemachineVirtualCamera>();
        m_PlayerVCam.Follow = m_PlayerCharacter.transform;

        m_PlayerInput = m_PlayerCharacter.GetComponent<PlayerInput>();

        if (enableInputByStart)
            m_PlayerInput.EnablePlayerInput();
    }

    IEnumerator PlayerSpawnCoroutine()
    {
        yield return m_TimeBeforeSpawn;
        PlayerSpawn();
    }

    IEnumerator CameraControlCoroutine()
    {
        yield return m_TimeBeforeChangeCam;
        startVCam.SetActive(false);
    }

#if UNITY_EDITOR
    #region CONTROL PANEL PARAMETERS

    [Header("CONTROL PANEL - ONLY IN EDITOR")]

    [SerializeField]
    bool showPlayerSpawnPosition;
    [SerializeField]
    float spawnMarkSize = 0.25f;
    [SerializeField]
    Color colorViewField = new(0f, 1.0f, 0f, 1.0f);

    #endregion // CONTROL PANEL

    void OnDrawGizmosSelected()
    {
        if (showPlayerSpawnPosition)
        {
            Gizmos.color = colorViewField;
            Vector3 spawnPosition = new(playerSpawnPosition.x, playerSpawnPosition.y, 0f);
            Gizmos.DrawSphere(spawnPosition, spawnMarkSize);
        }
    }
#endif
}
