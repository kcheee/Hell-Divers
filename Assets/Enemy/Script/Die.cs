using Photon.Pun.Demo.Cockpit;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Die : MonoBehaviour
{
    Animator myAnim;
    BipedIK iK;
    public List<Rigidbody> ragdollRigids;
    public List<Limb> limbRigids;
    bool die;
    public GameObject eft;
    void Start()
    {
        myAnim = GetComponent<Animator>();
        iK = GetComponent<BipedIK>();
        ragdollRigids = new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>());
        ragdollRigids.Remove(GetComponent<Rigidbody>());

        limbRigids = new List<Limb>(transform.GetComponentsInChildren<Limb>());
        limbRigids.Remove(GetComponent<Limb>());

        DeactiveRagdoll(true);
        //ActivateRagdoll();
    }

   public IEnumerator delay()
    {

        ActivateRagdoll();
        int rand = Random.Range(0, limbRigids.Count);
        Limb limb = GetComponent<Limb>();
        for (int i = 0; i < rand; i++)
        {
            int r = Random.Range(0, limbRigids.Count);
            limbRigids[r].GetComponent<Limb>().GetHit();
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.parent = limbRigids[r].transform;
            go.transform.localScale = Vector3.one * 0.1f;
            go.transform.position = limbRigids[r].GetComponent<Limb>().transform.position;

            GameObject T = Instantiate(eft, limbRigids[r].GetComponent<Limb>().transform.position, Quaternion.identity);
            T.transform.parent = limbRigids[r].transform;
        }
        //limbRigids[rand].GetComponent<Limb>().GetHit();
        //limbRigids[rand].transform.position
        yield return new WaitForSeconds(2);
        DeactiveRagdoll(false);
        yield return new WaitForSeconds(1);
        die = true;
        yield return new WaitForSeconds(4);
        Destroy(gameObject);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(delay());
        }
        if (die)
        {
            transform.position += Vector3.down * Time.deltaTime * 0.2f;
        }
    }

    void ActivateRagdoll()
    {
        myAnim.enabled = false;
        if (iK != null)
            iK.enabled = false;

        for (int i = 0; i < ragdollRigids.Count; i++)
        {
            ragdollRigids[i].useGravity = true;
            ragdollRigids[i].isKinematic = false;
        }
    }

    void DeactiveRagdoll(bool flag)
    {
        myAnim.enabled = flag;
        if(iK != null)
        iK.enabled = flag;
        for (int i = 0; i < ragdollRigids.Count; i++)
        {
            ragdollRigids[i].useGravity = false;
            ragdollRigids[i].isKinematic = true;
        }
    }
}
