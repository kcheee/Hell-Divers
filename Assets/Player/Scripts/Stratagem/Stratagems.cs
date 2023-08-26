using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Stratagems : MonoBehaviourPun
{

    Rigidbody rbody;
    //플레이어 스킬 스트라타잼 
    //물리적으로 작용하다가 일정 시간이 되면 멈추고 카운트 다운이 시작된다.
    public float callTime;
    public float startTime;
    //스트라타잼으로 생성할 무언가
    public GameObject Platform;
    public GameObject Item;
    public AudioClip clip;
    public GameObject timeText;
    public Sprite Stratagem_Image;
    public string id;

    //자신의 UI를 가지고(생성할때)
    public StratagemUICode myCodeUI;

    bool isUse;
    Animator anim;
    //호출 코드 리스트
    //플레이어 입력이 이 호출코드와 같다면 호출된다.
    public List<KeyType.Key> CallCode = new List<KeyType.Key>();
    public Image Image;

    public System.Action<Vector3,Quaternion> action;


    public StratagemUICode myStratagemUI; 

    void Start()
    {
        /*rbody = GetComponent<Rigidbody>();
        //현재 시간을 호출 시간으로 설정
        time = callTime;
        anim = GetComponent<Animator>();*/
        //rbody.AddForce(transform.forward * 7 + transform.up * 5, ForceMode.Impulse);
    }
    public void start() {
        rbody = GetComponent<Rigidbody>();
        //현재 시간을 호출 시간으로 설정
        time = callTime;
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    float time;

    StratagemArt art;


    IEnumerator CallStratagem() {
        //일정 시간이 흐르고
        
        //자신의 물리를 끈다.
        SoundManager.instance.SfxPlay(clip); //호출 효과
        //rbody.isKinematic = true;

        Transform Str_time = PlayerUI.instance.StratagemTime; //타임을 넣을 부모 오브젝트에 
        GameObject textObj = Instantiate(timeText, Str_time); //Text를 넣고 
        StratagemArt artObj = textObj.GetComponent<StratagemArt>();
        art = artObj;
        art.image.sprite = Stratagem_Image;
        //art.image = Stratagem_Image;
        //시간을 잰다.

        while (true) {
            yield return new WaitForSeconds(Time.deltaTime);
            //현재 시간에서 흐르는 시간을 계속 뺀다.
            time -= Time.deltaTime;

            art.text.text = "00:" + (Mathf.Floor(time * 100 ) / 100 ).ToString() ;
            //PlayerUI.instance.TimeText.text = "탄약 보충 : " + Mathf.Floor(time * 100) / 100;
            //그러다가 time이 0보다 작아질때
            if (time < 0) {


                
                Vector3 pos = transform.position;
                Quaternion rot = Quaternion.identity;
                if (PhotonNetwork.IsMasterClient) {
                    //마스터 클라이언트의 위치를 넘겨주며 모든 PC에 RPC를 실행한다.
                    photonView.RPC(nameof(Spawn), RpcTarget.All,pos,rot);
                }                
                art.text.text = "0";
                //Destroy(textObj);
                Debug.Log("삭제됬다고 @@@@@");
                //스트라타잼을 호출한다.

                break;
            }
        }
    }


    //방장이 스폰하라고 호출할때
    [PunRPC]
    public void Spawn(Vector3 pos,Quaternion rot) {
        //그때 Text를 지우고 스폰한다.
        if(art.gameObject != null)
            Destroy(art.gameObject);
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
        //던져지고, 바닥에 닿았다면
        if (collision.gameObject.CompareTag("Floor")){

            //2초 뒤에 action을 실행한다.
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


    public void UIChanged(int index) {
        myStratagemUI.Code_images[index].color = Color.red;
    }

}
