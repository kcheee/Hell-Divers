
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EJBombFire : MonoBehaviourPun
{
    #region bomb����
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
        //���� ȸ�� �� ��Ƶα�
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
    //Bomb�� ����, bomb��ũ��Ʈ���� ���� ���Ѵ�.
    public IEnumerator MakeBomb(System.Action<int> complete)
    {
        isBombDone = false;

        for (int i = 0; i < bombCount; i++)
        {
            //bomb ����
            bomb = EJObjectPoolMgr.instance.GetbombQueue();

            //**photonView
            //photonView�� ���� boss���� bomb�� ������ �Լ����� �ۼ��ϰ� ������ �� ����

            //bomb�� boss���� Instantiate�ǰ�, PhotonNetwork.Instantiate�� �ƴϱ� ������ photonView�� ������ �����Ǵ��� �𸥴�.
            //�׷��� ������ �����Ǵ� Bomb�� ������ Boss�� photonView�� �ٿ��ش�.
            //master������ bomb ���� �� �����ϰ� �̸� �� PC���� �����ֱ� ���ؼ�

            bomb.GetComponent<EJBomb>().tankPv = photonView;

            if (bomb != null)
            {
                //bomb ����
                bomb.transform.position = bombPos.position;
                bomb.transform.forward = bombPos.transform.forward;

                EJBossSFX.instance.PlaybombFlyingSFX();
                ONHeadAnim();
                ONBodyAnim();

                //bombMuzzle ����
                GameObject bombMuzzleImpact = Instantiate(bombMuzzleFactory);

                bombMuzzleImpact.transform.position = bombPos.position;
                bombMuzzleImpact.transform.localEulerAngles = bombPos.transform.parent.localEulerAngles;
                bombMuzzleImpact.transform.localScale = Vector3.one;
                bombMuzzleImpact.transform.forward = bombPos.transform.forward;


                //bombPos ȸ��
                bombPosX += Random.Range(1f,3f);
                bombPosZ += Random.Range(-3f, 2f);

                bombPos.Rotate(new Vector3(bombPosX,0,bombPosZ) + originBombAngle, Space.Self);

            }

            //��Ÿ��
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

    #region Animation�Լ���
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
    //���� Bomb Script���� ���������� �ϴ� �Լ����� RPC�� ������ ���ؼ�
    //Boss�� �پ��ִ� Script���ٰ� �ۼ�����.

    #region RPC) bomb �ٴ� �浹 FX
    [PunRPC]
    //bomb�� �ٴڰ� �浹���� �� ������ ����
    void ShowBombExploImpact(Vector3 pos, Vector3 normal, float waitTime)
    {
        GameObject bomExploImpact = EJObjectPoolMgr.instance.GetbombExploImpactQueue();

        bomExploImpact.transform.position = pos;
        bomExploImpact.transform.localScale = Vector3.one * 3;
        bomExploImpact.transform.forward = normal;

        StartCoroutine(wait(bomExploImpact, waitTime));
    }

    //bombFX�߻� �� �ٽ� ť�� �־��ֱ� ���� ��ٸ��� ��
    IEnumerator wait(GameObject bomb, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EJObjectPoolMgr.instance.ReturnbombExploImpactQueue(bomb);
    }
    #endregion
}