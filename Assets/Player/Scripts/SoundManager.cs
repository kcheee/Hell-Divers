using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    public static SoundManager instance;   
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [System.Serializable]
    public struct BGMCLIP
    {
        public AudioClip Title;
        public AudioClip Lobby;
        public AudioClip Main;
        public AudioClip Ending;
    }

    [SerializeField] public BGMCLIP BGMClip;

    public AudioSource BGMSrc;
    public AudioSource SFXSrc;


    // Start is called before the first frame update
    void Start()
    {
        // title bgm Ω√¿€.
        BGMSrc.clip = BGMClip.Title;
        BGMSrc.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SfxPlay(AudioClip clip)
    {
        SFXSrc.PlayOneShot(clip);

    }
    public void BgmPlay(AudioClip clip)
    {
        BGMSrc.clip = clip;
        BGMSrc.Play();
    }
}
