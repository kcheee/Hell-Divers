using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public static PlayerSound instance;
    public enum P_SOUND { 
        Reloading,Input
    
    }
    public AudioClip[] clips;

    public AudioClip GetClip(P_SOUND ps) {
        return clips[(int)ps];
    }
    // Start is called before the first frame update
    private void Awake()
    {
        instance = GetComponent<PlayerSound>();
    }

}
