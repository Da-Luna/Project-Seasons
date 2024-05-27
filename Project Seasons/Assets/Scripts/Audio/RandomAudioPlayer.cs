using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
    [System.Serializable]
    public struct TileOverride
    {
        public TileBase tile;
        public AudioClip[] clips;
    }

    public AudioClip[] clips;

    public TileOverride[] overrides;

    [Header("PLAY ON AWAKE")]

    [SerializeField]
    bool playOnAwake = false;

    [Header("DELAY PLAY SETTINGS")]

    [SerializeField]
    bool playWithDelay = false;

    [SerializeField]
    float delayTime = 1f;

    WaitForSeconds m_delayTime;

    [Header("RANDOM PITCH SETTINGS")]

    [SerializeField]
    bool randomizePitch = false;
    [SerializeField]
    float pitchRange = 0.2f;

    protected AudioSource m_Source;
    protected Dictionary<TileBase, AudioClip[]> m_LookupOverride;

    private void Awake()
    {
        m_Source = GetComponent<AudioSource>();
        m_LookupOverride = new Dictionary<TileBase, AudioClip[]>();

        for (int i = 0; i < overrides.Length; ++i)
        {
            if (overrides[i].tile == null)
                continue;

            m_LookupOverride[overrides[i].tile] = overrides[i].clips;
        }

    }

    private void OnEnable()
    {
        if (playWithDelay)
        {
            m_delayTime = new WaitForSeconds(delayTime);
            StartCoroutine(DelayTimer());
        }
        else if (playOnAwake)
        {
            PlayRandomSound();
        }
    }

    public void PlayRandomSound(TileBase surface = null)
    {
        AudioClip[] source = clips;

        if (surface != null && m_LookupOverride.TryGetValue(surface, out AudioClip[] temp))
            source = temp;

        int choice = Random.Range(0, source.Length);

        if (randomizePitch)
            m_Source.pitch = Random.Range(1.0f - pitchRange, 1.0f + pitchRange);

        m_Source.PlayOneShot(source[choice]);
    }

    public void SetVolumePlay(float setVol)
    {
        m_Source.volume = setVol;
        m_Source.PlayOneShot(clips[0]);
    }

    public void MoveTowardsVolume(float targetVol)
    {
        float tMultipier = 0.1f;
        m_Source.volume = Mathf.MoveTowards(m_Source.volume, targetVol, tMultipier * Time.deltaTime);
    }
    
    public void MoveTowardsPitch(float targetPitch)
    {
        float tMultipier = 0.4f;
        m_Source.pitch = Mathf.MoveTowards(m_Source.pitch, targetPitch, tMultipier * Time.deltaTime);
    }

    public bool CheckIfPlaying()
    {
        if (m_Source.isPlaying)
            return true;

        return false;
    }

    public void Stop()
    {
        m_Source.Stop();
    }

    IEnumerator DelayTimer()
    {
        yield return m_delayTime;
        PlayRandomSound();
    }
}
