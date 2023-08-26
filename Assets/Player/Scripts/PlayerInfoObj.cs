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
            //����� �ٲ��
            yield return ChangeColor(
                realcolor, Color.red ,15, (Color color) => {
                    //�÷��� �� ����.
                    OutLineAmmoImg.color = color;
                    AmmoText.color = color;
                }
            );
            //���� ����� �ٲ��
            yield return ChangeColor(OutLineAmmoImg.color, realcolor,3, (Color color) => {
                    OutLineAmmoImg.color = color;
                    AmmoText.color = color;
                }
            );
        }


    }


    //� Ŭ�������� �𸣰���. 
    //�׳� ��������Ʈ�� �÷��� �޴� �Լ��� �����,
    IEnumerator ChangeColor(Color color,Color newColor,float force, System.Action<Color> action) {
        //Color�� ����ü ���ӿ�����Ʈ�� Ŭ����
        for (float t = 0; t < 1; t += Time.deltaTime * force)
        {
            color = Color.Lerp(color, newColor, t);
            //�÷��� �ٲ������ ���� ���븦 ���Ͻÿ�.
            action(color);
            yield return new WaitForSeconds(Time.deltaTime);
            

        }

    }


}
