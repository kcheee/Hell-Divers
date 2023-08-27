using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;

public class Enemy1 : Enemy_Fun
{
    #region sigleton
    static public Enemy1 Instance;
    private void Awake()
    {
        Instance = this;
        getcomponent();
    }
    #endregion

    // 총과 final_IK_enable
    private void OnEnable()
    {
        Gun.SetActive(true);
        //gameObject.GetComponent<FullBodyBipedIK>().enabled = true;
    }

    float currrTime = 0;

    public GameObject Gun;
    public GameObject I_bullet;
    public GameObject I_Muzzle;
    public GameObject I_FirePos;

    bool flag = false;

    AudioSource audioSource;
    // 컴포넌트 
    void getcomponent()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //target = GameObject.FindWithTag("Player");
        photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
        E_state = EnemyState.patrol;
        
    }

    private void FixedUpdate()
    {
        // 플레이어가 없으면 리턴
        if (PlayerManager.instace.PlayerList.Count == 0)
            return;

        if(Input.GetKeyDown(KeyCode.M))
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
        }

        if (photonView.IsMine)
        {
            closestObject = FindClosestObject();
            //Debug.Log(closestObject.position);

            distance = Vector3.Distance(this.transform.position, closestObject.transform.position);
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
    }

    // 원거리 공격.
    [PunRPC]
    public void pun_I_RangedAttack()
    {
        StartCoroutine(I_RangedAttack());
    }

    [PunRPC]
    IEnumerator I_RangedAttack()
    {      
        flag = true;
        yield return new WaitForSeconds(0.2f);
        Vector3 pos = new Vector3(transform.position.x, -1.2f, transform.position.z);
        float T = 3;
        for (int i = 0; i < 10; i++)
        {
            T += Random.Range(0.8f, 2f);
            audioSource.Play();

            Vector3 bulletpos = pos + transform.forward * T + transform.right * Random.Range(-1f, 1f);
            GameObject bullet = Instantiate(I_bullet, I_FirePos.transform.position, Quaternion.identity);
            Instantiate(I_Muzzle, I_FirePos.transform.position, Quaternion.identity).transform.parent = transform;

            bullet.transform.parent = transform;

            //Debug.Log(bullet.transform.position);

            //bullet.transform.forward = TsetG.transform.position - transform.position;
            // 밑방향으로 힘을 줘야함.
            //Debug.Log(bulletpos - transform.position);
            bullet.GetComponent<Rigidbody>().AddForce((bulletpos - transform.position) * 8, ForceMode.Impulse);

            yield return new WaitForSeconds(0.15f);
        }
        photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "RAtk", false);
    }


    #region 상태 함수 애니메이션 여기다가.
    protected void F_idle()
    {

    }

    protected override void F_chase()
    {
        // 걷는 애니메이션
        //anim.Play("Walk");

        // 한번만 실행되게 수정
        //photonView.RPC(nameof(PlayAnim_T), RpcTarget.All,"Walk");


        agent.isStopped = false;

        // agent야 너의 목적지는 target의 위치야
        if(agent.destination!=null)
        agent.destination = closestObject.transform.position;

        // 목적지와 나의 거리를 재고싶다.
        distance = Vector3.Distance(this.transform.position, closestObject.transform.position);

        // 원거리 공격보다 작고 근거리 공격 거리보다 크면 wait로
        if (distance < ENEMYATTACK.ranged_attack_possible &&
            distance > ENEMYATTACK.melee_attack_possible)
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All,"Walk", false);
            // 공격상태로 전이하고싶다.
            E_state = EnemyState.wait;
            //anim.SetTrigger("Attack");
            // agent stop
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
        // 만약 추격중에 플레이어와의 거리가 포기거리보다 크다면
        else if (distance > ENEMYATTACK.farDistance)
        {
            E_state = EnemyState.patrol;
            // 순찰 상태로 전이하고싶다.
        }

        // 근거리 공격 거리보다 작으면 근거리 공격 상황으로 바꿈.
        if (distance < 3)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            E_state = EnemyState.melee_attack;
        }
    }

    protected override void F_patrol()
    {
        // 걷는 애니메이션

        //anim.Play("Walk");

        base.F_patrol();
    }
    // 원거리 공격
    protected override void F_rangedattack()
    {
        // 원겨리 공격
        base.F_rangedattack();

        //anim.Play("Ranged_Attack");
        //anim.SetTrigger("Ranged_Attack");
        if(!flag)
        {
            photonView.RPC(nameof(pun_I_RangedAttack), RpcTarget.All);
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "RAtk", true);


            //StartCoroutine(I_RangedAttack());
        }
        currrTime += Time.deltaTime;
        if (currrTime > 2.5f)
        {
            equip_flag = false;
            flag = false;
            currrTime = 0;
            

            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            E_state = EnemyState.chase;
        }

        // 총 쏘기.. 
        // 밑에서 위로 쏘는 형식.
    }

    bool equip_flag= false;
    protected override void F_wait()
    {
        // 장전 애니메이션
        //anim.SetBool("walk", false);
        if (!equip_flag)
        {         
            photonView.RPC(nameof(PlayAnimP), RpcTarget.All, "Equip");

            //anim.SetTrigger("equip");
            equip_flag = true;
        }

        // Enemy 앞방향 Player를 향하게 설정.
        //transform.forward = target.transform.position - transform.position;

        // 총을 꺼내는 시간.
        //Debug.Log("총 꺼냄.");
        transform.forward = closestObject.transform.position - transform.position;
        // 총 꺼내는 애니메이션

        // 일정 시간이 지나면 F_rangedattack으로
        //if(currTime > 2)
        //{
        //    currTime = 0;
        //    E_state = EnemyState.ranged_attack;        
        //}

        transform.forward = closestObject.transform.position - transform.position;

        // 애니메이션 취소 후 근거리 공격을 바꿈.
        // 일정시간 안에 플레이어가 일정 거리 오면 chase 하고 근거리 공격으로 바꿈.
        // 근거리 공격하러 쫒아감.
        if (distance < 5)
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All,"Walk", true);
            equip_flag = !equip_flag;
            E_state = EnemyState.chase;
        }

        // 공격 거리 벗어났을 경우
        if (distance > ENEMYATTACK.attackRange)
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All,"Walk", true);
            equip_flag = !equip_flag;
            E_state = EnemyState.chase;

        }

    }

    // 근거리 공격
    protected override void F_meleeattack()
    {
        // 근거리 공격
        //anim.Play("Melee_Attack");

        // 애니메이션 수정해야함.
        if (!flag)
        {
        photonView.RPC(nameof(PlayAnimP), RpcTarget.All, "Melee_Attack");
            flag= true;
        }

        currrTime += Time.deltaTime;
        // 테스트용
        if (currrTime > 2)
        {
            currrTime = 0;
            flag = false;
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            E_state = EnemyState.chase;
        }
    }

    protected override void Die()
    {
        transform.GetComponent<Enemy1>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        base.Die();
        Debug.Log("log");
        StartCoroutine(GetComponent<Die>().delay());
    }

    #endregion

    #region Animation Event
    // 애니메이션 event
    public void equip_end()
    {
        E_state = EnemyState.ranged_attack;
        //Debug.Log("실행");
    }

    #endregion
}
