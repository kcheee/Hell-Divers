using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_Atk : MonoBehaviour
{
    public GameObject gm;
    public GameObject bulletT;
    public GameObject FirePos;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(RangedAttack());
        }
    }

    // 이니시에이트 원거리 공격
    IEnumerator RangedAttack()
    {
        Vector3 pos = new Vector3(transform.position.x, -1.2f, transform.position.z);

        float T = 12;
        for (int i = 0; i < 10; i++)
        {
            T += Random.Range(0.8f, 2f);

            Vector3 bulletpos = pos + transform.forward * T + transform.right * Random.Range(-1f, 1f);
            GameObject bullet = Instantiate(bulletT, FirePos.transform.position, Quaternion.identity);

            //bullet.transform.forward = TsetG.transform.position - transform.position;
            // 밑방향으로 힘을 줘야함.
            Debug.Log(bulletpos - transform.position);
            bullet.GetComponent<Rigidbody>().AddForce((bulletpos - transform.position) * 10, ForceMode.Impulse);

            yield return new WaitForSeconds(0.15f);
        }
    }
}
