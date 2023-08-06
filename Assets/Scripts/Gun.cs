using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //�ϴ� �Ѿ� �߻� �����..
    //������ �Ѿ� �̵�,źâ,��������.
    public GameObject Bullet;

    //�ѱ�
    public Transform FirePos;

    public int maxBullet;
    public int currentBullet;

    public int maxManganize;
    public int currentManganize;
    //���� ��Ÿ���.
    public float MaxDistance;

    public float fireTime;

    public bool isFire = true;

    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        currentBullet = maxBullet;
        currentManganize = maxManganize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Fire�� ������� �ѹ� �� �÷��� �� ���ϱ�
    //Ray�� ���ִ°Ͱ���.
    public void Fire() {
        //�̰Ű������� ���߿� ������Ƽ�� bool ������ �������� �ϴ°� �� ��������
        
        //�Ѿ��� �ִ�?
        if (currentBullet > 0 && isFire) {
            isFire = false;
            StartCoroutine(FireWait());
            //�Ѿ� ����Ʈ�� �����Ѵ�
            //(���� �ڵ�)

            //�׸��� Ray�� �߻��Ѵ�,.(��Ȯ�ϰ� ���ڸ�, �ѱ� ��ġ���� Ray�� ��.)
            //�׸��� ��Ÿ�.
            //�ٵ� Ray�� �����̴ϱ� �ϳ��� �� ��ų� �ƴϸ� �׳� ������ �����ų�. 1��° ����� 
            //�� ������.
            //�븻����
            Debug.LogError("Shoot!");
            Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.red,1);
            Ray ray = new Ray(transform.position,transform.forward);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,MaxDistance)) {
                EnemyTest enemy = hit.collider.gameObject.GetComponent<EnemyTest>();
                if (enemy)
                {
                    enemy.Damaged(damage);
                }
            }
            //GameObject bullet = Instantiate(Bullet, transform.position, transform.rotation);
            //bullet.transform.forward = transform.forward;
            currentBullet--;
        }

    }

    private void OnEnable()
    {
        isFire = true;
    }
    public void Reload() {
        //źâ�� 0���� ũ�� ���� �Ѿ��� �ִ� �Ѿ˺��� ������ ������ �Ҽ�����!
        if (currentManganize > 0 && currentBullet < maxBullet) {
            Debug.Log("Reloading!!!");
            currentBullet = maxBullet;
            currentManganize--;
        }
        
    }

    IEnumerator FireWait() {
        yield return new WaitForSeconds(fireTime);
        isFire = true;
    }
}
