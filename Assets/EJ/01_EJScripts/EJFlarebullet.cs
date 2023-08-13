using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJFlarebullet : MonoBehaviour
{

    float flareBulletSpeed = 50f;
    float destroyTime = 1f;

    public GameObject floorEffectFactroy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * flareBulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //floor에 닿았을 때 생기는 효과 왜 안생김?
            GameObject floorEffect = Instantiate(floorEffectFactroy);
            floorEffect.transform.position = transform.position;

            floorEffect.transform.forward = other.transform.up;
            floorEffect.transform.localScale = Vector3.one * 2;

            StartCoroutine(DestroySelf4Trigger(other));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //collision의 정확한 지점
            GameObject floorEffect = Instantiate(floorEffectFactroy);
            floorEffect.transform.position = collision.contacts[0].point;
            //성공?
            floorEffect.transform.forward = collision.GetContact(0).normal;
            floorEffect.transform.localScale = Vector3.one * 10;

            StartCoroutine(DestroySelf4Collision(collision));

            //성공은 했는데 서로 부딪혀서 튕김 이걸 어떻게 해야하는지
        }
    }
    IEnumerator DestroySelf4Collision(Collision collision)
    {
        flareBulletSpeed = 0;
        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }

    IEnumerator DestroySelf4Trigger(Collider other)
    {
        flareBulletSpeed = 0;
        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }
}
