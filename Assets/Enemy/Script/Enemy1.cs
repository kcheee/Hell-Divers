using GLTF.Schema;
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
        getcomponent();
    }
    #endregion

    float currrTime=0;

    // 컴포넌트 
    void getcomponent()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        target = GameObject.Find("Player");
        E_state = EnemyState.patrol;
    }

    private void Update()
    {
        // 플레이어와의 거리
        distance = Vector3.Distance(this.transform.position, target.transform.position);
        switch (E_state)
        {
            case EnemyState.idle: F_idle(); break;
            case EnemyState.patrol: F_patrol(); break;
            case EnemyState.wait: F_wait(); break;
            case EnemyState.chase: F_chase(); break;
            case EnemyState.ranged_attack: F_rangedattack(); break;
            case EnemyState.melee_attack: F_meleeattack(); break;
        }

    }


    #region 상태 함수 애니메이션 여기다가.
    protected void F_idle()
    {
    }

    protected override void F_chase()
    {
        // 걷는 애니메이션
        anim.SetBool("walk", true);
        base.F_chase();
    }

    protected override void F_patrol()
    {
        // 걷는 애니메이션
        anim.Play("Walk");

        base.F_patrol();
    }
    // 원거리 공격
    protected override void F_rangedattack()
    {
        // 원겨리 공격
        base.F_rangedattack();

        currrTime += Time.deltaTime;
        if(currrTime>2)
        {
            currrTime = 0;
            E_state = EnemyState.chase;
        }

        // 총 쏘기.. 

    }

    protected override void F_wait()
    {
        
        // 장전 애니메이션
        anim.SetBool("walk", false);
        anim.Play("Equip");

        base.F_wait();
    }

    #endregion

    // 애니메이션 event
    public void equip_end()
    {
        E_state = EnemyState.ranged_attack;
        Debug.Log("실행");
    }
}
