using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class PlayerTest1 : MonoBehaviourPun,IPunObservable
{
    //Test Text
    public Transform trBody;
    public Transform RightHand;
    public Text NickNameText;

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

    //�������� �ϴ� ��ü
    public GameObject throwObject;
    public Stratagems C_Stratagem {
        get { return current_stratagem; }
        set { current_stratagem = value;
            //��Ʈ��Ÿ�� �ִϸ��̼� �� ��´�.
            //anim.SetTrigger("Grenade");

            photonView.RPC(nameof(PlayAnim), RpcTarget.All, "Grenade");
            photonView.RPC(nameof(test1), RpcTarget.All, C_Stratagem.id);
            //currentGun.gameObject.SetActive(false);
            //GameObject stratagemobj = PhotonNetwork.Instantiate("Stratagem",trBody.position,Quaternion.identity);  //Instantiate(stratagemObj, trBody.position + Vector3.up ,trBody.rotation);'

            //Stratagems stratagem = stratagemobj.GetComponent<Stratagems>();
            //stratagem = value;

        }
    }

    //���� : ��Ʈ��Ÿ�� �ڵ带 �Է� �� �����µ� 
    //�� PC������ ������ �������� �ƴµ�
    //��� PC������ ������ �������� ���� ��Ʈ��Ÿ�� ID�� ���� ������.
    //������,�����Ǵ°��� ������ ���� �ʴ´�.,
    string id;
    [PunRPC]
    public void test1(string s) {
        id = s;
    }



    //������ �ִϸ��̼��� ���� �� �� �Լ��� ����ȴ�. 
    public void FireGrenade() {
        Debug.Log(id);
        Throw(id);
    }
    [PunRPC]
    public void Throw(string name) {
        //��� Throw�ִϸ��̼��� ������ ������ Ŭ���̾�Ʈ������ ����
        //������ PhotonInstantiate�� ��� PC���� �����ϴϱ� �� ������ ����.
        //������ ��ġ�� ����ȭ�ϴ°�
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject stratagemobj = PhotonNetwork.Instantiate(name, RightHand.position, Quaternion.identity); //Instantiate((GameObject)Resources.Load("str01"), RightHand.position, Quaternion.identity); //
            //�׷��ϱ� �̳��� ����並 �����ͼ� ��� PC�� RPC�� �Ѵ�! 
            PhotonView view = stratagemobj.GetComponent<PhotonView>();
            //��Ʈ��Ÿ���� ������ ����� forward�� up ������ �޾Ƽ� ���������� ���� �ο���
            //RPC�� �ּ�ȭ�ϴ°� �����̳�, ��Ʈ��Ÿ���� ���� ȣ����� ����.
            view.RPC("Throw", RpcTarget.All, trBody.forward,trBody.up);

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

    private void Awake()
    {
        Debug.Log("�����ũ �Լ� ����!!");
        
        //PlayerManager.instace.action();
    }
    void Start()
    {
        if(NickNameText)
            NickNameText.text = photonView.Owner.NickName;
        ch = GetComponent<CharacterController>();
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
                PlayerManager.instace.DeathList.Add(this);
            }
        
        };


        if (photonView.IsMine) {
            PlayerManager.instace.action = null;
        }
        PlayerManager.instace.Addlist(this);
    }

    Vector3 last;

    public int layer = 0;

    Coroutine currCoroutine;
    private void LateUpdate()
    {
        last = transform.position;
    }
    float h = 0;
    float v = 0;
    private Vector3 targetPsition;
    private Quaternion targetRotation;

    public CharacterController ch;

    Vector2 myDir;
    void Update()
    {
        ch.Move(Vector3.up * -9.81f * Time.deltaTime);
        //transform.position += Vector3.up * -9.81f * Time.deltaTime;
        myDir = new Vector2(trBody.forward.x, trBody.forward.z);
/*        Vector3 test = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 pos = transform.position;
        //Debug.Log(test);
        if (test.x < 0.05) {
            test.x = 0.05f;
            //pos = Camera.main.ViewportToWorldPoint(test);
            //pos.x = 0.95f;
            pos.x += Time.deltaTime * 5;
            transform.position = pos;
            //Camera.main.GetComponent<FollowCam>().Iscam = false;
        }

        if (test.x > 0.95)
        {
            //Camera.main.GetComponent<FollowCam>().Iscam = false;
            //pos.x -= Time.deltaTime * 2;
            test.x = 0.95f;
            pos.x -= Time.deltaTime * 5;
            pos = Camera.main.ViewportToWorldPoint(test);
            //pos.x = 0.95f;
            transform.position = pos;

            return;
        }
        //Debug.Log(gameObject.name +  test);
        if (test.y < 0.05f) {
            test.y = 0.05f;
            pos = Camera.main.ViewportToWorldPoint(test);
            pos.y = 0;
            pos.z += Time.deltaTime * 5;
            transform.position = pos;
        }
        if(test.y > 0.95)
        {
            test.y = 0.95f;
            pos = Camera.main.ViewportToWorldPoint(test);
            pos.y = 0;
            pos.z -= Time.deltaTime * 5;
            transform.position = pos;
            return;
        }*/
        if (currentState == PlayerState.Die) {
            return;
        }
        //test
        //PlayerUI.instance.ManganizeText.text = currentGun.currentManganize.ToString();
        //PlayerUI.instance.BulletText.text = currentGun.maxBullet + " / " + currentGun.currentBullet;

        Vector3 dir = Vector3.right * h + Vector3.forward * v;
        dir.Normalize();
        if (photonView.IsMine)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            dir = Vector3.right * h + Vector3.forward * v;
            dir.Normalize();
            speed = 4;
            if (Input.GetMouseButton(0) && !reload) {
                           
                
                    
                
            }

            if (Input.GetMouseButton(0) && !reload)
            {
                if (current_stratagem)
                {
                    photonView.RPC(nameof(PlayAnim), RpcTarget.All, "Throw");

                }
                else {
                    int rand = Random.Range(-1, 2);
                    photonView.RPC(nameof(Fire), RpcTarget.All, rand);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                
                //CancelInvoke("ResetSpread");
                if(currCoroutine != null)
                {
                    StopCoroutine(currCoroutine);
                }
                anim.SetBool("Fire", false);
                //Invoke("ResetSpread", 0.5f);
                currCoroutine = StartCoroutine(ResetSpread());
                Debug.LogWarning("Invoke ȣ��");

            }

            //��Ʈ�� Ű�� �������� ��Ʈ��Ÿ�� �Է��� �ް�ʹ�.
            if (Input.GetKey(KeyCode.LeftControl))
            {
                PlayerUI.instance.StratagemImage.gameObject.SetActive(true);
                //�Է� �ڵ带 �Է��Ҷ�
                code_input.input(() => {
                    code_input.IsInput = !stratagemManager.Isreturn;

                    SoundManager.instance.SfxPlay(PlayerSound.instance.GetClip(PlayerSound.P_SOUND.Input));

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


            //������ Ű�� ������ Gun���� ������ �Ҽ� �ִ��� �����.
            if (Input.GetKeyDown(KeyCode.R) && currentGun.ReloadAble())
            {
                //�ִϸ��̼��� ������ ������ ����ȴ�.
                //���� - > iDLE
                photonView.RPC(nameof(PlayAnim), RpcTarget.All, "Reload");
                reload = true;
                
            }


            //1���� ������ ���� ����
            if (Input.GetKey(KeyCode.Alpha1))
            {
                ChangeGun(mainGun);
            }
            //2���� ������ ���� ����
            if (Input.GetKey(KeyCode.Alpha2))
            {
                ChangeGun(subGun);
            }



            //������
            //���� �ʱ�ȭ �Ѵ�.
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                PlayerUI.instance.StratagemImage.gameObject.SetActive(false);
                code_input.init();
                stratagemManager.init();
            }

            //������!
            if (Input.GetKeyDown(KeyCode.E) && currentGemObj != null)
            {
                photonView.RPC(nameof(GetItem), RpcTarget.All);
            }

            if (dir.sqrMagnitude > 0)
            {
                //SmoothDemp?
                // transform.forward = dir;
                if (Input.GetKey(KeyCode.LeftShift))
                {

                    speed = 10;
                    
                }



                // �̵� ����� Body �� ������ ������ ���̰��� ������
                float angle = Vector3.Angle(dir, trBody.right);

                //���࿡ ������ 90���� ������ �������� ȸ��
                //���ۿ� ȸ���� �������� ������ �ȴ�.
                if (!(Vector3.Magnitude(trBody.forward - dir) < 0.1))
                {
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
            //Debug.LogError(speed);
            //transform.position += dir * speed * Time.deltaTime;
            ch.Move(dir * speed * Time.deltaTime);
        }
        //End Ming
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPsition, Time.smoothDeltaTime * 20);
            trBody.rotation = Quaternion.Lerp(trBody.rotation,  targetRotation, Time.smoothDeltaTime * 20);
        }


        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);
        anim.SetFloat("speed",dir.magnitude);
        anim.SetFloat("RunSpeed", speed);
        anim.SetFloat("MyHorizontal",myDir.x);
        anim.SetFloat("MyVertical",myDir.y);
    }

    [PunRPC]
    public void Fire(int rand) {
        photonView.RPC("Res_Spr",RpcTarget.All);
        //Debug.LogError(rand);
        currentGun.Fire(rand);
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
        
        //RPC �Լ� �ּ�ȭ
        if (Input.GetButtonUp("Fire2"))
        {
            photonView.RPC(nameof(PlayAnim), RpcTarget.All, "RifleAiming", false);

        }
        if (Input.GetButtonDown("Fire2")) { 
            photonView.RPC(nameof(PlayAnim), RpcTarget.All, "RifleAiming", true);
        }
        //���콺 ��Ŭ��
        if (Input.GetButton("Fire2"))
        {
            //���� ���� ����ִٸ� 
            //���� �ִϸ��̼��� �����մϴ�.

            //���� ���� �ѱⰡ �������̶�� ������ �ִϸ��̼��� �����ϰ�
            //���� �ѱⰡ �ǽ����̶�� �ǽ��� �ִϸ��̼��� �����Ѵ�.

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


            //RPC ��� ����. ���� ���� �߻�!!

            //  photonView.RPC(nameof(PlayAnim), RpcTarget.All, "MyHorizontal", myDir.x);
           // photonView.RPC(nameof(PlayAnim), RpcTarget.All, "MyVertical", myDir.y);
            /*anim.SetFloat("MyHorizontal", myDir.x);
            anim.SetFloat("MyVertical", myDir.y);*/

        }

    }



    //RPC�� �κ�ũ�� ����Ҽ������Ϥ���(�κ�ũ�� �ȵǾ �ڷ�ƾ���� �ۼ� �� �ȵ���?)
    public IEnumerator ResetSpread() {
        yield return new WaitForSeconds(0.5f);
        Debug.LogWarning("RESETSPR�κ�ũȣ��");
        photonView.RPC(nameof(ResetSpreadRPC), RpcTarget.All);
        //currCoroutine = null;
    }

    //��� PC�� ���� �� �������� �ֿ��� �׳׵� �� ��������!
    [PunRPC]
    public void GetItem() {
        //������ ������ Ŭ���̾�Ʈ�ų� �� ��ü�� ������ �����Ѱ����� ���� ���� 
        //������ PC�� �ִ³� �̰��� �����ϸ� �ȴ�!
        if(PhotonNetwork.IsMasterClient && currentGemObj != null)
            currentGemObj.Add();
    }

    [PunRPC]
    public void ResetSpreadRPC() {
        Debug.LogWarning("RPC");
        currentGun.ResetSpread();
    }


    public enum AnimationType { 
        Trigger,Bool,Float
    }
    [PunRPC]
    public void PlayAnim(string name) {
        anim.SetTrigger(name);
    }

    [PunRPC]
    public void PlayAnim(string name,float value)
    {
        //Debug.Log("HELLO!" + value);
        anim.SetFloat(name,value);
    }

    [PunRPC]
    public void PlayAnim(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    [PunRPC]
    public void Res_Spr()
    {
        CancelInvoke("ResetSpread");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //�ʰ� ����
        if (stream.IsWriting)
        {
            stream.SendNext(h);
            stream.SendNext(v);
            stream.SendNext(speed);
            stream.SendNext(transform.position);
            stream.SendNext(trBody.rotation);
            
        }
        //������
        else {
            
            h = (float)stream.ReceiveNext();
            v = (float)stream.ReceiveNext();
            speed = (float)stream.ReceiveNext();
            targetPsition = (Vector3)stream.ReceiveNext();
            targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }


    //�÷��̾ ������ ��� ����
    private void OnDestroy()
    {
        PlayerManager.instace.PLAYER_LIST.Remove(this);
    }
}
