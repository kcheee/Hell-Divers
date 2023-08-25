using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{

    #region ½Ì±ÛÅæ
    static public Gamemanager instance;
    private void Awake()
    {
        instance = this; 
    }
    #endregion

    public FollowCam followCam_start;
    public GameObject TowerUI;
    public GameObject EndUI;
    public CanvasGroup fade;

    // ¼öÁ¤ ÇØ¾ßÇÔ.
    IEnumerator camstart()
    {
        fade.DOFade(0, 2);
        yield return new WaitForSeconds(0.5f);
        followCam_start.enabled = true;
    }

     void Start()
    {
        StartCoroutine(camstart());
    }


}
