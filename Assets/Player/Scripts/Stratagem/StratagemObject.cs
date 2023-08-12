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

    //자식한테 활성화를 보내겠죠? 
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
        Debug.Log("Add 함수를 실행했다.");
    }

    public void I_Destroy()
    {
        Debug.Log("Destroy 함수를 실행했다.");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("이것이 부모다");
    }
}
