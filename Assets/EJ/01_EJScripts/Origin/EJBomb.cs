using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

//Bomb�� �浹ó���� ������ִ� ��ũ��Ʈ?

public class EJBomb : MonoBehaviourPun
{
    //bomb ����
    public Rigidbody rb;
    float bombSpeed;
    GameObject bomExploImpact;
    float bombDestroyTime = 0.2f;
    float bombRadius = 2;

    public PhotonView tankPv;

    //���� Trail����
    //public TrailRenderer bombTrail;

    GameObject player;

    //ȸ����
    Vector3 rot;
    float bombPower;

    // Start is called before the first frame update
    void Start()
    {
        bombPower = Random.Range(14f, 20f);
        rb.velocity = Vector3.zero;
        //bombTrail = GetComponent<TrailRenderer>();
        //bombTrail.enabled = true;

        //bombFire Speed, Angle Random�ϰ�
        //bombSpeed = Random.Range(5,10);

        //rot = transform.eulerAngles;
        //rot.x += Random.Range(-5, 5);
        //rot.y += Random.Range(-5, 5);
        //rot.z += Random.Range(-5, 5);
        //transform.eulerAngles = rot;

        //�̰� �Ѱ���� �ϴ� �� fire?�� 
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = Vector3.zero;
        //rb.AddForce(transform.forward * bombPower, ForceMode.Impulse);
    }

    public void BulletFire()
    {
        //�̰� �Ѱ���� �ϴ� �� fire?�� 
        rb = GetComponent<Rigidbody>();
        //rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * bombPower, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.up * bombSpeed * Time.deltaTime;
    }

    /*    private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                transform.position = other.transform.position;
                //���� �ε����� bomb ���ֱ�
                EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);

                //04.coroutine���� ���� �� ������Ʈ�� ���� GameObject�� ���� �Ŀ��� �۵��ϵ��� �Ѵ�.
                EJGlobalCoroutine.instance.StartCoroutine(bombExplode(other));
            }
        }*/

    #region collision �Լ�

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if(PhotonNetwork.IsMasterClient)
            {
                //04.coroutine���� ���� �� ������Ʈ�� ���� GameObject�� ���� �Ŀ��� �۵��ϵ��� �Ѵ�.
                StartCoroutine(bombExplode(collision));
            }

            //���� �ε����� bomb ���ֱ�
            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);
            EJBossSFX.instance.PlaybombExploSFX();
        }

        
    }



    //bomb �ܻ��� ������ ������ �Լ�
    IEnumerator bombExplode (Collision collision)
    {
        tankPv.RPC("ShowBombExploImpact", RpcTarget.All, transform.position, collision.GetContact(0).normal, bombDestroyTime);
        //bomExploImpact = EJObjectPoolMgr.instance.GetbombExploImpactQueue();

        //bomExploImpact.transform.position = transform.position;
        //bomExploImpact.transform.localScale = Vector3.one *3;
        //bomExploImpact.transform.forward = collision.GetContact(0).normal;

        yield return new WaitForSeconds(bombDestroyTime);

        //PlayerRPC�� �ٲ��ִ� ��
        

        //bomb�ݰ� ���� player damage // ���� �ȵǰ� ����
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));


        //!!!!!!!!!!Player�� �¾Ҵ� �����ؼ� ���ȿ� ������ �������� �޾ƾ� �Ѵ�. 
       // player.GetComponent<PlayerTest1>().photonView = photonView;

        foreach (RaycastHit hitObj in bombHits)
        {
            //pho
            //hitObj.transform.GetComponent<EJPlayerHPforTest>().DamageHP(3);
            hitObj.transform.GetComponent<PhotonView>().RPC("damaged",RpcTarget.All,hitObj.point, 3);
        }

        //EJObjectPoolMgr.instance.ReturnbombExploImpactQueue(bomExploImpact);
        yield return null;

        //BossFSM.Sflag = false;
    }

    #endregion


    #region Trigger�Լ� 


    private void OnTriggerEnter(Collider other)
    {
        print("bomb���� ���� �ȿ� ���� ����" + other.gameObject);

        if (other.gameObject.tag == "Floor")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(bombExplodebyTrigger(other));
            }

            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);
            EJBossSFX.instance.PlaybombExploSFX();
        }

        if (other.gameObject.tag == "Player")
        {
            PlayerDamage();
        }
    }

    //bomb �ܻ��� ������ ������ �Լ�
    IEnumerator bombExplodebyTrigger(Collider other)
    {
        tankPv.RPC("ShowBombExploImpact", RpcTarget.All, transform.position, other.transform, bombDestroyTime);
        //bomExploImpact = EJObjectPoolMgr.instance.GetbombExploImpactQueue();

        //bomExploImpact.transform.position = transform.position;
        //bomExploImpact.transform.localScale = Vector3.one *3;
        //bomExploImpact.transform.forward = collision.GetContact(0).normal;

        yield return new WaitForSeconds(bombDestroyTime);

        //PlayerRPC�� �ٲ��ִ� ��


    }

    void PlayerDamage()
    {

        //bomb�ݰ� ���� player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));


        print("bomb�� ���� ���� "+ bombHits[0].transform.gameObject.name);
        


        //!!!!!!!!!!Player�� �¾Ҵ� �����ؼ� ���ȿ� ������ �������� �޾ƾ� �Ѵ�. 
        // player.GetComponent<PlayerTest1>().photonView = photonView;

        foreach (RaycastHit hitObj in bombHits)
        {
            //pho
            //hitObj.transform.GetComponent<EJPlayerHPforTest>().DamageHP(3);
            hitObj.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, hitObj.point, 3);
        }

        //EJObjectPoolMgr.instance.ReturnbombExploImpactQueue(bomExploImpact);
        //yield return null;

        //BossFSM.Sflag = false;
    }
    #endregion



}

