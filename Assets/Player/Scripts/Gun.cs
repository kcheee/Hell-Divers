using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //일단 총알 발사 방식이..
    //아직은 총알 이동,탄창,재장전만.
    public GameObject Bullet;

    //총구
    public Transform FirePos;

    public int maxBullet;
    public int currentBullet;

    public int maxManganize;
    public int currentManganize;
    //총의 사거리다.
    public float MaxDistance;

    public float fireTime;

    public bool isFire = true;

    public float damage;

    public bool WaitAnim;

    public Animator animtest;

    //원작에서는 두가지로 나뉘는것같음
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
    //Fire의 같은경우 한번 더 플레이 해 보니까
    //Ray로 되있는것같다.
    public bool Fire() {

        //이거같은경우는 나중에 프로퍼티로 bool 변수를 빼가지고 하는게 더 나을려나
        bool fire = currentBullet > 0 && isFire;
        //bool returnfire = currentBullet > 0 && (WaitAnim ? isFire && WaitAnim : true);
        //총알이 있니?
        if (fire) {
            animtest.ResetTrigger("Fire2");
            animtest.SetTrigger("Fire2");
            //action();
            isFire = false;
            StartCoroutine(FireWait());
            //총알 이팩트를 생성한다
            //(생성 코드)

            //그리고 Ray를 발사한다,.(정확하게 하자면, 총구 위치에서 Ray가 됨.)
            //그리고 사거리.
            //근데 Ray는 직선이니까 하나를 더 쏘거나 아니면 그냥 각도를 내리거나. 1번째 방법이 
            //더 낫겠지.
            //노말백터
            Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.white,1); Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.red,1);
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
        return isFire;
    }

    private void OnEnable()
    {
        isFire = true;
    }


    //여기는 자동 Reload 가 없습니다
    public bool Reload() {
        //탄창이 0보다 크고 현재 총알이 최대 총알보다 작을때 장전을 할수있음!
        if (currentManganize > 0 && currentBullet < maxBullet) {
            Debug.Log("Reloading!!!");
            currentBullet = maxBullet;
            currentManganize--;
            return true;
        }
        return false;
        
    }

    IEnumerator FireWait() {
        //animtest.ResetTrigger("Fire2");
        yield return new WaitForSeconds(fireTime);
        
        isFire = true;
    }
}
