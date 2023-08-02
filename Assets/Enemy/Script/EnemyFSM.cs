using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : EnemyInfo
{

    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Patrol
    }

    public EnemyState state;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnemyState.Idle: UpdateIdle(); break;
            case EnemyState.Chase: UpdateChase(); break;
            case EnemyState.Attack: UpdateAttack(); break;
            case EnemyState.Patrol: UpdatePatrol(); break;
        }
    }

    private void UpdatePatrol()
    {

    }

    private void UpdateAttack()
    {


    }

    private void UpdateChase()
    {


    }

    private void UpdateIdle()
    {


    }
}
