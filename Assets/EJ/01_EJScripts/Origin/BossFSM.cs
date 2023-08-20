using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


//Pun�� �Ⱦ��ٸ� RPC�� ��� �Ѵ�?

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


    //���� ����� Player�� return
    private Transform closestObject;

    //Animation
    //public Animator anim;

    //Player���� �Ÿ�
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
    bool rotate = false;

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
        //nav.transform.forward =transform.forward;

        //closest = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(closest);
        //closestObject = FindClosestObject();

        //�� chase���� �ȵ�����?
        B_state = BossState.Chase;

        //���� ���� ��Ƶα�
        headAxisOriginal = transform.localEulerAngles;

        //littleBoss ������� �ְ� ����
        littleBoss = new GameObject[] { houndFactory, turretFactory};
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.instace.PlayerList.Count == 0) return;
        Debug.Log(closestObject);

        if (photonView.IsMine)
        {
            closestObject = FindClosestObject();
            DistanceBoss2Player = Vector3.Distance(transform.position, closestObject.transform.position);

        //�����̴� player�� �ٶ󺸰� �ؾ� �Ѵ�.
        //headAxis.transform.LookAt(player.transform);
        switch (B_state)
        {
            case BossState.MakeLittleBoss:
                MakeLittleBoss();
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
    }

    #region ����
    public void ChangeState(BossState s)
    {
        if (B_state == s) return;

        B_state = s;

    }

    public Transform spawnPosGroup;
    public Vector3[] spawnPos;

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
        
        
        //���� �������� ����� Chase���
        if (DistanceBoss2Player > NoAttack_ChaseDistance)
        {
            print("Attack�� �� �ִ� �Ÿ��� �ƴմϴ�");
            B_state = BossState.Chase;
        }

        //���� ������� Attack
        if (DistanceBoss2Player < NoAttack_ChaseDistance)
        {
            B_state = BossState.Attack;
        }
    }

    //�� ���¸� �ٸ� ���·� �Ѿ �� ���� �ϰ� ����
    private void UpdateRotate2Player()
    {
        //transform.LookAt(player.transform.position);
            //player�� ������ �Ÿ�
            Vector3 LookingPlayerDir = closestObject.transform.position - transform.position;
            //�ٽ� �Ѿư���, �����ϵ� �÷��̾ ã�Ƽ� �ѱ��� ȸ���ϴ� ����
            transform.forward = Vector3.Lerp(transform.forward, LookingPlayerDir, 0.7f * Time.deltaTime  );
    }


    private void UpdateChase()
    {
        photonView.RPC(nameof(OnNavMesh),RpcTarget.All);
      
        nav.destination = closestObject.transform.position;
        photonView.RPC(nameof(OnWheelMesh), RpcTarget.All);


        //���ݰ��ɹ��� ������ ������ Attack
        if (DistanceBoss2Player <= NoAttack_ChaseDistance)
        {
            print("����XLDistance�� ���Ծ��");
            B_state = BossState.Wait;
        }

        //!!!!!������ ���� ���������� ���� �ʹ�.
        //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, headAxisOriginal, 0.7f);
    }

    public void AttackCompleted(int skillIdx)
    {
        if (skillIdx == 0) Sflag = false;
        if (skillIdx == 1) Lflag = false;
        if (skillIdx == 2) XLflag = false;

        //Attack�� ��ġ�� Wait�� �ٲ�� ���� �ٶ󺸴� �������� ���� Ʋ��� �Ѵ�.
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

    //��Ÿ���� �ɾ�ΰ� ������ �ɾ���� ���� �ٸ��� �߻�Ǵ� ����
    private void UpdateAttack()
    {

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
            //RPC�Ǿ� �ִ� ��ũ��Ʈ�� ��������
            photonView.RPC(nameof(StartGausCannonByRPC), RpcTarget.All);
            StartCoroutine(GetComponent<PhotonGausCannon.EJGausCannonFireInstantiate_photon>().CannonFireByRPC(AttackCompleted));
            XLflag = true;
            //B_state = BossState.Wait;
        }
        else if(DistanceBoss2Player >makeLittleBossDistance && DistanceBoss2Player<=NoAttack_ChaseDistance && !XXLflag)
        {
            print("makelittleBoss");
            photonView.RPC(nameof(MakeLittleBoss), RpcTarget.All);
            XXLflag = true;

            B_state = BossState.Wait;
        }

        //���� �������� ����� Chase���
        if (DistanceBoss2Player > NoAttack_ChaseDistance)
            {
                print("Attack�� �� �ִ� �Ÿ��� �ƴմϴ�");
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
        photonView.RPC(nameof(OffNavMesh), RpcTarget.All);
        photonView.RPC(nameof(OffWheelMesh),RpcTarget.All);

        //�����̴� player�� �ٶ󺸰� �ؾ� �Ѵ�.
        //headAxis.transform.LookAt(player.transform);
        //UpdatePatrol();
        UpdateRotate2Player();
        
        //player�� ������ ������ ���ؼ� Lerp�� ����Ѵ�?        
        Vector3 headAxisAngle = headAxis.transform.localEulerAngles;

        //-12�����̸�, �����ְ� ?
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
            print("���� ������ �ð��� ������ Attack���·� �����ϴ�");
             B_state = BossState.Attack;
             curTime = 0;
        }
    }
    #endregion

    // ����� �÷��̾� ã�� �Լ�.
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

    [PunRPC]
    void OffNavMesh()
    {
        this.nav.velocity = Vector3.zero;
        this.nav.isStopped = true;
        this.nav.updatePosition = false;
        this.nav.updateRotation = false;
    }

    //�̰��� �ʿ��� ���ΰ�?
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


