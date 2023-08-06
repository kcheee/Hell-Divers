using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy1 : Enemy_Fun
{
    #region sigleton
    static public Enemy1 Instance;
    private void Awake()
    {
        Instance= this;
        agent = GetComponent<NavMeshAgent>();
    }
    #endregion

    private void Start()
    {
        target = GameObject.Find("Player");
        E_state = EnemyState.patrol;
    }

    private void Update()
    {
        switch (E_state)
        {
            case EnemyState.idle: F_idle(); break;
            case EnemyState.patrol: F_patrol(); break;
            case EnemyState.wait: F_wait(); break;
            case EnemyState.chase: F_chase(); break;
            case EnemyState.attack1: F_attack1(); break;
        }
        
    }

    protected void F_idle()
    {
    }
    protected override void F_chase()
    {
        base.F_chase();
    }
    protected override void F_patrol()
    {
        base.F_patrol();
    }
    // 근거리 공격
    protected override void F_attack1()
    {
        base.F_attack1();
    }
    protected override void F_wait()
    {
        base.F_wait();
    }
}
