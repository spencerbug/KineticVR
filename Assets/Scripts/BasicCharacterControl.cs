using UnityEngine;
using System.Collections;

public class BasicCharacterControl : MonoBehaviour {

	Animator animator;
	Rigidbody rigidbody;
	Collider collider;
	public float h;
	public float v;
	public int j;
	public int windedState;
	public bool jumpPlaying=false;
	public float rotspeed = 90;
	public bool isGrounded = true;
	private CapsuleCollider cCollider;
	private float cHeight; // the capsule's starting height
	private float yHeight; // the capsule's starting y center
	private bool managedHitsRunning=false;

	void OnCollisionEnter(Collision collision){
		isGrounded = true;
		if (collision.transform.tag == "Projectile") {
			StartCoroutine ("manageHits2");
		}

	}

	public IEnumerator manageHits(){
		if (managedHitsRunning){
			yield return null;
		}
		managedHitsRunning=true;
		animator.SetBool("Hit", true);
		yield return new WaitForEndOfFrame();
		animator.SetBool("Hit", false);

		cCollider.height = cHeight * 0.75f; // shorten the collider
		cCollider.center = new Vector3(cCollider.center.x, yHeight * 0.75f, cCollider.center.z); // raisethe collider
		yield return new WaitForSeconds(0.7f);
		cCollider.height = cHeight; // set the height back to original
		cCollider.center = new Vector3(cCollider.center.x, yHeight, cCollider.center.z);
		managedHitsRunning=false;
	}

	public IEnumerator manageHits2(){
		if (managedHitsRunning){
			yield return null;
		}
		managedHitsRunning=true;
		animator.SetBool("Hit", true);
		yield return new WaitForEndOfFrame();
		animator.SetBool("Hit", false);
		yield return new WaitForSeconds(0.7f);
		managedHitsRunning=false;
	}

	public IEnumerator processWinded(){
		if (windedState != 0) {//already in process
			yield return null;
		}
		windedState = 1;
		// trigger the Catch Breath state on
		animator.SetInteger("Winded State", 1);
		yield return new WaitForEndOfFrame (); //give it a frame
		animator.SetInteger("Winded State", 2);
		// let it run for a random # seconds
		yield return new WaitForSeconds(Random.Range(3.0f, 5.5f));
		windedState = 0; //clear Winded flag
		animator.SetInteger("Winded State", 0);
	}

	public IEnumerator jumpRoutine(){
		jumpPlaying = true;
		animator.SetBool ("Jump", true);
		yield return new WaitForEndOfFrame ();
		animator.SetBool ("Jump", false);
		yield return new WaitForEndOfFrame ();
		jumpPlaying = false;
	}

	void FixedUpdate() {
		//print ("2: "+ v + " " + j + " " + isGrounded + " " + !jumpPlaying );
		AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo (0);
		animator.SetFloat ("V Input", v);
		animator.SetFloat ("Turning", h);
		if (windedState == 0  && isGrounded ) {
			transform.Rotate (new Vector3(0, h*Time.deltaTime*rotspeed, 0));
		}
		if (stateinfo.IsName ("Base Layer.Idle")) {
			if (j == 1 && isGrounded && !jumpPlaying && stateinfo.IsTag ("Jumpable")) {
				rigidbody.AddForce (Vector3.up * 200);
				isGrounded = false;
				StartCoroutine ("jumpRoutine"); 
			}
		}
		else if (animator.GetInteger ("Race State") == 3) {
			if (j == 1 && isGrounded && !jumpPlaying && stateinfo.IsTag ("Jumpable")) {
				print (v + " " + j + " " + isGrounded + " " + !jumpPlaying );
				rigidbody.AddForce (Vector3.up * 300);
				isGrounded = false;
				StartCoroutine ("jumpRoutine"); 
			}
		}
		if (stateinfo.IsName("Base Layer.Hit")) {
			cCollider.height = cHeight + animator.GetFloat("Collider Y");
//			cCollider.center = new Vector3(cCollider.center.x, yHeight * animator.GetFloat("Collider Y"), cCollider.center.z); // raisethe collider
		}

		//animator.SetInteger ("Jump", j);
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		rigidbody = GetComponent<Rigidbody> ();
		collider = GetComponent<Collider> ();
		cCollider = GetComponent<CapsuleCollider> ();
		cHeight = cCollider.height;
		yHeight = cCollider.center.y;

		windedState = animator.GetInteger ("Winded State");
		int layers = animator.layerCount;
		if (layers >= 2) {
			for (int i = 0; i < layers; i++) {
				animator.SetLayerWeight (i, 1f);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (windedState == 2){
			Input.ResetInputAxes (); // block player input
		}
		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");
		j = (int)Input.GetAxis ("Jump");
//		if (Input.GetAxis ("Fire3") == 1) {
//			StartCoroutine ("processWinded");
//		}
	}
}
