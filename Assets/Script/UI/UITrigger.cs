using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;

public class UITrigger : MonoBehaviour
{
    public GameObject uiElement; // 활성화할 UI 요소
    public GameObject buttonElement; // 활성화할 UI 요소

    public float distance;
    public float UI_on;
    public float button_on;
    GameObject player;
    
    Text text;
    private float initialAlpha;
    Color T_C;
    Color F_C;
    bool T_flag=false;
    bool B_flag =false;
    private void Start()
    {
        text = uiElement.GetComponent<Text>();
        T_C = new Color(255, 255, 255, 1);
        F_C = new Color(255, 255, 255, 0);
        player = GameObject.Find("Player");
    }
    private void Update()
    {
 
        distance = Vector3.Distance(transform.position,player.transform.position);

        if (distance < UI_on && !T_flag)
        {
            T_flag = true; // UI 활성화됨으로 표시

            uiElement.SetActive(true);

            text.DOColor(T_C, 0.5f);
        }
        else if (distance >= UI_on && T_flag)
        {
            T_flag = false; // UI 비활성화됨으로 표시

            text.DOColor(F_C, 0.5f).OnComplete(() =>
            {
                uiElement.SetActive(false);
            });

            Debug.Log("실행");
        }
        // 버튼
        if (distance < button_on && !B_flag)
        {
            
            B_flag = true; // UI 활성화됨으로 표시
            buttonElement.SetActive(true);
            text.DOColor(Color.yellow, 0.1f);

            // E키를 눌렀을 때
        }
        else if (distance >= button_on && B_flag)
        {
            B_flag = false; // UI 비활성화됨으로 표시
            buttonElement.SetActive(false);
            text.DOColor(T_C, 0.1f).OnComplete(() =>
            {
                
            });
        }

        if(B_flag)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log(LobbySceneChange.playerReady);
                LobbySceneChange.playerReady++;
                GetComponent<UITrigger>().enabled = false;
            }
        }

        //if (distance < button_on)
        //{
        //    text.DOColor(F_C, 0.2f).OnComplete(() =>
        //            {
        //                uiElement.SetActive(false);
        //            });
        //    Debug.Log("실행");
        //}
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player")) // 플레이어와의 충돌 감지
    //    {        
    //        uiElement.SetActive(true);

    //        text.DOColor(T_C, 0.2f);
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player")) // 플레이어와의 충돌 해제 감지
    //    {
    //        text.DOColor(F_C, 0.2f).OnComplete(() =>
    //        {
    //         uiElement.SetActive(false);
    //        });
    //    }
    //}
}
