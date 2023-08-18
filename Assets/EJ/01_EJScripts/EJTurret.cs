using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJTurret : MonoBehaviour
{
    //turretAnim
    public Animator turretHeadReaction;

    //fire변수
    float attackRange = 5;
    public LayerMask playerLayer;
    int fireCount;
    int maxFireCount = 5;

    GameObject target;

    public GameObject turretBulletFactory;
    public Transform turretFirePos;
    public GameObject turretMuzzleFactory;
    


    //Head Rotate
    public GameObject turretHead;
    float ry = 0;
    float rotateAngle = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //player 검출
        Collider[] players = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        //player 검출 시 Fire
        if (players.Length > 0)
        {
            Invoke(nameof(fireCountReset), 1f);

            for (int i = 0; i < players.Length; i++)
            {
                if (fireCount < maxFireCount)
                {
                    target = players[i].gameObject;
                    turretHead.transform.LookAt(target.transform);

                    StartCoroutine(UpdateTurretFire());
                }
            }
        }else
        {
            print("Idle상태입니다");
            UpdateIdle();
        }
    }

    bool isRightRotate = false;
    public void UpdateIdle()
    {

        if (isRightRotate)
        {
            ry += rotateAngle * Time.deltaTime;
            if (ry >= 90)
            {
                isRightRotate = false;
            }
        }else
        {
            ry -= rotateAngle * Time.deltaTime;
            if (ry <= -90)
            {
                isRightRotate = true;
            }
        }
        turretHead.transform.localEulerAngles = new Vector3(0, ry, 0);
    }

    // 한번만 실행
    bool isTurretDone = false;
    // 코루틴으로 Update
    float delayTime = 1f;
    // 탈출루트 설정: while문으로 distance보다 멀어졌을 때 감지는 이미 위에서 Idle상태로 가고 있음

    IEnumerator UpdateTurretFire()
    {
        if (!isTurretDone)
        {
            isTurretDone = true;
            GameObject bullet = Instantiate(turretBulletFactory, turretFirePos.transform.position, Quaternion.identity);

            //forward방향 맞춰주는 법??
            Instantiate(turretMuzzleFactory, turretFirePos.transform.position, Quaternion.identity).transform.parent = transform;

            bullet.transform.parent = transform;

            ONturretHeadAnim();

            bullet.GetComponent<Rigidbody>().AddForce(turretFirePos.forward*10, ForceMode.Impulse);

            yield return new WaitForSeconds(delayTime);
            isTurretDone = false;
        }

            
        
    }

    //turretHeadAnim
    public void ONturretHeadAnim()
    {
        turretHeadReaction.SetTrigger("turretFire");
    }

    public void fireCountReset()
    {
        fireCount = 0;
    }
}
