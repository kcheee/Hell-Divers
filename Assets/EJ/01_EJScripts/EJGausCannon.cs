using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJGausCannon : MonoBehaviour
{
    //fire변수
    float rayLength;
    float minRayLength = 3;
    float addLegnth = 0.5f;
    int fireCount = 15;
    public Transform[] firePos;
    int firePosIdx;
    int currentIdx;
    int direction;
    public GameObject cannonImpactFactory;

    [SerializeField]
    TrailRenderer bulletTrail;

    int enemyLayer;

    public Transform trFirePos;

    // Start is called before the first frame update
    void Start()
    {
        rayLength = minRayLength;       
        enemyLayer = (1 << LayerMask.NameToLayer("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(Fire());


            //for (int i = 0; i < fireCount; i++)
            //{
            //    //rayLength
            //    rayLength += addLegnth;

            //    //firePosIdx
            //    firePosIdx = currentIdx;
            //    currentIdx += direction;

            if (currentIdx == 4 || currentIdx == 0)
            {
            
                direction *= -1;
            }

            //    if (Physics.Raycast(firePos[firePosIdx].position, firePos[firePosIdx].forward, out hitInfo, float.MaxValue, enemyLayer))
            //    {
            //        GameObject cannonImpact = Instantiate(cannonImpactFactory);

            //        cannonImpact.transform.position = hitInfo.point;
            //        cannonImpact.transform.forward = hitInfo.normal;
            //        cannonImpact.transform.parent = hitInfo.transform;

            //        //TrailRenderer Trail = Instantiate(bulletTrail, firePos[firePosIdx].position, Quaternion.identity);
            //        //StartCoroutine(DrawTrail(Trail, hitInfo));
            //    }
            //}
        }
        
    }

    public void Shoot()
    {

    }

    IEnumerator Fire()
    {
        int dir = -1;
        RaycastHit hitInfo;
        for (int i = 0; i < 10; i++)
        {
            if (Physics.Raycast(trFirePos.position, trFirePos.up, out hitInfo, float.MaxValue, enemyLayer))
            {
                GameObject cannonImpact = Instantiate(cannonImpactFactory);

                cannonImpact.transform.position = hitInfo.point;
                cannonImpact.transform.forward = hitInfo.normal;
                cannonImpact.transform.parent = hitInfo.transform;


                //TrailRenderer Trail = Instantiate(bulletTrail, firePos[firePosIdx].position, Quaternion.identity);
                //StartCoroutine(DrawTrail(Trail, hitInfo));
            }

            //z
            trFirePos.Rotate(new Vector3(0.1f, 0, 5 * dir), Space.Self);
            yield return new WaitForSeconds(0.1f);
            if (i == 4) dir = 1;

            //원래로 돌리기
        }
    }

    private IEnumerator DrawTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }

        Trail.transform.position = Hit.point;
    }
}
