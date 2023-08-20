
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EJBombFire : MonoBehaviourPun
{
    //bomb
    bool isBombDone = true;
    int bombCount = 10;

    //bombPos
    public Transform bombPos;
    Vector3 originBombAngle;
    GameObject bomb;
    GameObject bombMuzzleImpact;
    public GameObject bombHead;

    //bombMuzzleFX
    public GameObject bombMuzzleFactory;

    //bombReaction

    // Start is called before the first frame update
    void Start()
    {
        originBombAngle = bombPos.localEulerAngles;
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

    [PunRPC]
    public IEnumerator MakeBomb(System.Action<int> complete)
    {
        isBombDone = false;

        for (int i = 0; i < bombCount; i++)
        {

            //bomb 생성
            bomb = EJObjectPoolMgr.instance.GetbombQueue();

            if (bomb != null)
            {
                bomb.transform.position = bombPos.position;
                bomb.transform.up = bombPos.transform.up;

                EJBossSFX.instance.PlaybombFlyingSFX();
                ONHeadAnim();
                ONBodyAnim();

                //bombMuzzle 생성
                GameObject bombMuzzleImpact = Instantiate(bombMuzzleFactory);

                bombMuzzleImpact.transform.position = bombPos.position;
                bombMuzzleImpact.transform.localEulerAngles = bombPos.transform.parent.localEulerAngles;
                bombMuzzleImpact.transform.localScale = Vector3.one;
                bombMuzzleImpact.transform.up = bombPos.transform.forward;
            }
            

            //쿨타임
            yield return new WaitForSeconds(Random.Range(0.1f,0.3f));
            BossFSM.Sflag = false;
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

}