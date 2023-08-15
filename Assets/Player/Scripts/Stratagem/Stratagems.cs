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


    //호출 코드 리스트
    //플레이어 입력이 이 호출코드와 같다면 호출된다.
    public List<KeyType.Key> CallCode = new List<KeyType.Key>();

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        //현재 시간을 호출 시간으로 설정
        time = callTime;
        StartCoroutine(CallStratagem(startTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    float time;
    IEnumerator CallStratagem(float startT) {
        //일정 시간이 흐르고
        yield return new WaitForSeconds(startT);
        //자신의 물리를 끈다.
        rbody.isKinematic = true;
        //시간을 잰다.
        while (true) {
            //현재 시간에서 흐르는 시간을 계속 뺀다.
            time -= Time.deltaTime;
            //그러다가 time이 0보다 작아질때
            if (time < 0) {
                //스트라타잼을 호출한다.
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
