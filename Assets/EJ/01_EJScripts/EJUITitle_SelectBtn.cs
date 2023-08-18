using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EJUITitle_SelectBtn : Button
{
    //type
    public int n;

    //Image image;
    // Monobehavior �� Start ȣ�� ���
    protected override void Start()
    {
        base.Start();
        print(n);
    }

    // Update is called once per frame
    void Update()
    {
     //e������
     //color���� 203�̰� n���� ���� ����Ǵ� �Լ� ����
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        //ColorBlock colorBlock = colors;
        //Color normal = colorBlock.normalColor;
        //normal.a = 1;
        //colorBlock.normalColor = normal;
        //colors = colorBlock;


        Color color = image.color;
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
