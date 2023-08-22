
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EJBombFire : MonoBehaviourPun
{
    //bomb
    bool isBombDone = true;
    int bombCount = 5;

    //bombPos
    public Transform bombPos;
    float bombPosX;
    float bombPosY;
    float bombPosZ;

    Vector3 originBombAngle;
    GameObject bomb;
    GameObject bombMuzzleImpact;
    public GameObject bombHead;

    //bombMuzzleFX
    public GameObject bombMuzzleFactory;

    //bombReaction

    //RigidBody
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        originBombAngle = bombPos.localEulerAngles;
        rb = GetComponent<Rigidbody>();

        //Vector3 rot = transform.eulerAngles;

        //rot.x += Random.Range(-5, 5);
        //rot.y += Random.Range(-5, 5);
        //rot.z += Random.Range(-5, 5);
        //transform.eulerAngles = rot;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (isBombDone)
            {
                StartCoroutine(MakeBomb(null));
            }
        }
    }

    //Bomb를 생성하고 힘을 가한다.
    public IEnumerator MakeBomb(System.Action<int> complete)
    {
        isBombDone = false;

        for (int i = 0; i < bombCount; i++)
        {
            //bomb 생성
            bomb = EJObjectPoolMgr.instance.GetbombQueue();

            //bomb는 boss에서 Instantiate되고, PhotonNetwork.Instantiate가 아니기 때문에
            //photonView의 주인이 누가되는지 모른다.
            //그렇기 때문에 생성되는 Bomb에 강제로 Boss의 photonView를 붙여준다.
            //master에서만 bomb 생성 후 공격하고 이를 각 PC에서 보여주기 위해서

            bomb.GetComponent<EJBomb>().tankPv = photonView;

            if (bomb != null)
            {
                //bomb 생성
                bomb.transform.position = bombPos.position;
                bomb.transform.forward = bombPos.transform.up;

                EJBossSFX.instance.PlaybombFlyingSFX();
                ONHeadAnim();
                ONBodyAnim();

                //bombMuzzle 생성
                GameObject bombMuzzleImpact = Instantiate(bombMuzzleFactory);

                bombMuzzleImpact.transform.position = bombPos.position;
                bombMuzzleImpact.transform.localEulerAngles = bombPos.transform.parent.localEulerAngles;
                bombMuzzleImpact.transform.localScale = Vector3.one;
                bombMuzzleImpact.transform.up = bombPos.transform.forward;

                //bombPosX += Random.Range(-0.3f, 0.3f);
                bombPosY += Random.Range(-20f, 20f);
                //bombPosZ += Random.Range(-0.3f, 0.3f);

                //forward위치를 Random으로 해둬야함.
                bombPos.Rotate(new Vector3(0,bombPosY,0)+ originBombAngle, Space.Self);

                //생성해서 총알에 AddForce를 주고 싶은데 이렇게 하면 달리나?
                bomb.GetComponent<EJBomb>().rb.AddForce(bombPos.transform.forward * 20, ForceMode.Impulse);

                //rb.AddForce(rot * 20, ForceMode.Impulse);
                //GetComponent<EJBomb>().BulletFire();
            }

            //쿨타임
            yield return new WaitForSeconds(0.5f);
            //BossFSM.Sflag = false;
        }

        isBombDone = true;

        if(complete != null)
        {
            complete(0);
        }
    }

    public Animator headReaction;
    public Animator bodyReaction;

    public void ONHeadAnim()
    {
        headReaction.SetTrigger("headFire");
    }

    //turretHeadAnim
    public void ONBodyAnim()
    {
        bodyReaction.SetTrigger("HeadReaction");
    }


    //실제 Bomb Script에서 실행시켜줘야 하는 함수지만 RPC를 던지기 위해서
    //Boss에 붙어있는 Script에다가 작성해주는 것

    [PunRPC]
    //bomb가 바닥과 충돌했을 때 나오는 연기
    void ShowBombExploImpact(Vector3 pos, Vector3 normal, float waitTime)
    {
        GameObject bomExploImpact = EJObjectPoolMgr.instance.GetbombExploImpactQueue();

        bomExploImpact.transform.position = pos;
        bomExploImpact.transform.localScale = Vector3.one * 3;
        bomExploImpact.transform.forward = normal;

        StartCoroutine(wait(bomExploImpact, waitTime));
    }

    //bombFX발생 후 다시 큐에 넣어주기 위해 기다리는 것
    IEnumerator wait(GameObject bomb, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EJObjectPoolMgr.instance.ReturnbombExploImpactQueue(bomb);
    }
}