using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;

public class UITrigger : MonoBehaviour
{
    public GameObject uiElement; // Ȱ��ȭ�� UI ���
    public GameObject buttonElement; // Ȱ��ȭ�� UI ���

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
            T_flag = true; // UI Ȱ��ȭ������ ǥ��

            uiElement.SetActive(true);

            text.DOColor(T_C, 1f);
        }
        else if (distance >= UI_on && T_flag)
        {
            T_flag = false; // UI ��Ȱ��ȭ������ ǥ��

            text.DOColor(F_C, 0.2f).OnComplete(() =>
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
        }
        else if (distance >= button_on && B_flag)
        {
            B_flag = false; // UI ��Ȱ��ȭ������ ǥ��
            buttonElement.SetActive(false);
            text.DOColor(T_C, 0.1f).OnComplete(() =>
            {
                
            });

            //Debug.Log("����");
        }

        //if (distance < button_on)
        //{
        //    text.DOColor(F_C, 0.2f).OnComplete(() =>
        //            {
        //                uiElement.SetActive(false);
        //            });
        //    Debug.Log("����");
        //}
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player")) // �÷��̾���� �浹 ����
    //    {        
    //        uiElement.SetActive(true);

    //        text.DOColor(T_C, 0.2f);
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player")) // �÷��̾���� �浹 ���� ����
    //    {
    //        text.DOColor(F_C, 0.2f).OnComplete(() =>
    //        {
    //         uiElement.SetActive(false);
    //        });
    //    }
    //}
}
