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

        // master가 씬을 넘기면 같이 넘어감.
        PhotonNetwork.AutomaticallySyncScene = true;
        StartCoroutine(Fade(true));


    }

    private void Update()
    {
        if (playerReady >= 3&& !flag)
        {
            flag = true;
            Debug.Log("Ui호출");
            photonView.RPC(nameof(sound), RpcTarget.All);
            photonView.RPC(nameof(PanelOn), RpcTarget.All);

            //PhotonNetwork.LoadLevel("MainScene");
        }
        // test용
        if (Input.GetKeyDown(KeyCode.L))
        {
            flag = true;
            Debug.Log("Ui호출");
            photonView.RPC(nameof(PanelOn), RpcTarget.All);
            photonView.RPC(nameof(sound), RpcTarget.All);

        }
        //테스트용 씬 넘김.
        if (Input.GetKeyDown(KeyCode.U))
        {
            // playermanager에 있는 모든 요소 삭제 후 이동.
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

    public IEnumerator Fade(bool In)
    {
        if (In)
            FadeImg.DOFade(0, 0.5f);
        else
        {
            // 오디오 실행
            audioSource.clip = clip;
            audioSource.Play();
            SoundManager.instance.BGMSrc.DOFade(0, 2);
            FadeImg.DOFade(1, 2f);
            yield return new WaitForSeconds(3);
            PhotonNetwork.LoadLevel("MainScene");
        }
    }

}
