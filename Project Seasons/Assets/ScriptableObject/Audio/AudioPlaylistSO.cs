using UnityEngine;

[CreateAssetMenu(menuName = "TLS Scriptable Objects/Audio System/Audio Playlist", order = 1)]
public class AudioPlaylistSO : ScriptableObject
{
    public AudioClip[] audioPlaylist;
}
