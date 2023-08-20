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
    public GameObject MuzzleEft;

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

    //ź������ ����.
    public float current_spreadRange;
    public float min_spreadRange = 1;
    public float max_spreadRange = 5;
    //ź���� ������
    public float add_spreadRange = 200;

    // Start is called before the first frame update
    void Start()
    {
        //Max�� ���ݴϴ�!
        currentBullet = maxBullet;
        currentManganize = maxManganize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Fire�� ������� �ѹ� �� �÷��� �� ���ϱ�
    //Ray�� ���ִ°Ͱ���.
    public bool Fire(int rand) {

        //�̰Ű������� ���߿� ������Ƽ�� bool ������ �������� �ϴ°� �� ��������
        bool fire = currentBullet > 0 && isFire;
        //bool returnfire = currentBullet > 0 && (WaitAnim ? isFire && WaitAnim : true);
        //�Ѿ��� �ִ�?
        if (fire) {
            //�߻�������, ź���� ������ ������Ų��.
            current_spreadRange += 0.01f * add_spreadRange;
            //Debug.LogError("���� ź����!" + current_spreadRange);
            //ź���� ������ �����Ѵ�.
            current_spreadRange = Mathf.Clamp(current_spreadRange, min_spreadRange, max_spreadRange);


            animtest.ResetTrigger("Fire2");
            animtest.SetTrigger("Fire2");
            isFire = false;
            StartCoroutine(FireWait());

            //�׸��� Ray�� �߻��Ѵ�,.(��Ȯ�ϰ� ���ڸ�, �ѱ� ��ġ���� Ray�� ��.)
            //�׸��� ��Ÿ�.
            //�ٵ� Ray�� �����̴ϱ� �ϳ��� �� ��ų� �ƴϸ� �׳� ������ �����ų�. 1��° ����� 
            //�� ������.(���� �׳� ����Ʈ�ΰŰ���!)
            //�븻����\

            //���� ź���� ���� ���Ѵ�.
            //�ڽ��� �չ��⿡�� �ڽ��� ������  ���Ϳ��� ���Ⱚ�� ���ϰ� �������� ���� ���͸� ���Ѵ�.
            Vector3 spread = transform.forward + transform.right * rand * 0.01f * current_spreadRange;
            //Debug.LogError("�̰��� ź�����̴�." + spread);
            Debug.DrawRay(transform.position, spread * MaxDistance, Color.red, 1);

            Ray ray = new Ray(transform.position,spread);
            //Debug.Log(transform.forward);
            GameObject fireEft = Instantiate(FireEft, FirePos.position,Quaternion.identity);
            GameObject muzzleEft = Instantiate(MuzzleEft, FirePos.position, Quaternion.identity);
            //fireEft.transform.parent = null;
            //fireEft.transform.forward = transform.forward ;
            fireEft.transform.rotation = Quaternion.LookRotation(spread,Vector3.up);
            muzzleEft.transform.forward = FirePos.forward;
            
            RaycastHit hit;
            // ���߿� layer�� ����
            if (Physics.Raycast(ray, out hit,MaxDistance)) {
                Debug.Log("Hit" + hit.collider.gameObject.name);
                I_Entity entity =  hit.collider.gameObject.GetComponent<I_Entity>();
                if (entity != null) {
                    entity.damaged(10);
                }
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

    public void ResetSpread() {
        current_spreadRange = 0;
    }
}
