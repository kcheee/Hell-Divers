using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJTest : MonoBehaviour
{
    private void OnEnable()
    {
        //��ƼŬ�� �ٽ� Play �ϴ� �ڵ�
        print("��ƼŬ �÷���");
        //���� �ڿ� ȣ��Ǵ� �Լ�
        StartCoroutine(ReturnGausCannonMuzzleImpactQueue());
        //Invoke(nameof(ReturnGausCannonMuzzleImpactQueue), 2);
    }

    IEnumerator ReturnGausCannonMuzzleImpactQueue()
    {
        yield return new WaitForSeconds(2);
        print("Ǯ�� �ٽ� ���� ��Ȱ��ȭ ��");
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
