
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EJBombFire : MonoBehaviourPun
{
    #region bomb변수
    //bomb
    bool isBombDone = true;
    int bombCount = 6;

    //bombPos
    public Transform bombPos;
    float bombPosX;
    float bombPosZ;
    Vector3 originBombAngle;

    //bombPrefab
    GameObject bomb;

    //bombMuzzleFX
    GameObject bombMuzzleImpact;
    public GameObject bombMuzzleFactory;

    //bombReactionAnim
    public GameObject bombHead;

    //RigidBody
    Rigidbody rb;

    #endregion
    void Start()
    {
        //원래 회전 값 담아두기
        originBombAngle = bombPos.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        //test
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (isBombDone)
            {
                StartCoroutine(MakeBomb(null));
            }
        }
    }

    #region MakeBomb
    //Bomb를 생성, bomb스크립트에서 힘을 가한다.
    public IEnumerator MakeBomb(System.Action<int> complete)
    {
        isBombDone = false;

        for (int i = 0; i < bombCount; i++)
        {
            //bomb 생성
            bomb = EJObjectPoolMgr.instance.GetbombQueue();

            //**photonView
            //photonView가 붙은 boss에서 bomb가 동작할 함수들을 작성하고 가져다 쓸 예정

            //bomb는 boss에서 Instantiate되고, PhotonNetwork.Instantiate가 아니기 때문에 photonView의 주인이 누가되는지 모른다.
            //그렇기 때문에 생성되는 Bomb에 강제로 Boss의 photonView를 붙여준다.
            //master에서만 bomb 생성 후 공격하고 이를 각 PC에서 보여주기 위해서

            bomb.GetComponent<EJBomb>().tankPv = photonView;

            if (bomb != null)
            {
                //bomb 생성
                bomb.transform.position = bombPos.position;
                bomb.transform.forward = bombPos.transform.forward;

                EJBossSFX.instance.PlaybombFlyingSFX();
                ONHeadAnim();
                ONBodyAnim();

                //bombMuzzle 생성
                GameObject bombMuzzleImpact = Instantiate(bombMuzzleFactory);

                bombMuzzleImpact.transform.position = bombPos.position;
                bombMuzzleImpact.transform.localEulerAngles = bombPos.transform.parent.localEulerAngles;
                bombMuzzleImpact.transform.localScale = Vector3.one;
                bombMuzzleImpact.transform.forward = bombPos.transform.forward;


                //bombPos 회전
                bombPosX += Random.Range(1f,3f);
                bombPosZ += Random.Range(-3f, 2f);

                bombPos.Rotate(new Vector3(bombPosX,0,bombPosZ) + originBombAngle, Space.Self);

            }

            //쿨타임
            yield return new WaitForSeconds(0.5f);
            //BossFSM.Sflag = false;
        }

        bombPosX = 0;
        bombPosZ = 0;
        isBombDone = true;

        if(complete != null)
        {
            complete(0);
        }
    }
    #endregion

    #region Animation함수들
    //bodyReactionAnim
    public Animator headReaction;
    public Animator bodyReaction;

    public void ONHeadAnim()
    {
        headReaction.SetTrigger("headFire");
    }

    public void ONBodyAnim()
    {
        bodyReaction.SetTrigger("HeadReaction");
    }
    #endregion

    //***photonView
    //실제 Bomb Script에서 실행시켜줘야 하는 함수지만 RPC를 던지기 위해서
    //Boss에 붙어있는 Script에다가 작성해줌.

    #region RPC) bomb 바닥 충돌 FX
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
    #endregion
}