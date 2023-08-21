using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EJUITitle_SelectBtn : Button
{
    //type
    public int n;
    Color color;

    //Image image;
    // Monobehavior �� Start ȣ�� ���
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        //color���� 203�̰� n���� ���� ����Ǵ� �Լ� ����
        //e������
        if(Input.GetKeyDown(KeyCode.E) && color.a ==203)
        {
                if (n == 1)
                {
                    //���ӽ����� �̵�
                    SceneManager.LoadScene("ConnectionScene");
                }else
                {


                //���� �ٸ� ������ �̵�
                SceneManager.LoadScene("ConnectionScene");
                }                  
        }
     
    }

    
    public void ClickSceneMove()
    {

            SceneManager.LoadScene("ConnectionScene");

    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        //ColorBlock colorBlock = colors;
        //Color normal = colorBlock.normalColor;
        //normal.a = 1;
        //colorBlock.normalColor = normal;
        //colors = colorBlock;

        color = image.color;
        color.a = 203;
        image.color = color;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        //ColorBlock colorBlock = colors;
        //Color normal = colorBlock.normalColor;
        //normal.a = 0;
        //colorBlock.normalColor = normal;
        //colors = colorBlock;

        Color color = image.color;
        color.a = 0;
        image.color = color;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        //monobehaviour�� loadScene�������� ��� 
        //��ư�� ������ LobbyScene���� �̵��ϰ� �ʹ�
    }

}
