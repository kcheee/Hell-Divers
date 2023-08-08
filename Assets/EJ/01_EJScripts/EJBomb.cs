using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBomb : MonoBehaviour
{
    Rigidbody rb;
    public GameObject bombImpactFactory;
    float speed;

    //���� Trail����
    public TrailRenderer bombTrail;

    // Start is called before the first frame update
    void Start()
    {
        bombTrail = GetComponent<TrailRenderer>();
        bombTrail.enabled = true;

        speed = Random.Range(10,30);

        Vector3 rot = transform.eulerAngles;
        rot.x += Random.Range(-5, 5);
        rot.y += Random.Range(-5, 5);
        rot.z += Random.Range(-5, 5);
        transform.eulerAngles = rot;
    }

    // Update is called once per frame
    void Update()
    {
        //��� ������ ���� �ʹ�.
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            GameObject bombImpact = Instantiate(bombImpactFactory);
            bombImpact.transform.position = transform.position;
            bombImpact.transform.forward = collision.GetContact(0).normal;

            //normal�� ���ִ� ���
            Destroy(bombImpact, 3);
            Destroy(gameObject);
            bombTrail.enabled = false;
        }
    }
}
