using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJFlarebullet : MonoBehaviourPun
{

    float flareBulletSpeed = 50f;
    float destroyTime = 1f;

    public GameObject floorEffectFactroy;

    public PhotonView tankPv;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf());
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * flareBulletSpeed;
        //transform.position += transform.up * flareBulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("11111111 : " +other.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //floor�� ����� �� ����� ȿ�� �� �Ȼ���?
            GameObject floorEffect = Instantiate(floorEffectFactroy);
            floorEffect.transform.position = transform.position;

            floorEffect.transform.forward = other.transform.up;
            floorEffect.transform.localScale = Vector3.one * 2;

            StartCoroutine(DestroySelf4Trigger(other));
        }

        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, transform.position, 3);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //�ε��� ���� Floor Effect�� ������ ���� RPC�� �� ����

                //collision�� ��Ȯ�� ����
                //GameObject floorEffect = Instantiate(floorEffectFactroy);
                //floorEffect.transform.position = collision.contacts[0].point;
                ////����?
                //floorEffect.transform.forward = collision.GetContact(0).normal;
                //floorEffect.transform.localScale = Vector3.one * 10;

                StartCoroutine(DestroySelf4Collision(collision));
                
            }          
            //������ �ߴµ� ���� �ε����� ƨ�� �̰� ��� �ؾ��ϴ���
        }
    }

    IEnumerator DestroySelf()
    {
        Destroy(gameObject, 3);
        yield return null;
    }

    IEnumerator DestroySelf4Collision(Collision collision)
    {
        tankPv.RPC("ShowGausCannonImpact", RpcTarget.All, transform.position, collision.GetContact(0).normal);

        #region HP�׽�Ʈ �ʿ�
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, collision, 3);
        }

        #endregion
        flareBulletSpeed = 0;
        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }

    IEnumerator DestroySelf4Trigger(Collider other)
    {
        tankPv.RPC("ShowGausCannonImpact", RpcTarget.All, transform.position,other.transform.up);

        #region HP�׽�Ʈ �ʿ�
        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, other, 3);
        }

        #endregion

        flareBulletSpeed = 0;
        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }
}
