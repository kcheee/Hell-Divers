using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stratagems : MonoBehaviour
{
    Rigidbody rbody;
    //�÷��̾� ��ų ��Ʈ��Ÿ�� 
    //���������� �ۿ��ϴٰ� ���� �ð��� �Ǹ� ���߰� ī��Ʈ �ٿ��� ���۵ȴ�.
    public float callTime;
    public float startTime;
    //��Ʈ��Ÿ������ ������ ����
    public GameObject Item;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        //���� �ð��� ȣ�� �ð����� ����
        time = callTime;
        StartCoroutine(CallStratagem(startTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    float time;
    IEnumerator CallStratagem(float startT) {
        //���� �ð��� �帣��
        yield return new WaitForSeconds(startT);
        //�ڽ��� ������ ����.
        rbody.isKinematic = true;
        //�ð��� ���.
        while (true) {
            //���� �ð����� �帣�� �ð��� ��� ����.
            time -= Time.deltaTime;
            //�׷��ٰ� time�� 0���� �۾�����
            if (time < 0) {
                //��Ʈ��Ÿ���� ȣ���Ѵ�.
                Instantiate(Item,transform.position,Quaternion.identity);
                Destroy(gameObject);
                break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        rbody.isKinematic = collision.gameObject.CompareTag("Floor");
        
    }
}
