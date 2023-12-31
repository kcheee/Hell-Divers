using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using TMPro;
using Photon.Pun;
using Unity.VisualScripting;

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
        // 고쳐야함
        //closestObject = GameObject.Find("Player");
        E_state = EnemyState.patrol;
    }

    private void FixedUpdate()
    {
        // 플레이어가 없으면 리턴
        if (PlayerManager.instace.PlayerList.Count == 0)
            return;

        if (photonView.IsMine)
        {
            // 가까운 플레이어 구함.
            closestObject = FindClosestObject();

            // 플레이어와의 거리
            distance = Vector3.Distance(this.transform.position, closestObject.transform.position);
            switch (E_state)
            {
                case EnemyState.patrol: F_patrol(); break;
                case EnemyState.wait: F_wait(); break;
                case EnemyState.chase: F_chase(); break;
                case EnemyState.melee_attack: F_meleeattack(); break;
            }
        }
    }

    protected override void Die()
    {
        transform.GetComponent<Hound>().enabled = false;
        //GetComponent<BoxCollider>().enabled = false;

        base.Die();
        Debug.Log("log");
        StartCoroutine(GetComponent<Die>().delay());
    }


    #region 상태 함수 애니메이션 여기다가.
    protected override void F_chase()
    {
        // 걷는 애니메이션

        agent.speed = 8;

        if(agent != null) 
        agent.isStopped = false;

        // agent야 너의 목적지는 target의 위치야
        agent.destination = closestObject.transform.position;

        // 목적지와 나의 거리를 재고싶다.
        distance = Vector3.Distance(this.transform.position, closestObject.transform.position);


        Vector3 toPlayer = closestObject.transform.position - transform.position;
        toPlayer.y = 0f; // y축 제외

        // 플레이어와 몬스터 사이의 각도 계산
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

        // 시야각 30도 안에 플레이어가 들어오면
        if (angleToPlayer < 30)
        {
            //photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Run",true);
            
            //anim.SetBool("Run", true);
        }
        else
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Run", false);
            //anim.SetBool("Run", false);
            E_state = EnemyState.wait;
        }


        // 만약 추격중에 플레이어와의 거리가 포기거리보다 크다면
        if (distance > ENEMYATTACK.farDistance)
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            agent.speed = 4;
            E_state = EnemyState.patrol;
            // 순찰 상태로 전이하고싶다.
        }

        // 근거리 공격 거리보다 작으면 근거리 공격 상황으로 바꿈.
        if (distance < 5 && angleToPlayer < 25)
        {
            photonView.RPC(nameof(PlayAnimT), RpcTarget.All, "Attack");
            E_state = EnemyState.melee_attack;
        }
    }

    protected override void F_patrol()
    {
        // 걷는 애니메이션
        //anim.SetBool("Walk", true);

        base.F_patrol();
    }
    // 원거리 공격

    protected override void F_wait()
    {

        // agent 멈춤
        StopNavSetting();

        Vector3 toPlayer = closestObject.transform.position - transform.position;
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
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Run", true);
            E_state = EnemyState.chase;
        }
    }
    bool triggerAttack=false;
    // 근거리 공격
    protected override void F_meleeattack()
    {
        // 쿨타임.

        Vector3 toPlayer = closestObject.transform.position - transform.position;
        toPlayer.y = 0f; // y축 제외

        // 플레이어와 몬스터 사이의 각도 계산
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

        // 돌진 공격
        if (angleToPlayer <= 10&&!flag)
        {
            // 1. agent 멈추고
            // 2. 돌진 공격
            // 3. agent 다시 실행.
            StopNavSetting();

            HoundAttack_anim.Hound_anim_falg = false;

            Vector3 targetPosition = transform.position + transform.forward * 8;
            agent.enabled = false;

            // 이때 공격 상황 진행.
            triggerAttack = true;
            transform.DOMove(targetPosition, 1.3f).OnComplete(() => {
                //Debug.Log(agent.transform.position);
                agent.enabled = true;
                // 이 조건문이 한번 실행하기 위한 flag
                flag = false;
                TraceNavSetting();
                photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Run", true);
                triggerAttack=false;
                E_state = EnemyState.chase;

            });

            Debug.Log("이거 한번만 실행되게 해야함.");
            photonView.RPC(nameof(PlayAnimP), RpcTarget.All, "Attack");
            //anim.SetTrigger("Attack");

            // attack 애니메이션이 끝나면
            // 콜라이더 on
            flag = true;
        }
        //else if (angleToPlayer > 10)
        //{
        //    TraceNavSetting();
        //    E_state = EnemyState.chase;
        //}

        // 애니메이션이 진행되고 있으면 움직이면 안됌.
        if (distance > 7&&HoundAttack_anim.Hound_anim_falg)
        {
            agent.enabled = true;
            TraceNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Run", true);
            E_state = EnemyState.chase;
        }

    }


    #endregion

    #region Animation Event

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (triggerAttack)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, other.transform.position, 2);
            }
        }

    }

}
