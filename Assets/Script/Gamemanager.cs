using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static EnemyInfo;
using static Gamemanager;
using Photon.Pun;

public class Gamemanager : MonoBehaviourPun
{

    #region 싱글톤
    static public Gamemanager instance;
    private void Awake()
    {
        instance = this; 
    }
    #endregion

    public FollowCam followCam_start;

    [System.Serializable]
    public struct UIMANAGER
    {
         public GameObject TowerUI;
         public GameObject EndUI;
         public CanvasGroup fade;
         public CanvasGroup MissionUI;
    }
         public Image Bossmission;

    [SerializeField]
    protected UIMANAGER uimanager;

    // 수정 해야함.
    IEnumerator camstart()
    {
        uimanager.fade.DOFade(0, 5);
        yield return new WaitForSeconds(0.5f);
        followCam_start.enabled = true;
    }
    #region UI
    public IEnumerator MissionUIOnOff()
    {
        uimanager.MissionUI.DOFade(1,2);
        yield return new WaitForSeconds(5);
        uimanager.MissionUI.DOFade(0, 2);
    }

    #endregion

    // Ending scene으로 전환
    public IEnumerator ending()
    {
        yield return new WaitForSeconds(2f);
        SoundManager.instance.BGMSrc.DOFade(0, 5);
        uimanager.fade.DOFade(1, 7);
        yield return new WaitForSeconds(4f);
        yield return new WaitForSeconds(2f);
        PhotonNetwork.LoadLevel("LastScene");
    }
    void Start()
    {       
        StartCoroutine(camstart());
        StartCoroutine(MissionUIOnOff());

        // mainscene bgm 
        SoundManager.instance.BGMSrc.clip = SoundManager.instance.BGMClip.Main;
        SoundManager.instance.BGMSrc.Play();
        SoundManager.instance.BGMSrc.DOFade(1, 5);
    }

    private void Update()
    {
        // endingscene
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(ending());
        }
    }

}
