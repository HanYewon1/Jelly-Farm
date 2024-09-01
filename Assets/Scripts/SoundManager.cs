using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource SfxPlayer;
    SoundManager soundManager;
    public AudioClip[] audioClips;
    public static SoundManager Instance;
    void Awake()
    {
        SfxPlayer=GameObject.Find("SfxPlayer").GetComponent<AudioSource>();
    }
    public void Sound(string type)
    {
        int index = 0;

        switch (type) {
            case "Button": index = 0; break;
            case "Buy": index = 1; break;
            case "Clear": index = 2; break;
            case "Fail": index = 3; break;
            case "Grow": index = 4; break;
            case "Pause In": index = 5; break;
            case "Pause Out": index=6; break;
            case "Sell":index = 7; break;
            case "Touch": index=8; break;
            case "Unlock": index = 9; break;
        }
        
        
        
    }
}
