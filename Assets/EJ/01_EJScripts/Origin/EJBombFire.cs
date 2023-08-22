
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

    //Bomb�� �����ϰ� ���� ���Ѵ�.
    public IEnumerator MakeBomb(System.Action<int> complete)
    {
        isBombDone = false;

        for (int i = 0; i < bombCount; i++)
        {
            //bomb ����
            bomb = EJObjectPoolMgr.instance.GetbombQueue();

            //bomb�� boss���� Instantiate�ǰ�, PhotonNetwork.Instantiate�� �ƴϱ� ������
            //photonView�� ������ �����Ǵ��� �𸥴�.
            //�׷��� ������ �����Ǵ� Bomb�� ������ Boss�� photonView�� �ٿ��ش�.
            //master������ bomb ���� �� �����ϰ� �̸� �� PC���� �����ֱ� ���ؼ�

            bomb.GetComponent<EJBomb>().tankPv = photonView;

            if (bomb != null)
            {
                //bomb ����
                bomb.transform.position = bombPos.position;
                bomb.transform.forward = bombPos.transform.up;

                EJBossSFX.instance.PlaybombFlyingSFX();
                ONHeadAnim();
                ONBodyAnim();

                //bombMuzzle ����
                GameObject bombMuzzleImpact = Instantiate(bombMuzzleFactory);

                bombMuzzleImpact.transform.position = bombPos.position;
                bombMuzzleImpact.transform.localEulerAngles = bombPos.transform.parent.localEulerAngles;
                bombMuzzleImpact.transform.localScale = Vector3.one;
                bombMuzzleImpact.transform.up = bombPos.transform.forward;

                //bombPosX += Random.Range(-0.3f, 0.3f);
                bombPosY += Random.Range(-20f, 20f);
                //bombPosZ += Random.Range(-0.3f, 0.3f);

                //forward��ġ�� Random���� �ص־���.
                bombPos.Rotate(new Vector3(0,bombPosY,0)+ originBombAngle, Space.Self);

                //�����ؼ� �Ѿ˿� AddForce�� �ְ� ������ �̷��� �ϸ� �޸���?
                bomb.GetComponent<EJBomb>().rb.AddForce(bombPos.transform.forward * 20, ForceMode.Impulse);

                //rb.AddForce(rot * 20, ForceMode.Impulse);
                //GetComponent<EJBomb>().BulletFire();
            }

            //��Ÿ��
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


    //���� Bomb Script���� ���������� �ϴ� �Լ����� RPC�� ������ ���ؼ�
    //Boss�� �پ��ִ� Script���ٰ� �ۼ����ִ� ��

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
}