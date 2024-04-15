using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TLS Scriptable Objects/Audio System/Single Player", order = 2)]
public class AudioSinglePlayerSO : ScriptableObject
{
    public List<AudioSinglePlayerListener> eventActionPlayer = new();

    // Raise event through different method signatures
    public void Raise(AudioSinglePlayerEventData eventData)
    {
        for (int i = eventActionPlayer.Count - 1; i >= 0; i--)
        {
            eventActionPlayer[i].OnEventRaised(eventData);
        }
    }

    // Manage Listeners
    public void RegisterListener(AudioSinglePlayerListener listener)
    {
        if (!eventActionPlayer.Contains(listener))
            eventActionPlayer.Add(listener);
    }

    public void UnregisterListener(AudioSinglePlayerListener listener)
    {
        if (eventActionPlayer.Contains(listener))
            eventActionPlayer.Remove(listener);
    }
}