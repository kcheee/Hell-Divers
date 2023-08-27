using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBossSFX : MonoBehaviour
{
    public static EJBossSFX instance;

    public AudioClip bombFlyingSFX;
    public AudioClip bombExploSFX;
    public AudioClip gausCannonSFX;
    public AudioClip machineGunSFX;
    public AudioClip deadSFX;

    AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaybombFlyingSFX()
    {
        audiosource.PlayOneShot(bombFlyingSFX);
    }

    public void PlaybombExploSFX()
    {
        audiosource.PlayOneShot(bombExploSFX);
    }
    public void PlaygausCannonSFX()
    {
        audiosource.PlayOneShot(gausCannonSFX);
    }
    public void PlaymachineGunSFX()
    {
        audiosource.PlayOneShot(machineGunSFX);
    }
    public void DeadSFX()
    {
        audiosource.PlayOneShot(deadSFX);
    }
}
