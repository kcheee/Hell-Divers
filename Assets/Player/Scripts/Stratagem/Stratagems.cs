using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stratagems : MonoBehaviour
{

    Rigidbody rbody;
    //플레이어 스킬 스트라타잼 
    //물리적으로 작용하다가 일정 시간이 되면 멈추고 카운트 다운이 시작된다.
    public float callTime;
    public float startTime;
    //스트라타잼으로 생성할 무언가
    public GameObject Item;
    public AudioClip clip;

    bool isUse;
    Animator anim;
    //호출 코드 리스트
    //플레이어 입력이 이 호출코드와 같다면 호출된다.
    public List<KeyType.Key> CallCode = new List<KeyType.Key>();

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        //현재 시간을 호출 시간으로 설정
        time = callTime;
        anim = GetComponent<Animator>();
        //rbody.AddForce(transform.forward * 7 + transform.up * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    float time;
    IEnumerator CallStratagem() {
        //일정 시간이 흐르고
        
        //자신의 물리를 끈다.
        SoundManager.instance.SfxPlay(clip);
        //rbody.isKinematic = true;
        //시간을 잰다.
        while (true) {
            yield return new WaitForSeconds(Time.deltaTime);
            //현재 시간에서 흐르는 시간을 계속 뺀다.
            time -= Time.deltaTime;
            
            PlayerUI.instance.TimeText.text = "탄약 보충 : " + Mathf.Floor(time * 100) / 100;
            //그러다가 time이 0보다 작아질때
            if (time < 0) {
                PlayerUI.instance.TimeText.text = "탄약 보충 : " + "0";
                //스트라타잼을 호출한다.
                Instantiate(Item,transform.position,Quaternion.identity);
                Destroy(gameObject);
                break;
            }
        }
    }

    public void Call() {
        StartCoroutine(CallStratagem());
    }

    private void OnCollisionEnter(Collision collision)
    {

        //rbody.isKinematic = collision.gameObject.CompareTag("Floor");


        if (collision.gameObject.CompareTag("Floor")){
            //코루틴으로 처리한다.(예정)

            //rbody.isKinematic = true;
            StartCoroutine(GroundDelay(2f));
            
            
        }
        
    }


    IEnumerator GroundDelay(float t) {
        yield return new WaitForSeconds(t);
        rbody.isKinematic = true;
        transform.rotation = Quaternion.identity;
        anim.SetTrigger("Land");
        Call();
    }
}
