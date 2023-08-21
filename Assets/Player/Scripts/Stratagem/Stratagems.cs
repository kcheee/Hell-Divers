using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Stratagems : MonoBehaviourPun
{

    Rigidbody rbody;
    //�÷��̾� ��ų ��Ʈ��Ÿ�� 
    //���������� �ۿ��ϴٰ� ���� �ð��� �Ǹ� ���߰� ī��Ʈ �ٿ��� ���۵ȴ�.
    public float callTime;
    public float startTime;
    //��Ʈ��Ÿ������ ������ ����
    public GameObject Platform;
    public GameObject Item;
    public AudioClip clip;
    public GameObject timeText;
    public string id;

    bool isUse;
    Animator anim;
    //ȣ�� �ڵ� ����Ʈ
    //�÷��̾� �Է��� �� ȣ���ڵ�� ���ٸ� ȣ��ȴ�.
    public List<KeyType.Key> CallCode = new List<KeyType.Key>();
    public Image Image;

    public System.Action<Vector3,Quaternion> action;

    void Start()
    {
        /*rbody = GetComponent<Rigidbody>();
        //���� �ð��� ȣ�� �ð����� ����
        time = callTime;
        anim = GetComponent<Animator>();*/
        //rbody.AddForce(transform.forward * 7 + transform.up * 5, ForceMode.Impulse);
    }
    public void start() {
        rbody = GetComponent<Rigidbody>();
        //���� �ð��� ȣ�� �ð����� ����
        time = callTime;
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    float time;

    Text text;
    IEnumerator CallStratagem() {
        //���� �ð��� �帣��
        
        //�ڽ��� ������ ����.
        SoundManager.instance.SfxPlay(clip);
        //rbody.isKinematic = true;

        Transform Str_time = PlayerUI.instance.StratagemTime;
        GameObject textObj = Instantiate(timeText, Str_time);
        text = textObj.GetComponent<Text>();
        //�ð��� ���.

        while (true) {
            yield return new WaitForSeconds(Time.deltaTime);
            //���� �ð����� �帣�� �ð��� ��� ����.
            time -= Time.deltaTime;
            text.text = (Mathf.Floor(time * 100 ) / 100 ).ToString() ;
            //PlayerUI.instance.TimeText.text = "ź�� ���� : " + Mathf.Floor(time * 100) / 100;
            //�׷��ٰ� time�� 0���� �۾�����
            if (time < 0) {
                text.text = "0";
                Destroy(textObj);

                //��Ʈ��Ÿ���� ȣ���Ѵ�.

                
                Vector3 pos = transform.position;
                Quaternion rot = Quaternion.identity;
                if (PhotonNetwork.IsMasterClient) {
                    //PhotonNetwork.Instantiate("Platform-Main", pos,rot);
                    photonView.RPC(nameof(Spawn), RpcTarget.All,pos,rot);
                }

                break;
            }
        }
    }

    [PunRPC]
    public void Spawn(Vector3 pos,Quaternion rot) {
        
        Debug.LogWarning("��������~");
        Destroy(text.gameObject);
        SpawnAction(pos,rot);
        /*GameObject platObj = Instantiate(Platform, pos, rot);
        Platform platform = platObj.GetComponent<Platform>();
        platform.Item = this.Item;*/
        Destroy(gameObject);
    }
    public void Call() {
        StartCoroutine(CallStratagem());
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Floor")){
            //�ڷ�ƾ���� ó���Ѵ�.(����)

            //rbody.isKinematic = true;
            StartCoroutine(GroundDelay(2f));
            
            
        }
        
    }

    [PunRPC]
    public void Throw(Vector3 forward,Vector3 up) {
        Debug.LogWarning("SSSSSSFFFFF");
        Rigidbody rbody = this.GetComponent<Rigidbody>();
        rbody.AddForce(forward * 7 + up * 5, ForceMode.Impulse);
        rbody.AddTorque(Vector3.forward * 1000 + Vector3.right * 500 + Vector3.up * 400, ForceMode.Impulse);
    }

    IEnumerator GroundDelay(float t) {
        yield return new WaitForSeconds(t);
        rbody.isKinematic = true;
        transform.rotation = Quaternion.identity;
        anim.SetTrigger("Land");
        Call();
    }

    protected virtual void SpawnAction(Vector3 pos,Quaternion rot) {
        Debug.Log("Spawn");
    }
}
