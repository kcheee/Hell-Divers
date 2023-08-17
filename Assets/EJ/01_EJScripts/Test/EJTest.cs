using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJTest : MonoBehaviour
{
    private void OnEnable()
    {
        //파티클을 다시 Play 하는 코드
        //print("파티클 플레이");
        //몇초 뒤에 호출되는 함수
        //StartCoroutine(ReturnGausCannonMuzzleImpactQueue());
        //Invoke(nameof(ReturnGausCannonMuzzleImpactQueue), 2);
    }

    IEnumerator ReturnGausCannonMuzzleImpactQueue()
    {
        yield return new WaitForSeconds(2);
        print("풀에 다시 들어가고 비활성화 됨");
       // gameObject.SetActive(false);
        //EJObjectPoolMgr.instance.ReturnGausCannonMuzzleImpactQueue(gameObject);
    }

    bool isFire;

    //처음 포의 로컬 위치
    //public GameObject bombHead;

    Vector3 bombHeadLocalPos;
    Vector3 bombHeadOriginLocalPos;

    //뒤로 가야하는 로컬 위치
    Vector3 backLocalPos;

    //bool체크
    bool isBackPos = false;

    // Start is called before the first frame update
    void Start()
    {
        bombHeadOriginLocalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            isFire = true;
        }

        if(isFire)
        {
            //현재 로컬위치, 뒤로 가야하는 로컬위치 Lerp
            if (!isBackPos)
            {
                bombHeadLocalPos = transform.position;
                bombHeadLocalPos = Vector3.Lerp(bombHeadLocalPos, backLocalPos, 0.2f);

                //거리체크


            }else
            {
                bombHeadLocalPos = transform.position;
                bombHeadLocalPos = Vector3.Lerp(bombHeadLocalPos, bombHeadOriginLocalPos, 0.1f);
            }
            isBackPos = true;
        }       
    }
}
