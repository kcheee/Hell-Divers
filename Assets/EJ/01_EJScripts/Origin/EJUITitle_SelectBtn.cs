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
    // Monobehavior 속 Start 호출 방법
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //color값이 203이고 n값에 따라서 실행되는 함수 설정
        //e누르고
        if(Input.GetKeyDown(KeyCode.E) && color.a ==203)
        {
                if (n == 1)
                {
                //게임신으로 이동
                SceneManager.LoadScene("ConnectionScene");
            }
            else
                {
                //뭔가 다른 씬으로 이동
                SceneManager.LoadScene("ConnectionScene");
            }                  
        }
    }

    
    public void ClickSceneMove()
    {
        StartCoroutine(FadeOUt());
       //SceneManager.LoadScene("ConnectionScene");

    }

    IEnumerator FadeOUt()
    {
        EJUITitle_Logo.Instance.fadeout();
        yield return new WaitForSeconds(2);
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

        //monobehaviour의 loadScene가져오는 방법 
        //버튼을 누르면 LobbyScene으로 이동하고 싶다
    }

}
