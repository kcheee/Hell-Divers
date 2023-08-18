using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource BGMSrc;
    public AudioSource SFXSrc;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SfxPlay(AudioClip clip)
    {
        SFXSrc.PlayOneShot(clip);

    }
}
