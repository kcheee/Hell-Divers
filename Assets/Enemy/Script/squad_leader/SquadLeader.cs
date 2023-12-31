using DG.Tweening;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SquadLeader : Enemy_Fun
{

    #region sigleton & 변수
    static public SquadLeader Instance;

    public float escape=15;
    // 공격
    public GameObject Flare;
    public GameObject FirePos;
    public GameObject Granade;
    public GameObject E_Initiate;
    public int spawnPos = 5;
    bool flare_flag = true;
    bool flag = false;
    bool Equip_flag = false;
    float currrTime = 0;

    AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        getcomponent();
    }
    #endregion


    

    // 컴포넌트 
    void getcomponent()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // 고쳐야함
        //closestObject = GameObject.Find("Player");
        photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
        E_state = EnemyState.patrol;
    }

    private void FixedUpdate()
    {
        // 플레이어가 없으면 리턴
        if (PlayerManager.instace.PlayerList.Count == 0)
            return;

        if (photonView.IsMine)
        {
            closestObject = FindClosestObject();

            // 플레이어와의 거리
            distance = Vector3.Distance(this.transform.position, closestObject.transform.position);
            switch (E_state)
            {
                case EnemyState.patrol: F_patrol(); break;
                case EnemyState.wait: F_wait(); break;
                case EnemyState.chase: F_chase(); break;
                case EnemyState.ranged_attack: F_rangedattack(); break;
                case EnemyState.melee_attack: F_meleeattack(); break;
                case EnemyState.escape: F_escape(); break;
            }
        }

    }


    #region 유탄 발사

    // 플레어건 발사
    [PunRPC]
    void pun_Squad_Flare()
    {
        StartCoroutine(Squad_Flare());
    }

    IEnumerator Squad_Flare()
    {
        flare_flag = false;
        anim.SetTrigger("Flare");
        yield return new WaitForSeconds(1.5f); // 임의로

        yield return new WaitForSeconds(1f); // 임의로

        audioSource.PlayOneShot(ENEMY.sound_Normal[0], 1);
        //ENEMY.sound_Normal[0].
        Instantiate(Flare, FirePos.transform.position, Quaternion.identity);

        // 플레어건이 터진 위치 기억

        Vector3 FlarePo = transform.position;
        Quaternion FlareRo = transform.rotation;
        // 애니메이션 실행 후
        yield return new WaitForSeconds(2); // 임의로
        flag = true;
        yield return new WaitForSeconds(4);
        // 쫄따구들 소환.

        //float angle = 360 / spawnPos;
        //Transform tf = gameObject.transform;
        //for (int i = 0; i < spawnPos; i++)
        //{
        //    Vector3 Po = FlarePo + new Vector3(Random.Range(-4f, 4), Random.Range(-4f, 4), Random.Range(-4f, 4));

        //    PhotonNetwork.Instantiate("Initiate_E-main", Po, Quaternion.identity);

        //    yield return new WaitForSeconds(Random.Range(0.2f, 1f));
        //}

    }

    [PunRPC]
    void pun_FireGrenada(Vector3 pos)
    {
        StartCoroutine(FireGrenada(pos));
    }
    IEnumerator FireGrenada(Vector3 POS)
    {
        anim.SetTrigger("Ranged_Attack");
        flag = false;
        yield return new WaitForSeconds(1.1f);
        audioSource.PlayOneShot(ENEMY.sound_Normal[1], 1);
        GameObject Grenda = Instantiate(Granade, FirePos.transform.position, Quaternion.identity);
        Grenda.GetComponent<GranadeLauncher>().value(POS);
    }

    #endregion
    #region 상태함수
    protected override void F_chase()
    {
        // 걷는 애니메이션
        //anim.Play("Walk");

        agent.isStopped = false;

        // agent야 너의 목적지는 target의 위치야
        agent.destination = closestObject.transform.position;

        // 목적지와 나의 거리를 재고싶다.
        distance = Vector3.Distance(this.transform.position, closestObject.transform.position);

        // 원거리 공격보다 작고 근거리 공격 거리보다 크면 wait로
        if (distance < ENEMYATTACK.ranged_attack_possible &&
            distance > ENEMYATTACK.melee_attack_possible)
        {
            // 공격상태로 전이하고싶다.
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", false);

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
    }
    protected override void F_patrol()
    {
        // 걷는 애니메이션
        //anim.Play("Walk");

        base.F_patrol();
    }
    
    protected override void F_wait()
    {

        //anim.Play("Equip");
        
        // 한번만 실행해야함.
        if(!Equip_flag)
        {
            photonView.RPC(nameof(PlayAnimP), RpcTarget.All, "Equip");
            Equip_flag = true;
        }


        // agent 미끄러짐 방지
        StopNavSetting();

        // Enemy 앞방향 Player를 향하게 설정.
        // 회전
        f_rotation();

        //Vector3.Lerp(transform.forward, target.transform.position - transform.position, 0.1f);

        currTime += Time.deltaTime;
        // 총을 꺼내는 시간.
        //Debug.Log("총 꺼냄.");

        // 총 꺼내는 애니메이션

        // 일정 시간이 지나면 F_rangedattack으로
        if (currTime > 2f)
        {
            currTime = 0;
            Equip_flag = false;
            E_state = EnemyState.ranged_attack;
        }


        // 애니메이션 취소 후 근거리 공격을 바꿈.
        // 일정시간 안에 플레이어가 일정 거리 오면 chase 하고 근거리 공격으로 바꿈.
        // 근거리 공격하러 쫒아감.
        if (distance < escape)
        {
            Equip_flag = false;

            // agent 다시 세팅
            TraceNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk",true);
            E_state = EnemyState.escape;
            currTime = 0;
        }

        // 공격 거리 벗어났을 경우
        if (distance > ENEMYATTACK.attackRange)
        {
            Equip_flag = false;
            TraceNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            E_state = EnemyState.chase;
            currTime = 0;
        }

    }
    protected void F_escape()
    {
        //anim.Play("Walk");

        // 도망가기 위한 코드
        Vector3 fleeDirection = transform.position - closestObject.transform.position;
        Vector3 targetPosition = transform.position + fleeDirection.normalized * 10;

        NavMeshHit hit;
        // 도망침
        if (NavMesh.SamplePosition(targetPosition, out hit, 10, NavMesh.AllAreas))
        {
            agent.destination = hit.position;

        }
        if (distance >= ENEMYATTACK.ranged_attack_possible )
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", false);
            E_state = EnemyState.wait;
        }
        if (distance < 3)
        {
            StopNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", false);
            photonView.RPC(nameof(PlayAnimT), RpcTarget.All, "MeleeAttack");
            E_state = EnemyState.melee_attack;
        }
        //break;
    }

    protected override void F_rangedattack()
    {
        // 원겨리 공격
        base.F_rangedattack();
        agent.velocity = Vector3.zero;

        f_rotation();

        // 플레어 건 | 한번만 실행하기 위한 코드
        if (flare_flag)
        {
            photonView.RPC(nameof(pun_Squad_Flare), RpcTarget.All);

            //StartCoroutine(Squad_Flare());
        }

        // 유탄 발사.
        if (flag && !flare_flag)
        {
            // 유탄발사
            //StartCoroutine(FireGrenada());

            photonView.RPC(nameof(pun_FireGrenada), RpcTarget.All,closestObject.position);

        }

        //anim.Play("Flare");

        currrTime += Time.deltaTime;
        if (currrTime > 2.5f)
        {
            flag = true;
            currrTime = 0;
            TraceNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            E_state = EnemyState.chase;
        }

        // 총 쏘기.. 
        // 밑에서 위로 쏘는 형식.
    }
    protected override void F_meleeattack()
    {
        currrTime += Time.deltaTime;
        f_rotation();

        if (transform.forward != (closestObject.transform.position - transform.position))
        {
            f_rotation();
            //Debug.Log(transform.rotation + " " + newRotation);
        }
        else
        {
            Debug.Log("tlfgo");
        }
        // 근접공격 코드 아직 안짬. 애니메이션 없음
        Debug.Log("근접공격");

        if (distance < 3 && currrTime > 2.5f)
        {
            currrTime = 0;
            photonView.RPC(nameof(PlayAnimT), RpcTarget.All, "MeleeAttack");

        }
        if (distance > 3)
        {
            TraceNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            E_state = EnemyState.escape;
        }
        // escape 설정해야함.
    }
    #endregion

    protected override void Die()
    {
        transform.GetComponent<SquadLeader>().enabled = false;
        //GetComponent<BoxCollider>().enabled = false;

        base.Die();
        Debug.Log("log");
        StartCoroutine(GetComponent<Die>().delay());
    }

    #region animation event

    public void equip_end()
    {
        //E_state = EnemyState.ranged_attack;
    }

    #endregion

}


