using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class StratagemManager : MonoBehaviour
{
    //��Ʈ��Ÿ�� 4���� �����ϰ��ִ�!
    static int maxStratagem;
    //�����ϰ� �ִ� ��Ʈ��Ÿ��
    public List<Stratagems> current_Stratagems = new List<Stratagems>();
    //Ȱ��ȭ�� �Ǿ��ִ� ��Ʈ��Ÿ��
    public List<Stratagems> active_Stratagems = new List<Stratagems>();

    public bool Isreturn = false;

    public PlayerInfoObj PlayerInfoUI;
    public GameObject UIObject;

    public System.Action<int,int,bool> active_Action;
    private void Start()
    {
        //active_Stratagems = current_Stratagems.ToList();
        current_Stratagems.ForEach((s) =>
        {
            GameObject go = Instantiate(UIObject, PlayerInfoUI.Stratagem_Panel);
            StratagemUICode Code = go.GetComponent<StratagemUICode>();
            //�ڱ� �ڽ��� �ڵ带 �����ϰ� ����.(����)
            s.myStratagemUI = Code;
            //foreach (KeyType.Key key in s.CallCode)
            //{
            //    ���ڵ忡 ���� �����Ǵ� �̹��� UI
            //    Image img = new Image();
            //    img.sprite = Code.GetTexture(key)
            //    Code.Code_images.Add();
            //}

            //PlayerInfoUI.codes.Add(Code);


            active_Stratagems.Add(s);
        });
          
        

    }
    public Stratagems CompareCode(List<KeyType.Key> list,int index) {
        if (Isreturn) { return null; }

        KeyType.Key key = list[index];
        //���� ������ �ִ� ��Ʈ��Ÿ����� �ڵ尡 
        //���� �Է¹��� �ڵ�� ��ġ�ϴ���
        
        for(int i = 0; i < active_Stratagems.Count; i++) { //�̰Ŷ����� �ִ°ſ��� �ؾ��Ѵٴϱ�..�������./
            Stratagems Stratagem = active_Stratagems[i];
            //�ε����� ũ�� : ������ �ִ�.
            //������ �ε����� �񱳸� ���ص� �׳� �� �������� ������ �����ٸ� 
            if (Stratagem.CallCode.Count -1 < index) {
                continue;
            }
            //���� ���� �Է°��� �ε����� ��Ʈ��Ÿ���� �ڵ�� �Է°��� ����?
            if (Stratagem.CallCode[index] == key)
            {
                //�� Ȱ��ȭ (���߼���)
                if (!active_Stratagems.Contains(Stratagem)) {
                    active_Stratagems.Add(Stratagem);
                }
                
                //PlayerInfoUI.codes[i].Code_images[index].color = Color.red;
                //�ε����� ���� �ڵ��� ���̰� ���� �ε����� ������
                //��ų�� ����Ѵ�.
                Debug.Log(Stratagem.CallCode.Count - 1 + " " + index + 1);
                Stratagem.myStratagemUI.Code_images[index].color = Color.black;
                if (Stratagem.CallCode.Count - 1 == index) {
                    Debug.Log("��ų���!!");
                    //�ε����� �ʱ�ȭ �ϰ�ʹ�.
                    list.Clear();
                    active_Stratagems.Clear();
                    Stratagem.myStratagemUI.ResetColor();
                    return Stratagem;
                }
            }
            else {
                //�� ��Ȱ��ȭ
                Stratagem.myStratagemUI.ResetColor();
                //PlayerInfoUI.codes[i].ResetColor();
                active_Stratagems.Remove(Stratagem);
                Debug.Log("�� ��Ȱ��ȭ");
            }
        }

        //���⼭ �׳� �� ���� �Է������� ��� �˷�����.
        if (active_Stratagems.Count <= 0) {
            Isreturn = true;
        }
        return null;

    }

    public void init() {
        active_Stratagems.Clear();
        Isreturn = false;
        current_Stratagems.ForEach(s =>
            active_Stratagems.Add(s)
        );
    }


}
