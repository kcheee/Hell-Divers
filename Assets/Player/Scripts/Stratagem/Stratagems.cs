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
    public AudioClip clip;

    //ȣ�� �ڵ� ����Ʈ
    //�÷��̾� �Է��� �� ȣ���ڵ�� ���ٸ� ȣ��ȴ�.
    public List<KeyType.Key> CallCode = new List<KeyType.Key>();

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        //���� �ð��� ȣ�� �ð����� ����
        time = callTime;
        //rbody.AddForce(transform.forward * 7 + transform.up * 5, ForceMode.Impulse);
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
        SoundManager.instance.SfxPlay(clip);
        //rbody.isKinematic = true;
        //�ð��� ���.
        while (true) {
            yield return new WaitForSeconds(Time.deltaTime);
            //���� �ð����� �帣�� �ð��� ��� ����.
            time -= Time.deltaTime;
            
            PlayerUI.instance.TimeText.text = "ź�� ���� : " + Mathf.Floor(time * 100) / 100;
            //�׷��ٰ� time�� 0���� �۾�����
            if (time < 0) {
                PlayerUI.instance.TimeText.text = "ź�� ���� : " + "0";
                //��Ʈ��Ÿ���� ȣ���Ѵ�.
                Instantiate(Item,transform.position,Quaternion.identity);
                Destroy(gameObject);
                break;
            }
        }
    }

    public void Call() {
        StartCoroutine(CallStratagem(startTime));
    }

    private void OnCollisionEnter(Collision collision)
    {

        //rbody.isKinematic = collision.gameObject.CompareTag("Floor");


        if (collision.gameObject.CompareTag("Floor")){
            rbody.isKinematic = true;
            Call();
        }
        
    }
}
