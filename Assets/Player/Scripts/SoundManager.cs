using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource source;
    
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(AudioClip clip) {
        source.clip = clip;
        source.Play();
    }
}
