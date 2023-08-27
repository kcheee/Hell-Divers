using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJFlarebullet : MonoBehaviourPun
{
    #region flarebullet����
    float flareBulletSpeed = 50f;
    float destroyTime = 1f;
    float bombRadius = 3;

    public GameObject floorEffectFactroy;
    //public PhotonView tankPv;

    Rigidbody rb;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(DestroySelf());
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * flareBulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }


    #region 01. Triggerȿ��
    private void OnTriggerEnter(Collider other)
    {
        //print("FlareBullet�� trigger�� ���� ����" + other.gameObject);

        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            print("Floor�� ��Ҵ�");

            //RPC�� �Ⱥ��δ�.

            //photonView.RPC(nameof(ShowBulletImpact), RpcTarget.All);
            photonView.RPC(nameof(test), RpcTarget.All);
        }

        if (other.gameObject.tag == "Player")
        {
            //player �ǰ� pos ���� �ʿ�
            //other.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, transform.position + Vector3.down * 1.6f, 3);

            PlayerDamage();
        }
    }

    #endregion

    [PunRPC]
    void test()
    {
        Debug.Log("���� ��Ÿ");
    }

    #region 02. ShowBulletImpact�Լ�
    [PunRPC]
   public void ShowBulletImpact()
    {
        print("ShowBulletImpact�Լ� ����");

        //GameObject floorEffect = Instantiate(floorEffectFactroy, transform.position, Quaternion.identity);

        //Debug.Log("gausCannon�� �ٴڿ� ����� �� ����� ȿ����" + floorEffect);

        ////floorEffect.transform.localScale = Vector3.one * 2;
        //Destroy(gameObject, 2.5f);
    }


    #endregion

    #region �Ⱦ��� �Լ�
    IEnumerator DestroySelf4Trigger(Collider other)
    {
        //tankPv.RPC("ShowGausCannonImpact", RpcTarget.All, transform.position,other.transform.up);

        //HP�׽�Ʈ �ʿ�
        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, other.gameObject.transform.position + Vector3.up , 3);
        }      

        flareBulletSpeed = 0;
        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }
    #endregion


    #region 03. bombRadius�� ���� PlayerDamage�Լ�
    void PlayerDamage()
    {
        //bomb�ݰ� ���� player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        print("bomb�� ���� ���� " + bombHits[0].transform.gameObject.name);

        foreach (RaycastHit hitObj in bombHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, hitObj.point, 3);
        }

    }
    #endregion
}
