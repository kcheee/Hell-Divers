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

    //Player���� �Ÿ�
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

        //�� chase���� �ȵ�����?
        B_state = BossState.Chase;

        //���� ���� ��Ƶα�
        headAxisOriginal = transform.localEulerAngles;

    }

    // Update is called once per frame
    void Update()
    {
        DistanceBoss2Player = Vector3.Distance(transform.position, player.transform.position);
        //�����̴� player�� �ٶ󺸰� �ؾ� �Ѵ�.
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

    //�� ���¸� �ٸ� ���·� �Ѿ �� ���� �ϰ� ����
    private void UpdateRotate2Player()
    {
        //transform.LookAt(player.transform.position);

            //player�� ������ �Ÿ�
            Vector3 LookingPlayerDir = player.transform.position - transform.position;
            //�ٽ� �Ѿư���, �����ϵ� �÷��̾ ã�Ƽ� �ѱ��� ȸ���ϴ� ����
            transform.forward = Vector3.Lerp(transform.forward, LookingPlayerDir, 0.7f * Time.deltaTime  );
    }


    private void UpdateChase()
    {
        OnNavMesh();
        nav.destination = player.transform.position;
        OnWheelMesh();

        //���ݰ��ɹ��� ������ ������ Attack
        if (DistanceBoss2Player <= NoAttackDistance)
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

    //��Ÿ���� �ɾ�ΰ� ������ �ɾ���� ���� �ٸ��� �߻�Ǵ� ����
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

        //���� �������� ����� Chase���
        if (DistanceBoss2Player > NoAttackDistance)
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
        OffNavMesh();
        OffWheelMesh();

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

    //�̰��� �ʿ��� ���ΰ�?
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


