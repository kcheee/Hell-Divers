using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


//Pun을 안쓴다면 RPC를 써야 한다?
public class BossFSM : MonoBehaviourPun
{
    public static BossFSM instance;

    public enum BossState
    {
        MakeLittleBoss,
        Chase,
        Attack,
        Wait,
        Die
    }

    public BossState B_state;

    //navigation
    NavMeshAgent nav;


    //가장 가까운 Player을 return
    private Transform closestObject;

    //Animation
    //public Animator anim;

    //Player와의 거리
    public float DistanceBoss2Player;
    public float bombDistanceS = 7.5f;
    public float machineGunDistanceM = 12.5f;
    public float GausCannonDistanceL = 17.5f;
    public float makeLittleBossDistance = 22.5f;
    public float NoAttack_ChaseDistance = 27.5f;

    //bool
    static public bool Sflag = false;
    static public bool Mflag = false;
    static public bool Lflag = false;
    static public bool XLflag = false;
    static public bool XXLflag = false;
    //bool rotate = false;

    //time
    float curTime = 0;
    float waitTime = 3f;

    //head rotation axis
    public GameObject headAxis;
    Vector3 headAxisOriginal;

    //makeLittleBoss 
    public GameObject houndFactory;
    public GameObject turretFactory;
    GameObject[] littleBoss = new GameObject[2];

    bool isHoundDone = false;
    bool isTurretDone = false;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        nav.Warp(transform.position);

        //nav.transform.forward =transform.forward;

        //closest = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(closest);
        //closestObject = FindClosestObject();

        //왜 chase부터 안들어오지?
        B_state = BossState.Chase;

        //원래 각도 담아두기
        headAxisOriginal = transform.localEulerAngles;

        //littleBoss 순서대로 넣고 싶음
        littleBoss = new GameObject[] { houndFactory, turretFactory };
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.instace.PlayerList.Count == 0) return;

        Debug.Log("플레이어는" + closestObject);

        //이러면 photonView가 붙은 Boss를 만든 방장?과의 위치만 잰다?

        //자기 자신이 생성한 것인가? 아닌가?
        //마스터라면
        if (photonView.IsMine)
        {
            Debug.Log("photonViewisMine이 실행되고 있습니다");
            closestObject = FindClosestObject();

            Debug.Log("플레이어는" + closestObject);
            DistanceBoss2Player = Vector3.Distance(transform.position, closestObject.transform.position);


            //움직이는 player를 바라보게 해야 한다.
            //headAxis.transform.LookAt(player.transform);
            switch (B_state)
            {
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
    }

    #region 상태
    public void ChangeState(BossState s)
    {
        if (B_state == s) return;

        B_state = s;

    }

    // State를 바꿀 때마다 RPC를 보낸다.


    public Transform spawnPosGroup;
    public Vector3[] spawnPos;

    [PunRPC]
    private void MakeLittleBoss()
    {
        if (!isHoundDone)
        {
            isHoundDone = true;
            spawnPos = new Vector3[3];

            //hound spawnPos
            float angle = 360 / spawnPos.Length;
            for (int i = 0; i < spawnPos.Length; i++)
            {
                GameObject hound = Instantiate(houndFactory);
                hound.GetComponent<Hound>().E_state = Enemy_Fun.EnemyState.chase;

                spawnPosGroup.Rotate(0, angle, 0);
                spawnPos[i] = spawnPosGroup.position + spawnPosGroup.forward * 2;

                hound.transform.position = spawnPos[i];
            }
        }

        if (!isTurretDone)
        {
            isTurretDone = true;
            spawnPos = new Vector3[1];

            //turret spawnPos
            GameObject turret = Instantiate(turretFactory);

            turret.transform.position = spawnPos[0];
            isTurretDone = false;
            isHoundDone = false;
        }


        //공격 범위에서 벗어나면 Chase모드
        if (DistanceBoss2Player > NoAttack_ChaseDistance)
        {
            print("Attack할 수 있는 거리가 아닙니다");
            B_state = BossState.Chase;
        }

        //공격 범위라면 Attack
        if (DistanceBoss2Player < NoAttack_ChaseDistance)
        {
            B_state = BossState.Attack;
        }
    }

    //한 상태면 다른 상태로 넘어갈 수 없게 하고 싶음
    private void UpdateRotate2Player()
    {
        //transform.LookAt(player.transform.position);
        //player와 나와의 거리
        Vector3 LookingPlayerDir = closestObject.transform.position - transform.position;
        //다시 쫓아가든, 공격하든 플레이어를 찾아서 총구를 회전하는 상태
        transform.forward = Vector3.Lerp(transform.forward, LookingPlayerDir, 0.7f * Time.deltaTime);
    }


    private void UpdateChase()
    {
        Debug.Log(nav.destination);
        nav.destination = closestObject.transform.position;
        //photonView.RPC(nameof(OnWheelMesh), RpcTarget.All);


        //공격가능범위 안으로 들어오면 Attack
        if (DistanceBoss2Player <= NoAttack_ChaseDistance)
        {
            print("공격XLDistance에 들어왔어요");
            OffNavMesh();
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

    [PunRPC]
    void StartMakeBombByRPC()
    {
        StartCoroutine(transform.GetComponent<EJBombFire>().MakeBomb(AttackCompleted));
    }

    [PunRPC]
    void StartGausCannonByRPC()
    {
        StartCoroutine(transform.GetComponent<EJGausCannonFireInstantiate>().CannonFire(AttackCompleted));
    }

    [PunRPC]
    void StartMachineGunByRPC()
    {
        StartCoroutine(transform.GetComponent<EJMachineGun>().MachineGunFire(AttackCompleted));
    }

    //쿨타임을 걸어두고 앞으로 걸어나가면 공격 다르게 발사되는 상태
    private void UpdateAttack()
    {
        //if(!Sflag)
        //{
        //    photonView.RPC(nameof(StartMakeBombByRPC), RpcTarget.All);
        //    //StartCoroutine(transform.GetComponent<>)
        //    Sflag = true;
        //}
       
        if (DistanceBoss2Player <= bombDistanceS && !Sflag)
        {
            print("MakeBomb");
            //StartCoroutine(transform.GetComponent<EJBombFire>().MakeBomb(AttackCompleted));

            photonView.RPC(nameof(StartMakeBombByRPC), RpcTarget.All);
            //StartCoroutine(transform.GetComponent<>)
            Sflag = true;
            //B_state = BossState.Wait;
        }
        else if (DistanceBoss2Player > machineGunDistanceM && DistanceBoss2Player <= GausCannonDistanceL && !Lflag)
        {
            print("MachineGunFire");
            //StartCoroutine(GetComponent<EJMachineGun>().MachineGunFire(AttackCompleted));

            photonView.RPC(nameof(StartMachineGunByRPC), RpcTarget.All);
            Lflag = true;
            //B_state = BossState.Wait;
        }
        else if (DistanceBoss2Player > GausCannonDistanceL && DistanceBoss2Player <= makeLittleBossDistance && !XLflag)
        {
            print("GausCannonFire");
            //StartCoroutine(GetComponent<EJGausCannonFireInstantiate>().CannonFire(AttackCompleted));
            //RPC되어 있는 스크립트로 가져오기
            photonView.RPC(nameof(StartGausCannonByRPC), RpcTarget.All);
            //StartCoroutine(GetComponent<PhotonGausCannon.EJGausCannonFireInstantiate_photon>().CannonFireByRPC(AttackCompleted));
            XLflag = true;
            //B_state = BossState.Wait;
        }
        else if (DistanceBoss2Player > makeLittleBossDistance && DistanceBoss2Player <= NoAttack_ChaseDistance && !XXLflag)
        {
            print("makelittleBoss");
            //photonView.RPC(nameof(MakeLittleBoss), RpcTarget.All);
            XXLflag = true;

            B_state = BossState.Wait;
        }

        //공격 범위에서 벗어나면 Chase모드
        if (DistanceBoss2Player > NoAttack_ChaseDistance)
        {
            print("Attack할 수 있는 거리가 아닙니다");
            OnNavMesh();
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
        //photonView.RPC(nameof(OffNavMesh), RpcTarget.All);
        //photonView.RPC(nameof(OffWheelMesh),RpcTarget.All);

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
        }
        else if (headAxisAngle.x <= -8)
        {
            headAxisAngle.x = -8;
        }

        headAxis.transform.localEulerAngles = headAxisAngle;

        curTime += Time.deltaTime;

        if (curTime > waitTime)
        {
            print("공격 딜레이 시간이 지나고 Attack상태로 접어듭니다");
            B_state = BossState.Attack;
            curTime = 0;
        }
    }
    #endregion

    // 가까운 플레이어 찾는 함수.
    protected Transform FindClosestObject()
    {
        Transform closest = PlayerManager.instace.PlayerList[0].transform;
        float closestDistance = Vector3.Distance(transform.position, closest.position);

        foreach (PlayerTest1 obj in PlayerManager.instace.PlayerList)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closest = obj.transform;
                closestDistance = distance;
            }
        }

        return closest;
    }

    [PunRPC]
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

    [PunRPC]
    void OnWheelMesh()
    {
        GetComponent<EJWheel>().enabled = true;
    }
    [PunRPC]
    void OffWheelMesh()
    {
        GetComponent<EJWheel>().enabled = false;
    }
}


