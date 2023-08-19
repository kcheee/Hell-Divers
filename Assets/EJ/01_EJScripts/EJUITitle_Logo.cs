using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EJUITitle_Logo : MonoBehaviour
{
    public GameObject mainLogo;
    public GameObject minLogo;
    public GameObject bg;
    public GameObject PressAnyKeyText;

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
    }

    // Update is called once per frame
    void Update()
    {
        //01.
        //mainLogo Scale : �¿�� ��Ʈ��ġ �Ǿ��ٰ� ����
        //mainLogo Opacity: dissolve

        DOTween.To(() => mainLogoScale, x => mainLogoScale = x, Vector3.one * 3, 1);
        mainLogo.transform.localScale = mainLogoScale;
        //DOTween.To(() => mainLogoOpacity, x => mainLogoOpacity = x, 225, 1);

        //mainLogo.transform.DOScale(Vector3.one * 3, 1f);

        //02.
        //subLogo Opacity: dissolve

        //�� �Ҵ���� �ʴ°�!
        //DOTween.To(() => subLogoOpacity, x => subLogoOpacity = x, 225, 1);
        Debug.Log(subLogoOpacity);
        //����ã��

        //subLogoOpacity.DOColor(Color(), 1);


        //03.
        //BG: dissolve 
        //BG: scale Ŀ����
        Vector3 BG = bg.transform.localScale;
        float bgOpacity = bg.gameObject.GetComponent<Image>().color.a;
        bgOpacity = 0;

        DOTween.To(() => bgOpacity, x => bgOpacity = x, 1, 1);

        //04.
        //PressAnykey �����̱�
        
    }
}
