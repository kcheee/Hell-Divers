using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTest1 : MonoBehaviour
{
    //Test Text
    public Text ManganizeText;
    public Text BulletText;


    public Transform trBody;
    public float speed = 5;
    public Gun currentGun;
    public Gun mainGun;
    public Gun subGun;
    public bool reload = false;
    Animator anim;

    //���� ������ �ٰ����� Ȱ��ȭ �Ǿ��ִ� ������Ʈ

    public GameObject Ammo;
    public I_StratagemObject currentGemObj;
    

    void Start()
    {
        anim = trBody.GetComponent<Animator>();
        //test
        ManganizeText.text = currentGun.currentManganize.ToString();
        BulletText.text = currentGun.maxBullet + " / " + currentGun.currentBullet;
    }

    void Update()
    {
        //test
        ManganizeText.text = currentGun.currentManganize.ToString();
        BulletText.text = currentGun.maxBullet + " / " + currentGun.currentBullet;


        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");



        Vector3 dir = Vector3.right * h + Vector3.forward * v;

        //
        dir.Normalize();
        speed = 4;
        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);
        anim.SetFloat("speed", dir.magnitude);
        anim.SetFloat("RunSpeed", speed);


        //���࿡ �����̰� �ִٸ�(sqr�� ��Ʈ ����)
        if (dir.sqrMagnitude > 0)
        {
            //SmoothDemp?
           // transform.forward = dir;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 10;
                anim.SetFloat("RunSpeed", speed);
            }

            // �̵� ����� Body �� ������ ������ ���̰��� ������
            float angle = Vector3.Angle(dir, trBody.right);

            //���࿡ ������ 90���� ������ �������� ȸ��
            //���ۿ� ȸ���� �������� ������ �ȴ�.
            if (! (Vector3.Magnitude(trBody.forward - dir) < 0.1)) {
                if (angle < 90)
                {
                    trBody.Rotate(new Vector3(0, 5, 0));
                    //Debug.Log(angle);
                }
                //�׷��������� �������� ȸ��
                else
                {
                    //Debug.Log(angle);
                    trBody.Rotate(new Vector3(0, -5, 0));
                }
            }
            

            //trBody.forward = Vector3.Lerp(trBody.forward, dir,Time.deltaTime * 20);

        }
        Aiming();
        transform.position += dir * speed * Time.deltaTime;

        if (Input.GetMouseButton(0) && !reload)
        {
            //anim.SetTrigger("Fire2");
            bool IsAnim = currentGun.Fire();
            
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("Fire", false);
        }


        //������ Ű�� ������ Gun���� ������ �Ҽ� �ִ��� �����.
        if (Input.GetKey(KeyCode.R) && currentGun.ReloadAble()) {
            //�ִϸ��̼��� ������ ������ ����ȴ�.
            //���� - > iDLE
            anim.SetTrigger("Reload");
            reload = true;
            
        }
        


        //1���� ������ ���� ����
        if (Input.GetKey(KeyCode.Alpha1)) {
            ChangeGun(mainGun);
        }
        //2���� ������ ���� ����
        if (Input.GetKey(KeyCode.Alpha2))
        {
            ChangeGun(subGun);
        }

        //��Ʈ�� Ű�� �������� ��Ʈ��Ÿ���� ȣ���Ҽ��ִ�.
        //�ϴ� �����ϰ� �ڽ��� ��ġ���� ������.
        if (Input.GetKeyDown(KeyCode.LeftControl)){
            GameObject ammo = Instantiate(Ammo,trBody.position + Vector3.up ,trBody.rotation);
            Rigidbody arbody = ammo.GetComponent<Rigidbody>();
            arbody.AddForce(trBody.forward * 7 + trBody.up * 5, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.E) && currentGemObj != null) {
            currentGemObj.Add();
        }

    }

    public void ChangeGun(Gun gun) {
        currentGun.gameObject.SetActive(false);
        currentGun = gun;
        currentGun.gameObject.SetActive(true);
    }
    public void test() {
        anim.SetBool("Fire", false);
    }

    public void Reloading() {
        currentGun.Reload();
    }

    public void Aiming() {
        if (Input.GetButtonUp("Fire2"))
        {
            anim.SetBool("PistolAiming", false);
            anim.SetBool("RifleAiming", false);
        }
        //���콺 ��Ŭ��
        if (Input.GetButton("Fire2"))
        {
            //���� ���� ����ִٸ� 
            //���� �ִϸ��̼��� �����մϴ�.

            //���� ���� �ѱⰡ �������̶�� ������ �ִϸ��̼��� �����ϰ�
            //���� �ѱⰡ �ǽ����̶�� �ǽ��� �ִϸ��̼��� �����Ѵ�.
            switch (currentGun.gunType)
            {
                case Gun.GunType.Rifle:
                    anim.SetBool("RifleAiming", true);

                    break;
                case Gun.GunType.Pistol:
                    anim.SetBool("PistolAiming",true);

                    break;
                
            }
            //anim.SetBool("Aiming", true);

            speed = 1;
            //���콺 ��ġ��
            Vector3 msPos = Input.mousePosition;

            //��ũ�� ��ġ�� �ٲٰ� �ű⿡ �ش��ϴ� ���̸� �����.
            Ray ray = Camera.main.ScreenPointToRay(msPos);
            RaycastHit hitInfo;
            //���̸� ���.
            if (Physics.Raycast(ray, out hitInfo))
            {

                //������ - �ڽ��� ��ġ�� target���� �Ѵ�. y���� ������� ������ 0���� �Ѵ�.
                Vector3 target = hitInfo.point - transform.position;
                target.y = 0;
                trBody.forward = target;
            }

        }

    }

}
