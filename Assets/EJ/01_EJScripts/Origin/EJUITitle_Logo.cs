using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EJUITitle_Logo : MonoBehaviour
{
    //1번째 뜨는 것
    public GameObject first;

    public GameObject mainLogo;
    public GameObject minLogo;
    public GameObject bg;
    public GameObject PressAnyKeyText;

    //2번째에 뜨는 것
    public GameObject Second;

    //mainLogo
    Vector3 mainLogoScale;
    Image mainLogoOpacity;

    //subLogo
    Image subLogoOpacity;

    //BG
    Vector3 bgScale;
    Image bgOpacity;

    //PressAnyKey
    TextMeshProUGUI pressAnyKeyOpacity;

    Color c;


    // Start is called before the first frame update
    void Start()
    {
        //c = new Color(0, 0, 0, 1);

        //mainLogo
        mainLogoScale = mainLogo.transform.localScale;
        mainLogoOpacity = mainLogo.gameObject.GetComponent<Image>();
        //mainLogoOpacity.color.a = 0;

        //subLogo
        subLogoOpacity = minLogo.gameObject.GetComponent<Image>();
        //subLogoOpacity = 0;

        //bg
        bgScale = bg.gameObject.transform.localScale;
        bgOpacity = bg.gameObject.GetComponent<Image>();

        //pressAnyKey
        pressAnyKeyOpacity = PressAnyKeyText.GetComponent<TextMeshProUGUI>();
        mainLogoScale = mainLogo.transform.localScale;

        StartCoroutine(firstUION());
    }

    // Update is called once per frame
    void Update()
    {
        //01.
        //mainLogo Scale : 좌우로 스트레치 되었다가 원래
        //mainLogo Opacity: dissolve

        DOTween.To(() => mainLogoScale, x => mainLogoScale = x, Vector3.one * 3, 2);
        mainLogo.transform.localScale = mainLogoScale;

        
        //DOTween.To(() => mainLogoOpacity, x => mainLogoOpacity = x, 225, 1);

        //mainLogo.transform.DOScale(Vector3.one * 3, 1f);

        //02.
        //subLogo Opacity: dissolve

        //왜 할당되지 않는가!
        //DOTween.To(() => subLogoOpacity, x => subLogoOpacity = x, 225, 1);
        //정답찾기

        //subLogoOpacity.DOColor(Color(), 1);


        //03.
        //BG: dissolve 
        //BG: scale 커지게
        Vector3 BG = bg.transform.localScale;
        float bgOpacity = bg.gameObject.GetComponent<Image>().color.a;
        bgOpacity = 0;

        DOTween.To(() => bgOpacity, x => bgOpacity = x, 1, 1);

        //04.
        //PressAnykey 깜빡이기
        if (first.activeSelf && Input.anyKeyDown)
        {
            first.SetActive(false);
            Second.SetActive(true);
        }
    }


    IEnumerator firstUION()
    {
        yield return new WaitForSeconds(0.3f);
        //첫번째를 키고
        first.SetActive(true);

            
    } 

    IEnumerator secondUION()
    {
        if (!Second.activeSelf)
        {
            //두번째를 킨다
            Second.SetActive(true);
            yield return null;
        }
       
    }
}
