using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Lastscene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.BGMSrc.clip = SoundManager.instance.BGMClip.Ending;
        SoundManager.instance.BGMSrc.Play();
        SoundManager.instance.BGMSrc.DOFade(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
