using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using TMPro;

public class Hound : Enemy_Fun
{

    #region 싱글톤
    static public Hound instance;
    private void Awake()
    {
        instance = this;
        getcomponent();
    }
    #endregion

    // 각종 변수
    float currrTime = 0;
    bool flag = false;

    #region 컴포넌트 가져옴
    // 컴포넌트 가져오기
    void getcomponent()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    #endregion

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
            case EnemyState.patrol: F_patrol(); break;
            case EnemyState.wait: F_wait(); break;
            case EnemyState.chase: F_chase(); break;
            case EnemyState.melee_attack: F_meleeattack(); break;

        }
    }


    #region 상태 함수 애니메이션 여기다가.
    protected override void F_chase()
    {
        // 걷는 애니메이션

        agent.speed = 8;

        if(agent != null) 
        agent.isStopped = false;

        // agent야 너의 목적지는 target의 위치야
        agent.destination = target.transform.position;

        // 목적지와 나의 거리를 재고싶다.
        distance = Vector3.Distance(this.transform.position, target.transform.position);


        Vector3 toPlayer = target.transform.position - transform.position;
        toPlayer.y = 0f; // y축 제외

        // 플레이어와 몬스터 사이의 각도 계산
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

        // 시야각 30도 안에 플레이어가 들어오면
        if (angleToPlayer < 30)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
            E_state = EnemyState.wait;
        }


        // 만약 추격중에 플레이어와의 거리가 포기거리보다 크다면
        if (distance > ENEMYATTACK.farDistance)
        {
            E_state = EnemyState.patrol;
            // 순찰 상태로 전이하고싶다.
        }

        // 근거리 공격 거리보다 작으면 근거리 공격 상황으로 바꿈.
        if (distance < 5 && angleToPlayer < 25)
        {
            E_state = EnemyState.melee_attack;
        }
    }

    protected override void F_patrol()
    {
        // 걷는 애니메이션
        anim.SetBool("Walk", true);
        agent.speed = 4;
        base.F_patrol();
    }
    // 원거리 공격

    protected override void F_wait()
    {

        // agent 멈춤
        StopNavSetting();

        Vector3 toPlayer = target.transform.position - transform.position;
        toPlayer.y = 0f; // y축 제외

        // 플레이어와 몬스터 사이의 각도 계산
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

        f_rotation();

        //Vector3.Lerp(transform.forward, target.transform.position - transform.position, 0.2f);

        // 플레이어와 각도가 비슷하면 agent다시 enable
        if (angleToPlayer < 10)
        {
            // agent 다시 실행.
            TraceNavSetting();
            E_state = EnemyState.chase;
        }
    }

    // 근거리 공격
    protected override void F_meleeattack()
    {

        Vector3 toPlayer = target.transform.position - transform.position;
        toPlayer.y = 0f; // y축 제외

        // 플레이어와 몬스터 사이의 각도 계산
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

        // 돌진 공격
        if (angleToPlayer < 10&&!flag)
        {
            // 1. agent 멈추고
            // 2. 돌진 공격
            // 3. agent 다시 실행.
            StopNavSetting();

            Debug.Log(transform.position);
            Vector3 targetPosition = transform.position + transform.forward * 8;
            agent.enabled = false;

            transform.DOMove(targetPosition, 1).OnComplete(() => {
                //Debug.Log(agent.transform.position);
                agent.enabled = true;
                // 이 조건문이 한번 실행하기 위한 flag
                flag = false;
                TraceNavSetting();
                E_state = EnemyState.chase;

            });

            Debug.Log("실행");
            anim.SetTrigger("Attack");
            // 콜라이더 on
            flag = true;
        }

        if(distance > 5)
        {
            agent.enabled = true;
            TraceNavSetting();

            E_state = EnemyState.chase;
        }

    }


    #endregion

    #region Animation Event

    #endregion



}
