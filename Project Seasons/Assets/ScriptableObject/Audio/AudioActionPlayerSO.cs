using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TLS Scriptable Objects/Audio System/Action Player", order = 3)]
public class AudioActionPlayerSO : ScriptableObject
{
    public List<AudioActionPlayerListener> eventActionPlayer = new();

    // Raise event through different method signatures
    public void Raise(AudioPlaylistPlayerEventData eventData)
    {
        for (int i = eventActionPlayer.Count - 1; i >= 0; i--)
        {
            eventActionPlayer[i].OnEventRaised(eventData);
        }
    }

    // Manage Listeners
    public void RegisterListener(AudioActionPlayerListener listener)
    {
        if (!eventActionPlayer.Contains(listener))
            eventActionPlayer.Add(listener);
    }

    public void UnregisterListener(AudioActionPlayerListener listener)
    {
        if (eventActionPlayer.Contains(listener))
            eventActionPlayer.Remove(listener);
    }
}
