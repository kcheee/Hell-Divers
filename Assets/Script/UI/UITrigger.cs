using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using JetBrains.Annotations;
using Photon.Pun;

public class UITrigger : MonoBehaviourPun
{
    public GameObject uiElement; // Ȱ��ȭ�� UI ���
    public GameObject buttonElement; // Ȱ��ȭ�� UI ���

    public float distance;
    public float UI_on;
    public float button_on;

    Text text;
    private float initialAlpha;
    Color T_C;
    Color F_C;
    bool T_flag=false;
    bool B_flag =false;
    // ����� ������Ʈ 
    private Transform closestObject;

    AudioSource audioSource;

    bool flag = false;
    IEnumerator delay()
    {
        yield return new WaitForSeconds(2);
        flag = true;
    }

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        text = uiElement.GetComponent<Text>();
        T_C = new Color(255, 255, 255, 1);
        F_C = new Color(255, 255, 255, 0);
        StartCoroutine(delay());
    }
    private void Update()
    {
        if (!flag) return;


        closestObject = FindClosestObject();

        if(closestObject != null)
        {
            distance = Vector3.Distance(transform.position, closestObject.position);

            if (distance < UI_on && !T_flag)
            {
                T_flag = true; // UI Ȱ��ȭ������ ǥ��

                uiElement.SetActive(true);

                text.DOColor(T_C, 0.5f);
            }
            else if (distance >= UI_on && T_flag)
            {
                T_flag = false; // UI ��Ȱ��ȭ������ ǥ��

                text.DOColor(F_C, 0.5f).OnComplete(() =>
                {
                    uiElement.SetActive(false);
                });

                Debug.Log("����");
            }
            // ��ư
            if (distance < button_on && !B_flag)
            {

                B_flag = true; // UI Ȱ��ȭ������ ǥ��
                buttonElement.SetActive(true);
                text.DOColor(Color.yellow, 0.1f);

                // EŰ�� ������ ��
            }
            else if (distance >= button_on && B_flag)
            {
                B_flag = false; // UI ��Ȱ��ȭ������ ǥ��
                buttonElement.SetActive(false);
                text.DOColor(T_C, 0.1f).OnComplete(() =>
                {

                });
            }
        }
        

        if (B_flag)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(LobbySceneChange.playerReady<2)  audioSource.Play();
                photonView.RPC(nameof(ReadyCount), RpcTarget.All);
            }
        }

        //if (distance < button_on)
        //{
        //    text.DOColor(F_C, 0.2f).OnComplete(() =>
        //            {
        //                //uiElement.SetActive(false);
        //            });
        //}
    }

    protected Transform FindClosestObject()
    {
        if(PlayerManager.instace.PlayerList.Count <= 0) return null;
        Transform closest = PlayerManager.instace.PlayerList[0].transform;
        float closestDistance = Vector3.Distance(transform.position, closest.position);

        foreach (PlayerTest1 obj in PlayerManager.instace.PlayerList)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closest = obj.transform;
                closestDistance = distance;
            }
        }
        return closest;
    }


    [PunRPC]
    void ReadyCount()
    {
        LobbySceneChange.playerReady++;
    }

}
