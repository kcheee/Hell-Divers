using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJTest : MonoBehaviour
{
    private void OnEnable()
    {
        //��ƼŬ�� �ٽ� Play �ϴ� �ڵ�
        //print("��ƼŬ �÷���");
        //���� �ڿ� ȣ��Ǵ� �Լ�
        //StartCoroutine(ReturnGausCannonMuzzleImpactQueue());
        //Invoke(nameof(ReturnGausCannonMuzzleImpactQueue), 2);
    }

    IEnumerator ReturnGausCannonMuzzleImpactQueue()
    {
        yield return new WaitForSeconds(2);
        print("Ǯ�� �ٽ� ���� ��Ȱ��ȭ ��");
       // gameObject.SetActive(false);
        //EJObjectPoolMgr.instance.ReturnGausCannonMuzzleImpactQueue(gameObject);
    }

    bool isFire;

    //ó�� ���� ���� ��ġ
    //public GameObject bombHead;

    Vector3 bombHeadLocalPos;
    Vector3 bombHeadOriginLocalPos;

    //�ڷ� �����ϴ� ���� ��ġ
    Vector3 backLocalPos;

    //boolüũ
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
            //���� ������ġ, �ڷ� �����ϴ� ������ġ Lerp
            if (!isBackPos)
            {
                bombHeadLocalPos = transform.position;
                bombHeadLocalPos = Vector3.Lerp(bombHeadLocalPos, backLocalPos, 0.2f);

                //�Ÿ�üũ


            }else
            {
                bombHeadLocalPos = transform.position;
                bombHeadLocalPos = Vector3.Lerp(bombHeadLocalPos, bombHeadOriginLocalPos, 0.1f);
            }
            isBackPos = true;
        }       
    }
}
