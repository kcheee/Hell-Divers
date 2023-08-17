
using System.Collections;
using UnityEngine;

public class EJBombFire : MonoBehaviour
{
    //bomb
    bool isBombDone = true;
    int bombCount = 10;

    //bombPos
    public Transform bombPos;
    Vector3 originBombAngle;
    GameObject bomb;
    GameObject bombMuzzleImpact;
    public GameObject bombHead;

    //bombMuzzleFX
    public GameObject bombMuzzleFactory;

    //bombReaction

    // Start is called before the first frame update
    void Start()
    {
        originBombAngle = bombPos.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (isBombDone)
            {
                StartCoroutine(MakeBomb(null));
            }
        }
    }

    public IEnumerator MakeBomb(System.Action<int> complete)
    {
        isBombDone = false;

        for (int i = 0; i < bombCount; i++)
        {
            //bomb 积己
            bomb = EJObjectPoolMgr.instance.GetbombQueue();

            bomb.transform.position = bombPos.position;
            bomb.transform.up = bombPos.transform.up;

            EJBossSFX.instance.PlaybombFlyingSFX();
            ONHeadAnim();

            //bombMuzzle 积己
            GameObject bombMuzzleImpact = Instantiate(bombMuzzleFactory);

            bombMuzzleImpact.transform.position = bombPos.position;
            bombMuzzleImpact.transform.localEulerAngles =bombPos.transform.parent.localEulerAngles;
            bombMuzzleImpact.transform.localScale = Vector3.one ;
            bombMuzzleImpact.transform.up = bombPos.transform.forward;

            //酿鸥烙
            yield return new WaitForSeconds(Random.Range(0.1f,0.3f));
            BossFSM.Sflag = false;
        }

        isBombDone = true;

        if(complete != null)
        {
            complete(0);
        }
    }

    public Animator headReaction;

    public void ONHeadAnim()
    {
        headReaction.SetTrigger("headFire");
    }

}