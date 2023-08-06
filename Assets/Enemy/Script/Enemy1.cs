using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy1 : Enemy_Fun
{

    private void Start()
    {
        target = GameObject.Find("Player");
        E_state = EnemyState.idle;
    }

    private void Update()
    {
        switch (E_state)
        {
            case EnemyState.idle: F_idle(); break;
            case EnemyState.patrol: F_patrol(); break;
            case EnemyState.chase: F_chase(); break;
            case EnemyState.attack1: F_attack1(); break;
        }
    }

    protected void F_idle()
    {

    }

    protected override void F_chase()
    {
        // 부모 것을 사용
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
}
