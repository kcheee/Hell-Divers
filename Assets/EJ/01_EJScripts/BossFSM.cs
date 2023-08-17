using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFSM : MonoBehaviour
{
    public static BossFSM instance;

    public enum BossState
    {
        Patrol,
        Chase,
        Attack,
        Wait,
        Die
    }

    public BossState B_state;

    //navigation
    NavMeshAgent nav;
    GameObject player;

    //Animation
    //public Animator anim;

    //Player와의 거리
    public float DistanceBoss2Player;
    public float bombDistanceS = 7.5f;
    public float machineGunDistanceM = 12.5f;
    public float GausCannonDistanceL = 17.5f;
    public float NoAttackDistance = 22.5f;

    //bool
    static public bool Sflag = false;
    static public bool Mflag = false;
    static public bool Lflag = false;
    static public bool XLflag = false;
    bool rotate = false;

    //time
    float curTime = 0;
    float waitTime = 3f;

    //head rotation axis
    public GameObject headAxis;
    Vector3 headAxisOriginal;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        //nav.transform.forward =transform.forward;

        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player);

        //왜 chase부터 안들어오지?
        B_state = BossState.Chase;

        //원래 각도 담아두기
        headAxisOriginal = transform.localEulerAngles;

    }

    // Update is called once per frame
    void Update()
    {
        DistanceBoss2Player = Vector3.Distance(transform.position, player.transform.position);
        //움직이는 player를 바라보게 해야 한다.
        //headAxis.transform.LookAt(player.transform);

        switch (B_state)
        {
            case BossState.Patrol:
                UpdateRotate2Player();
                break;
            case BossState.Chase:
                UpdateChase();
                break;
            case BossState.Wait:
                UpdateWait();
                break;
            case BossState.Attack:
                UpdateAttack();
                break;
            case BossState.Die:
                UpdateDie();
                break;
        }
    }

    public void ChangeState(BossState s)
    {
        if (B_state == s) return;

        B_state = s;

    }

    //한 상태면 다른 상태로 넘어갈 수 없게 하고 싶음
    private void UpdateRotate2Player()
    {
        //transform.LookAt(player.transform.position);

            //player와 나와의 거리
            Vector3 LookingPlayerDir = player.transform.position - transform.position;
            //다시 쫓아가든, 공격하든 플레이어를 찾아서 총구를 회전하는 상태
            transform.forward = Vector3.Lerp(transform.forward, LookingPlayerDir, 0.7f * Time.deltaTime  );
    }


    private void UpdateChase()
    {
        OnNavMesh();
        nav.destination = player.transform.position;
        OnWheelMesh();

        //공격가능범위 안으로 들어오면 Attack
        if (DistanceBoss2Player <= NoAttackDistance)
        {
            print("공격XLDistance에 들어왔어요");
            B_state = BossState.Wait;
        }

        //!!!!!서서히 원래 포지션으로 돌고 싶다.
        //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, headAxisOriginal, 0.7f);
    }

    public void AttackCompleted(int skillIdx)
    {
        if (skillIdx == 0) Sflag = false;
        if (skillIdx == 1) Lflag = false;
        if (skillIdx == 2) XLflag = false;

        //Attack을 마치면 Wait로 바뀌기 전에 바라보는 방향으로 고개를 틀어야 한다.
        //UpdateRotate2Player();

        B_state = BossState.Wait;
        //rotate = true;
    }

    //쿨타임을 걸어두고 앞으로 걸어나가면 공격 다르게 발사되는 상태
    private void UpdateAttack()
    {

        if (DistanceBoss2Player <= bombDistanceS && !Sflag)
        {
            print("MakeBomb");
            StartCoroutine(transform.GetComponent<EJBombFire>().MakeBomb(AttackCompleted));
            Sflag = true;
            //B_state = BossState.Wait;
        }
        else if (DistanceBoss2Player > machineGunDistanceM && DistanceBoss2Player <= GausCannonDistanceL && !Lflag)
        {
            print("MachineGunFire");
            StartCoroutine(GetComponent<EJMachineGun>().MachineGunFire(AttackCompleted));
            Lflag = true;
            //B_state = BossState.Wait;
        }
        else if (DistanceBoss2Player > GausCannonDistanceL && DistanceBoss2Player <= NoAttackDistance && !XLflag)
        {
            print("GausCannonFire");
            StartCoroutine(GetComponent<EJGausCannonFireInstantiate>().CannonFire(AttackCompleted));
            XLflag = true;
            //B_state = BossState.Wait;
        }

        //공격 범위에서 벗어나면 Chase모드
        if (DistanceBoss2Player > NoAttackDistance)
            {
                print("Attack할 수 있는 거리가 아닙니다");
                B_state = BossState.Chase;
                //anim.SetTrigger("Chase");
            }
       // AllFlagFalse();
    }

    private void UpdateDie()
    {
        if (EJBossHP.instance.HP < 0)
            B_state = BossState.Die;
    }

    private void UpdateWait()
    {
        OffNavMesh();
        OffWheelMesh();

        //움직이는 player를 바라보게 해야 한다.
        //headAxis.transform.LookAt(player.transform);
        //UpdatePatrol();
        UpdateRotate2Player();
        
        //player와 나와의 방향을 구해서 Lerp를 사용한다?        
        Vector3 headAxisAngle = headAxis.transform.localEulerAngles;

        //-12이하이면, 막아주고 ?
        if (headAxisAngle.x >= 12)
        {
            headAxisAngle.x = 12;
        }else if(headAxisAngle.x<=-8)
        {
            headAxisAngle.x = -8;
        }

        headAxis.transform.localEulerAngles = headAxisAngle;
     
        curTime += Time.deltaTime;

        if (curTime > waitTime)
        {
            B_state = BossState.Attack;
            curTime = 0;
        }
    }

    void OnNavMesh()
    {
        this.nav.isStopped = false;
        this.nav.updatePosition = true;
        this.nav.updateRotation = true;
    }

    void OffNavMesh()
    {
        this.nav.velocity = Vector3.zero;
        this.nav.isStopped = true;
        this.nav.updatePosition = false;
        this.nav.updateRotation = false;
    }

    //이것이 필요한 것인가?
    void AllFlagFalse()
    {
        Sflag = false;
        Mflag = false;
        Lflag = false;
        XLflag = false;
    }

    void OnWheelMesh()
    {
        GetComponent<EJWheel>().enabled = true;
    }

    void OffWheelMesh()
    {
        GetComponent<EJWheel>().enabled = false;
    }
}


