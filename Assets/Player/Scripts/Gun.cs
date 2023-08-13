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
    public GameObject FireEft;

    public int maxBullet;
    public int currentBullet;

    public int maxManganize;
    public int currentManganize;
    //���� ��Ÿ���.
    public float MaxDistance;

    public float fireTime;

    public bool isFire = true;

    public float damage;

    public bool WaitAnim;

    public Animator animtest;

    //���ۿ����� �ΰ����� �����°Ͱ���
    public enum GunType { Rifle,Pistol }
    public GunType gunType;

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
    public bool Fire() {

        //�̰Ű������� ���߿� ������Ƽ�� bool ������ �������� �ϴ°� �� ��������
        bool fire = currentBullet > 0 && isFire;
        //bool returnfire = currentBullet > 0 && (WaitAnim ? isFire && WaitAnim : true);
        //�Ѿ��� �ִ�?
        if (fire) {
            animtest.ResetTrigger("Fire2");
            animtest.SetTrigger("Fire2");
            //action();
            isFire = false;
            StartCoroutine(FireWait());
            //�Ѿ� ����Ʈ�� �����Ѵ�
            //(���� �ڵ�)

            //�׸��� Ray�� �߻��Ѵ�,.(��Ȯ�ϰ� ���ڸ�, �ѱ� ��ġ���� Ray�� ��.)
            //�׸��� ��Ÿ�.
            //�ٵ� Ray�� �����̴ϱ� �ϳ��� �� ��ų� �ƴϸ� �׳� ������ �����ų�. 1��° ����� 
            //�� ������.
            //�븻����
            Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.white,1); Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.red,1);
            Ray ray = new Ray(transform.position,transform.forward);
            Instantiate(FireEft, FirePos);
            RaycastHit hit;
            // ���߿� layer�� ����
            if (Physics.Raycast(ray, out hit,MaxDistance)) {
                
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.GetComponent<Enemy_Fun>().E_Hit(hit.point);
                }
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
        return isFire;
    }

    private void OnEnable()
    {
        isFire = true;
    }


    //����� �ڵ� Reload �� �����ϴ�
    public bool ReloadAble() {
        //źâ�� 0���� ũ�� ���� �Ѿ��� �ִ� �Ѿ˺��� ������ ������ �Ҽ�����!
        if (currentManganize > 0 && currentBullet < maxBullet) {

            return true;
        }
        return false;
        
    }

    public void Reload() {
        Debug.Log("Reloading!!!");
        currentBullet = maxBullet;
        currentManganize--;
    }

    IEnumerator FireWait() {
        //animtest.ResetTrigger("Fire2");
        yield return new WaitForSeconds(fireTime);
        
        isFire = true;
    }
}
