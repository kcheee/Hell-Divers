using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LobbySceneChange : MonoBehaviourPun
{
    static public LobbySceneChange instance;

    private void Awake()
    {
        instance = this; 
    }
    public static int playerReady = 0;
    public GameObject spawnPos;
    public GameObject panel;
    public CanvasGroup FadeImg;
    bool flag = false;

    public AudioClip clip;
    AudioSource audioSource;

    private void Start()
    {
        GameObject player =  PhotonNetwork.Instantiate("AlphaPlayer 1", spawnPos.transform.position, Quaternion.identity);
        player.transform.localScale = new Vector3(3,3,3);

        audioSource = GetComponent<AudioSource>();

        // master�� ���� �ѱ�� ���� �Ѿ.
        PhotonNetwork.AutomaticallySyncScene = true;
        StartCoroutine(Fade(true));

        SoundManager.instance.BGMSrc.clip = SoundManager.instance.BGMClip.Lobby;
        SoundManager.instance.BGMSrc.Play();
        SoundManager.instance.BGMSrc.DOFade(1, 5);

    }

    private void Update()
    {
        if (playerReady >= 3&& !flag)
        {
            flag = true;
            Debug.Log("Uiȣ��");
            photonView.RPC(nameof(sound), RpcTarget.All);
            photonView.RPC(nameof(PanelOn), RpcTarget.All);

            //PhotonNetwork.LoadLevel("MainScene");
        }
        // test��
        if (Input.GetKeyDown(KeyCode.L))
        {
            flag = true;
            Debug.Log("Uiȣ��");
            photonView.RPC(nameof(PanelOn), RpcTarget.All);
            photonView.RPC(nameof(sound), RpcTarget.All);

        }
        //�׽�Ʈ�� �� �ѱ�.
        if (Input.GetKeyDown(KeyCode.U))
        {
            // playermanager�� �ִ� ��� ��� ���� �� �̵�.
            //PlayerManager.instace.PlayerList.Clear();
            //PhotonNetwork.LoadLevel("MainScene");
            StartCoroutine(Fade(false));
        }
    }


    [PunRPC]
    void PanelOn()
    {
        panel.SetActive(true);
    }

    [PunRPC]
    public void changeScene()
    {
        playerReady++;
        Debug.Log(playerReady);
    }

    [PunRPC]
    void sound()
    {
        audioSource.Play();
    }

   public void delePun(bool flag)
    {
        photonView.RPC(nameof(pun_Fade), RpcTarget.All, flag);
    }
    [PunRPC]
    public void pun_Fade(bool flag)
    {
        StartCoroutine(Fade(flag));
    }

    public IEnumerator Fade(bool In)
    {
        if (In)
            FadeImg.DOFade(0, 0.5f);
        else
        {
            Debug.Log("��� ���� �Ǿ���");
            // ����� ����
            audioSource.clip = clip;
            audioSource.Play();
            SoundManager.instance.BGMSrc.DOFade(0, 2);
            FadeImg.DOFade(1, 2f);
            yield return new WaitForSeconds(3);
            PhotonNetwork.LoadLevel("MainScene");
        }
    }

}
