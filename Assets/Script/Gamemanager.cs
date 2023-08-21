using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gamemanager : MonoBehaviour
{

    #region ΩÃ±€≈Ê
    static public Gamemanager instance;
    private void Awake()
    {
        instance = this; 
    }
    #endregion

    public FollowCam followCam_start;

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
