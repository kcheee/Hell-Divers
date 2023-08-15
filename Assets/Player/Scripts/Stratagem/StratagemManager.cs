using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class StratagemManager : MonoBehaviour
{
    //��Ʈ��Ÿ�� 4���� �����ϰ��ִ�!
    static int maxStratagem;
    //�����ϰ� �ִ� ��Ʈ��Ÿ��
    public List<Stratagems> current_Stratagems = new List<Stratagems>();
    //Ȱ��ȭ�� �Ǿ��ִ� ��Ʈ��Ÿ��
    public List<Stratagems> active_Stratagems = new List<Stratagems>();

    public bool Isreturn;
    private void Start()
    {
        //active_Stratagems = current_Stratagems.ToList();
    }
    public Stratagems CompareCode(List<KeyType.Key> list,int index) {
        if (Isreturn) { return null; }

        KeyType.Key key = list[index];
        //���� ������ �ִ� ��Ʈ��Ÿ����� �ڵ尡 
        //���� �Է¹��� �ڵ�� ��ġ�ϴ���
        
        foreach (Stratagems Stratagem in current_Stratagems) { //�̰Ŷ����� �ִ°ſ��� �ؾ��Ѵٴϱ�..�������./

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
                
                //�ε����� ���� �ڵ��� ���̰� ���� �ε����� ������
                //��ų�� ����Ѵ�.
                if (Stratagem.CallCode.Count - 1 == index) {
                    Debug.Log("��ų���!!");
                    //�ε����� �ʱ�ȭ �ϰ�ʹ�.
                    list.Clear();
                    active_Stratagems.Clear();
                    return Stratagem;
                }
            }
            else {
                //�� ��Ȱ��ȭ
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
    }
}
