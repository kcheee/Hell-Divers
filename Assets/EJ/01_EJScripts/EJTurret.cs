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
    

    IEnumerator UpdateTurretFire()
    {
        //Vector3 pos = new Vector3(transform.position.x, -1.2f, transform.position.z);
        //float T = 3;
        //T += Random.Range(0.8f, 2f);
        //Vector3 bulletpos = pos + transform.forward * T + transform.right * Random.Range(-1f, 1f);

            GameObject bullet = Instantiate(turretBulletFactory, turretFirePos.transform.position, Quaternion.identity);
            Instantiate(turretMuzzleFactory, turretFirePos.transform.position, Quaternion.identity).transform.parent = transform;
            bullet.transform.parent = transform;

            ONturretHeadAnim();

            bullet.GetComponent<Rigidbody>().AddForce(turretFirePos.forward, ForceMode.Impulse);

            yield return new WaitForSeconds(0.15f);
        
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
