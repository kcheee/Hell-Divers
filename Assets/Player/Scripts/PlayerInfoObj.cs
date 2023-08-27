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
    public Image EffectChanel;

    public Transform Stratagem_Panel;
    public List<StratagemUICode> codes = new List<StratagemUICode>();
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
                realcolor, Color.red, 15, (Color color) => {
                    //�÷��� �� ����.
                    OutLineAmmoImg.color = color;
                    AmmoText.color = color;
                }
            );
            //���� ����� �ٲ��
            yield return ChangeColor(OutLineAmmoImg.color, realcolor, 3, (Color color) => {
                OutLineAmmoImg.color = color;
                AmmoText.color = color;
            }
            );
        }
    }

    public IEnumerator ChangedAmmo(Color defaultColor) {

            //�÷��� �����ϰ�
            yield return ChangeColor(defaultColor, new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0.7f),15, (Color color) => {
                EffectChanel.color = color;
            });
            //�ٽ� ���ƿ���
            yield return ChangeColor(EffectChanel.color, defaultColor, 5, (Color color) =>
            {
                EffectChanel.color = color;
            });
        EffectChanel.color = defaultColor;
    }
    public void resetcolor() {
        OutLineAmmoImg.color = realcolor;
        AmmoText.color = realcolor;
    }

    //� Ŭ�������� �𸣰���. 
    //�׳� ��������Ʈ�� �÷��� �޴� �Լ��� �����,
    IEnumerator ChangeColor(Color color, Color newColor, float force, System.Action<Color> action) {
        //Color�� ����ü ���ӿ�����Ʈ�� Ŭ����
        for (float t = 0; t < 1; t += Time.deltaTime * force)
        {
            color = Color.Lerp(color, newColor, t);
            //�÷��� �ٲ������ ���� ���븦 ���Ͻÿ�.
            action(color);
            yield return new WaitForSeconds(Time.deltaTime);


        }

    }


    public IEnumerator ReloadGunUI(float force = 1, System.Action action = null) {
        for (float t = 0; t <= 1; t += Time.deltaTime * force) {
            GunUI.fillAmount = t;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GunUI.fillAmount = 1;
        if (action != null) {
            action();
        }
    }
}
