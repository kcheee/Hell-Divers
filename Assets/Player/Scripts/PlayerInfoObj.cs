using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoObj : MonoBehaviour
{
    public Text NameText;
    public Animator anim;
    public Image GunUI;
    public Text AmmoText;
    public Image AmmoImg;
    public Image OutLineAmmoImg;
    // Start is called before the first frame update
    void Start()
    {
        realcolor = OutLineAmmoImg.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    Color realcolor;
    public IEnumerator NoAmmo()
    {
        while (true) {
            //레드로 바뀌기
            yield return ChangeColor(
                realcolor, Color.red ,15, (Color color) => {
                    //컬러로 할 무언가.
                    OutLineAmmoImg.color = color;
                    AmmoText.color = color;
                }
            );
            //원래 색깔로 바뀌기
            yield return ChangeColor(OutLineAmmoImg.color, realcolor,3, (Color color) => {
                    OutLineAmmoImg.color = color;
                    AmmoText.color = color;
                }
            );
        }


    }


    //어떤 클래스인지 모르겠음. 
    //그냥 델리게이트로 컬럴ㄹ 받는 함수를 만든다,
    IEnumerator ChangeColor(Color color,Color newColor,float force, System.Action<Color> action) {
        //Color은 구조체 게임오브젝트는 클래스
        for (float t = 0; t < 1; t += Time.deltaTime * force)
        {
            color = Color.Lerp(color, newColor, t);
            //컬러가 바뀌었으니 무언가 조취를 취하시오.
            action(color);
            yield return new WaitForSeconds(Time.deltaTime);
            

        }

    }


}
