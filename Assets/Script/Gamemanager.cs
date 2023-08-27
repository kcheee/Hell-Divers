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

    #region �̱���
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

    // ���� �ؾ���.
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

    // Ending scene���� ��ȯ
    public IEnumerator ending()
    {
        SoundManager.instance.BGMSrc.DOFade(0, 2);
        uimanager.fade.DOFade(1, 5);
        yield return new WaitForSeconds(4f);
        SoundManager.instance.BGMSrc.DOFade(0, 2);
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
