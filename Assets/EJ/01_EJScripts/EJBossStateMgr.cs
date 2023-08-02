using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBossStateMgr : MonoBehaviour
{
    public enum State
    {
        Idle,              
        Chase,              
        Attack_Fire,
        Attack_Bomb,
        Attack_Canon,
        Attack_Flareself,
        Die
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(State s)
    {
        if (state == s) return;

        state = s;

        switch (state)
        {
            case State.Idle:
                break;
            case State.Chase:
                break;
            case State.Attack_Fire:
                break;
            case State.Attack_Bomb:
                break;
            case State.Attack_Canon:
                break;
            case State.Attack_Flareself:
                break;
            case State.Die:
                break;

        }
    }
}
