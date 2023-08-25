using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    // ¼öÁ¤ ÇØ¾ßÇÔ.
    IEnumerator camstart()
    {
        yield return new WaitForSeconds(5);
        followCam_start.enabled = true;
    }

     void Start()
    {
        StartCoroutine(camstart());
    }


}
