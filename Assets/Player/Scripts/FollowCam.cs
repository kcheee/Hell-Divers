using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public float camY = 10; //����   
    public float followSpeed = 10;
    Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindWithTag("Player").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        Vector3 target = Target.position;
        target.y = 0;
        //�� ��ġ�� Ÿ���� ��ġ�� y�� 0���� �����ϰ�
        //���� �Ÿ��� ���.
        float distance = Vector3.Distance(pos, target);

        //�ٽ� ���� y�� ��������.
        pos.y = camY;        
        target.y = camY;
        target.z += -8.5f;


        //������ ������ -7�̴ϱ� �Ʒ��ΰ�. ���߿� ������
        //Debug.Log(distance);
        //�Ÿ��� ���� ���ǵ尡 �޶������� �����Ѵ�.
        transform.position = Vector3.Lerp(pos, target, Time.smoothDeltaTime * 10  );
    }
}
