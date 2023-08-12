using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJTest : MonoBehaviour
{
    private void OnEnable()
    {
        //파티클을 다시 Play 하는 코드
        print("파티클 플레이");
        //몇초 뒤에 호출되는 함수
        StartCoroutine(ReturnGausCannonMuzzleImpactQueue());
        //Invoke(nameof(ReturnGausCannonMuzzleImpactQueue), 2);
    }

    IEnumerator ReturnGausCannonMuzzleImpactQueue()
    {
        yield return new WaitForSeconds(2);
        print("풀에 다시 들어가고 비활성화 됨");
        gameObject.SetActive(false);
        //EJObjectPoolMgr.instance.ReturnGausCannonMuzzleImpactQueue(gameObject);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
