using UnityEngine;
using Cinemachine;

public class CineMConfinerAutoSetup : MonoBehaviour
{
    public string m_CameraConfinerName = "ConfinerCollider";

    CinemachineConfiner m_CinemachineConfiner;

    void OnEnable()
    {
        m_CinemachineConfiner = GetComponent<CinemachineConfiner>();

        if (m_CinemachineConfiner == null)
        {
            Debug.LogError("CinemachineConfiner component not found on the GameObject.");
            return;
        }

        var gObj = GameObject.Find(m_CameraConfinerName);
        if (gObj == null)
        {
            Debug.LogError($"GameObject with name '{m_CameraConfinerName}' not found in the scene.");
            return;
        }

        var boxCollider = gObj.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider component not found on the GameObject.");
            return;
        }

        m_CinemachineConfiner.m_BoundingVolume = boxCollider;
    }
}
