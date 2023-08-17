using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTest1 : MonoBehaviour
{
    //Test Text
    public Transform trBody;
    public float speed = 5;
    public Gun currentGun;
    public Gun mainGun;
    public Gun subGun;
    public bool reload = false;
    Animator anim;

    //���� ������ �ٰ����� Ȱ��ȭ �Ǿ��ִ� ������Ʈ
    public AudioClip testclip;
    public AudioClip testclip2;
    public GameObject stratagemObj;
    public Stratagems current_stratagem;


    public Stratagems C_Stratagem {
        get { return current_stratagem; }
        set { current_stratagem = value;
            //��Ʈ��Ÿ�� �ִϸ��̼� �� ��´�.
            GameObject stratagemobj = Instantiate(stratagemObj, trBody.position + Vector3.up ,trBody.rotation);
            Stratagems stratagem = stratagemobj.GetComponent<Stratagems>();
            stratagem = value;
            Rigidbody arbody = stratagemobj.GetComponent<Rigidbody>();
            arbody.AddForce(trBody.forward * 7 + trBody.up * 5, ForceMode.Impulse);
        }
    }

    public I_StratagemObject currentGemObj;

    public Code_InputManager code_input;
    public StratagemManager stratagemManager;
    public PlayerHP playerHp;
    public enum PlayerState { 
        Live,Die
    }
    public PlayerState currentState = PlayerState.Live;
    void Start()
    {
        anim = trBody.GetComponent<Animator>();
        //test
       // PlayerUI.instance.ManganizeText.text = currentGun.currentManganize.ToString();
        //PlayerUI.instance.BulletText.text = currentGun.maxBullet + " / " + currentGun.currentBullet;

        //����, mine �̶��
        //add component.
        code_input = gameObject.AddComponent<Code_InputManager>();
        stratagemManager = GetComponent<StratagemManager>();
        playerHp = GetComponent<PlayerHP>();
        playerHp.Ondie = () => {
            if (currentState != PlayerState.Die) {
                anim.SetTrigger("Die"); 
                currentState = PlayerState.Die;
                PlayerManager.instace.PlayerList.Remove(this);
            }
        
        };
    }

    Vector3 last;

    public int layer = 0;
    private void LateUpdate()
    {
        last = transform.position;
    }
    float h = 0;
    float v = 0;
    void Update()
    {

        Vector3 test = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 pos = transform.position;
        //Debug.Log(test);
        if (test.x < 0.05) {
            test.x = 0.05f;
            pos = Camera.main.ViewportToWorldPoint(test);
            //pos.x = 0.95f;
            pos.x -= Time.deltaTime * 5;
            transform.position = pos;
            Camera.main.GetComponent<FollowCam>().Iscam = false;
        }

        if (test.x > 0.95)
        {
            Camera.main.GetComponent<FollowCam>().Iscam = false;

            Debug.Log("ss");

            //pos.x -= Time.deltaTime * 2;
            test.x = 0.95f;
            pos.x -= Time.deltaTime * 5;
            pos = Camera.main.ViewportToWorldPoint(test);
            //pos.x = 0.95f;
            transform.position = pos;

            return;
        }

        if (test.y < 0.5f) {
            test.y = 0.05f;
            //pos = Camera.main.ViewportToWorldPoint(test);
            //pos.x = 0.95f;
            pos.z -= Time.deltaTime * 5;
            transform.position = pos;
        }
        if(test.y > 0.95)
        {
            test.y = 0.05f;
            //pos = Camera.main.ViewportToWorldPoint(test);
            //pos.x = 0.95f;
            pos.z -= Time.deltaTime * 5;
            transform.position = pos;
            return;
        }
        if (currentState == PlayerState.Die) {
            return;
        }
        //test
        //PlayerUI.instance.ManganizeText.text = currentGun.currentManganize.ToString();
        //PlayerUI.instance.BulletText.text = currentGun.maxBullet + " / " + currentGun.currentBullet;

        if (layer != 1) {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            Debug.Log("sS" + gameObject.name);
        }

        if (layer == 1) {
            if (Input.GetKeyDown(KeyCode.T)) {
                v = 1;
                Debug.Log("sS");
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                v = -1;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                h = 1;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                h = -1;
            }
            if (Input.GetKeyUp(KeyCode.T))
            {
                v = 0;
                Debug.Log("sS");
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                v = 0;
            }
            if (Input.GetKeyUp(KeyCode.H))
            {
                h = 0;
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                h = 0;
            }






        }


        Vector3 dir = Vector3.right * h + Vector3.forward * v;

        //
        dir.Normalize();
        speed = 4;
        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);
        anim.SetFloat("speed", dir.magnitude);
        anim.SetFloat("RunSpeed", speed);


        //��Ʈ�� Ű�� �������� ��Ʈ��Ÿ�� �Է��� �ް�ʹ�.
        if (Input.GetKey(KeyCode.LeftControl))
        {
            PlayerUI.instance.StratagemImage.gameObject.SetActive(true);
            //�Է� �ڵ带 �Է��Ҷ�
            code_input.input(() => {
                code_input.IsInput = !stratagemManager.Isreturn;
                //(���� ���⼭ ���� �ϴ°� �ƴ�. �׽�Ʈ��.)
                SoundManager.instance.Play(testclip2);
                //�ڵ尡 ��¥ �ڵ�� �´��� ��� Ȯ�����ش�.
                int count = code_input.KeyInputList.Count - 1;
                List<KeyType.Key> list = code_input.KeyInputList;
                Stratagems stratagem = stratagemManager.CompareCode(list, count);
                if (stratagem)
                {
                    C_Stratagem = stratagem;
                    code_input.init();
                    stratagemManager.init();
                }

            }); //end lambda.
            return;
        } //end Input.
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

        /*Vector3 playerPosition = transform.position;
        Camera mainCamera = Camera.main;
        float moveSpeed = 10;
        // ī�޶� �þ��� ��踦 ���մϴ�.
        float cameraDistance = mainCamera.transform.position.y - transform.position.y;
        float cameraHalfHeight = Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * cameraDistance;
        float cameraHalfWidth = cameraHalfHeight * mainCamera.aspect;

        // �÷��̾��� �̵� �Է��� �޽��ϴ�.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // �÷��̾��� ��ġ�� ������Ʈ�մϴ�.
        float newPosX = playerPosition.x + moveHorizontal * moveSpeed * Time.deltaTime;
        float newPosZ = playerPosition.z + moveVertical * moveSpeed * Time.deltaTime;

        // ī�޶� �þ� ���� �����մϴ�.
        float clampedX = Mathf.Clamp(newPosX, mainCamera.transform.position.x - cameraHalfWidth, mainCamera.transform.position.x + cameraHalfWidth);
        float clampedZ = Mathf.Clamp(newPosZ, mainCamera.transform.position.z - cameraHalfHeight, mainCamera.transform.position.z + cameraHalfHeight);

        // ���ѵ� ��ġ�� �÷��̾��� ��ġ�� ������Ʈ�մϴ�.
        transform.position = new Vector3(clampedX, playerPosition.y, clampedZ);*/



        transform.position += dir * speed * Time.deltaTime;
        
        if (Input.GetMouseButton(0) && !reload)
        {
            CancelInvoke("ResetSpread");
            //anim.SetTrigger("Fire2");
            bool IsAnim = currentGun.Fire();
            
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("ResetSpread");
            anim.SetBool("Fire", false);
            Invoke("ResetSpread",0.5f);
            
        }


        //������ Ű�� ������ Gun���� ������ �Ҽ� �ִ��� �����.
        if (Input.GetKey(KeyCode.R) && currentGun.ReloadAble()) {
            //�ִϸ��̼��� ������ ������ ����ȴ�.
            //���� - > iDLE
            anim.SetTrigger("Reload");
            reload = true;
            SoundManager.instance.Play(testclip);
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



        //������
        //���� �ʱ�ȭ �Ѵ�.
        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            PlayerUI.instance.StratagemImage.gameObject.SetActive(false);
            code_input.init();
            stratagemManager.init();
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
                //�ϴ� �̷��� �ϰ� �����սô�.
                trBody.forward = target;
                //trBody.forward = Vector3.Lerp(trBody.forward,target,Time.deltaTime * 5);
            }

            Vector2 myDir = new Vector2(trBody.forward.x, trBody.forward.z);
            anim.SetFloat("MyHorizontal", myDir.x);
            anim.SetFloat("MyVertical", myDir.y);

        }

    }


    public void ResetSpread() {
        currentGun.ResetSpread();
    }
}
