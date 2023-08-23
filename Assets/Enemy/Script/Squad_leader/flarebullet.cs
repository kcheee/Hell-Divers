using UnityEngine;
using System.Collections;
using static UnityEngine.ParticleSystem;
using DG.Tweening;
using Photon.Pun;

public class flarebullet : MonoBehaviourPun {
			
	private Light flarelight;
	private AudioSource flaresound;
	private ParticleSystemRenderer smokepParSystem;
	private ParticleSystem particle;
	private ParticleSystem.MainModule mainModule;


    private bool myCoroutine;
	private float smooth = 1.5f;
	public 	float flareTimer = 9;
	public AudioClip flareBurningSound;
	public int spawnPos;

	Rigidbody rb;

	IEnumerator spawn()
	{
		yield return new WaitForSeconds(2);
        float angle = 360 / spawnPos;
        Transform tf = gameObject.transform;
        Vector3 FlarePo = new Vector3(transform.position.x,0,transform.position.z);
        for (int i = 0; i < spawnPos; i++)
        {
            Vector3 Po = FlarePo + new Vector3(Random.Range(-4f, 4),0, Random.Range(-4f, 4));

            PhotonNetwork.Instantiate("Initiate_E-main", Po, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(0.2f, 1f));
        }
    }

	// Use this for initialization
	void Start () {

		// 카메라 쉐이크 모두 되게 설정해야함.
		Camera.main.transform.DOShakePosition(0.5f, 0.3f);
		StartCoroutine("flareLightoff");

		StartCoroutine(spawn());

		GetComponent<AudioSource>().PlayOneShot(flareBurningSound);
		flarelight = GetComponent<Light>();
		flaresound = GetComponent<AudioSource>();
		smokepParSystem = GetComponent<ParticleSystemRenderer>();
		particle=GetComponent<ParticleSystem>();
		mainModule = particle.main;
        rb = GetComponent<Rigidbody>();
		// 임시로
		rb.AddForce(transform.up*15+transform.right*Random.Range(-2,2),ForceMode.Impulse);

		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		
		if (myCoroutine == true)		
		{
			//flarelight.intensity = Random.Range(2f,6.0f);			
		}else	
		{
			// 플레어건 천천히 사라지게
            flarelight.intensity = Mathf.Lerp(flarelight.intensity, 0f, Time.deltaTime * smooth);
            flarelight.range = Mathf.Lerp(flarelight.range, 0f, Time.deltaTime * smooth);
            flaresound.volume = Mathf.Lerp(flaresound.volume, 0f, Time.deltaTime * smooth);
            smokepParSystem.maxParticleSize = Mathf.Lerp(smokepParSystem.maxParticleSize, 0f, Time.deltaTime * 5);
            //mainModule.startSize = Mathf.Lerp(particle.main.startSizeMultiplier, 0, Time.deltaTime * 8);
        }
		if (rb.velocity.y < 0f)
		{
			//mainModule.startSize = Mathf.Lerp(particle.main.startSizeMultiplier, 0, Time.deltaTime * 1.2f);
			rb.drag = 10;
		}

		if(flarelight.range<=0.2f)
			mainModule.startSize = Mathf.Lerp(particle.main.startSizeMultiplier, 0, Time.deltaTime * 1.5f);
        
		if(mainModule.startSizeMultiplier<=0.4f)
		{
			Destroy(gameObject);
		}
	}

    IEnumerator flareLightoff()
	{
		myCoroutine = true;
		yield return new WaitForSeconds(flareTimer);
		myCoroutine = false;
	}
}
