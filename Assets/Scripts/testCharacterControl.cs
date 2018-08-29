using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//requirements:
//can be moved with key board controls, or bluetooth controller
//

[RequireComponent(typeof(Animator))]

public class testCharacterControl : MonoBehaviour {
	public InputAccumulator inputInterface;
	public float runSpeed = 5.0F;
	public float jumpSpeed = 10.0F;
	public float turnSpeed = 10.0F;
	public float gravity = 9.8F;
	public float fatalVelocity = 34.0F;
	public float terminalVelocity=100.0F;
	public float fallDistThreshold = 20.0F;
	public float animRunScale = 1.0F;
	public float rotationAnimationSensitivity=10.0F; //to do, test this against headset and a mouse

	private float verticalVelocity = 0.0F;
	public bool grounded = false;
	CharacterController controller;
	public Animator animator;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 rotDirection = Vector3.zero;

	public int a, b, s;
	public float x, y, z, roll, tilt, yaw;
	private float prevYaw = 0.0F;

	private bool jumpPlaying;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
		animator = GetComponent<Animator> ();
		inputInterface = new InputAccumulator ();
		inputInterface.addModule (GetComponent<KeyboardMouseModule> ());
		a = b = s = 0;
		x = y = z = roll = tilt = yaw = 0.0F;
	}

	void FixedUpdate(){

		//transform.Translate (new Vector3 (v * Time.deltaTime, h * Time.deltaTime));
	}

	public IEnumerator jumpRoutine(){
		jumpPlaying = true;
		animator.SetBool ("Jump", true);
		yield return new WaitForEndOfFrame ();
		animator.SetBool ("Jump", false);
		yield return new WaitForEndOfFrame ();
		jumpPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		y = inputInterface.getY ();
		x = inputInterface.getX ();
		b = inputInterface.getB ();
		a = inputInterface.getA ();
		s = inputInterface.getSelect ();
		roll = inputInterface.getRoll ();
		tilt = inputInterface.getTilt ();
		yaw = inputInterface.getYaw ();

		//calculate the differential of Yaw with respect to time
		float dWdT = Mathf.Abs (yaw - prevYaw);
		if (dWdT > 90.0) {
			dWdT = 0.0F;
		} else {
			dWdT /= Time.deltaTime;
		}
		prevYaw = yaw;

		if (y < 0) //you can only go forward
			y = 0;
		
		animator.SetFloat ("Speed", y * runSpeed);
		animator.SetFloat ("AV", Mathf.Abs( ( dWdT) * rotationAnimationSensitivity) );

		rotDirection = controller.transform.localEulerAngles;

		rotDirection.y += (yaw * turnSpeed);
		controller.transform.localEulerAngles = rotDirection;

		CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime );
		//grounded = ((flags & CollisionFlags.Below) != 0);
		//animator.SetBool ("Grounded", grounded);
		if (controller.isGrounded) {
			animator.SetBool ("Grounded", true);
			verticalVelocity = -gravity * Time.deltaTime;
			if (a > 0 && !jumpPlaying) {
				print ("jumping!");
				StartCoroutine ("jumpRoutine");
				verticalVelocity = jumpSpeed;
			}
		} else {
			animator.SetBool ("Grounded", false);
			if ( verticalVelocity > terminalVelocity ) {
				verticalVelocity -= gravity * Time.deltaTime;
			}
		}
		//print ("vv="+verticalVelocity+", tv="+terminalVelocity);
		//transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.x+Input.GetAxis("Mouse X") * speed, transform.localEulerAngles.y);
	}


	void OnAnimatorMove(){
		if (animator) {
			moveDirection = new Vector3(x, 0, y);

			moveDirection *= animator.GetFloat("Speed") * animRunScale;
			moveDirection = transform.TransformDirection(moveDirection);

			moveDirection.y = verticalVelocity;
			//print (moveDirection.y);
		}
	}
}
