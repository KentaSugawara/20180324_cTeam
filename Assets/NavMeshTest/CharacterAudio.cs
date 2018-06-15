using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour {
    [SerializeField]
    private AudioSource _Audio_Unique;
    [SerializeField]
    private AudioSource _Audio_Happy;
    [SerializeField]
    private AudioSource _Audio_Angry;
    [SerializeField]
    private AudioSource _Audio_Sad;
    [SerializeField]
    private AudioSource _Audio_Funny;

    public enum eAudioType
    {
        Unique,
        Happy,
        Angry,
        Sad,
        Funny
    }

    public void Play(eAudioType Type)
    {
        if (Type == eAudioType.Unique) if (_Audio_Unique != null) _Audio_Unique.Play();
        else if (Type == eAudioType.Happy) if (_Audio_Happy != null) _Audio_Happy.Play();
        else if (Type == eAudioType.Angry) if (_Audio_Angry != null) _Audio_Angry.Play();
        else if (Type == eAudioType.Sad) if (_Audio_Sad != null) _Audio_Sad.Play();
        else if (Type == eAudioType.Funny) if (_Audio_Funny != null) _Audio_Funny.Play();
    }
}
