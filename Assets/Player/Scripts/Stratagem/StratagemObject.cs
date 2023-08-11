using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemObject : MonoBehaviour,I_StratagemObject
{
    protected PlayerTest1 Player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTest1 Pl = other.GetComponent<PlayerTest1>();
            Player = Pl;
            OnActive(Pl);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTest1 Pl = other.GetComponent<PlayerTest1>();
            Player = null;
            OnDeactive(Pl);
        }
    }

    //�ڽ����� Ȱ��ȭ�� ��������? 
    public virtual void OnActive(PlayerTest1 player)
    {
        Debug.Log("Active");
    }

    public virtual void OnDeactive(PlayerTest1 player)
    {
        Debug.Log("Deactive");
    }
    public void Add()
    {
        Debug.Log("Add �Լ��� �����ߴ�.");
    }

    public void I_Destroy()
    {
        Debug.Log("Destroy �Լ��� �����ߴ�.");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("�̰��� �θ��");
    }
}
