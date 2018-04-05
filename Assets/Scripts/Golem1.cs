using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem1 : MonoBehaviour {
	Rigidbody rb;
	protected Animator animator;
	bool canMove = true;
	bool isBlocking = false;
	public bool isDead = false;
	bool canAction = true;
	public Camera sceneCamera;
	float dv;
	float dh;
	float x;
	float z;
	Vector3 inputVec;
	Vector3 newVelocity;

	string tipo = "Normal";
	public Material normal;
	public Material arctic;
	public Material desert;

	public float walkSpeed = 1.35f;
	public float runSpeed = 6f;
	float rotationSpeed = 10f;

	bool isGrounded = true;

	bool isStrafing = false;

	float moveSpeed;

	//jumping variables
	public float gravity = -9.8f;
	bool canJump =true;
	bool isJumping = false;
	public float jumpSpeed = 12;
	public float doublejumpSpeed = 6;
	bool doublejumping = true;
	bool canDoubleJump = false;
	bool isDoubleJumping = false;
	bool doublejumped = false;
	bool isFalling;
	bool startFall;
	float fallingVelocity = -1f;

	// Used for continuing momentum while in air
	public float inAirSpeed = 8f;
	float maxVelocity = 2f;
	float minVelocity = -2f;

	public AudioSource audioPasos;
	public AudioSource otrosAudios;
	AudioClip[] sonidos;

    RaycastHit hit;
    public Transform objectPosition;
    public Transform PutObjectPosition;

    bool cargandoObjeto = false;
    GameObject objeto;

    public bool objetoEnFrente = false;




    SkinnedMeshRenderer my_rend;


	public Transform rockThrowingPosition;
	public GameObject rock;
	public Transform respawnCharacter;
	// Use this for initialization
	void Start () {

		my_rend = GetComponentInChildren<SkinnedMeshRenderer>();
		animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (animator) {
			if (canMove && !isBlocking && !isDead ) {
				CameraRelativeMovement ();
			}
			Jumping ();
			if(Input.GetButtonDown("AttackL") && canAction && isGrounded && !isBlocking)
			{
				Attack();
			}


		}else
		{
			Debug.Log("ERROR: There is no animator for character.");
		}
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Debug.DrawRay(pos, transform.forward, Color.red);
        if (Physics.Raycast(pos, transform.forward, out hit, 3.5f))
        {
            objetoEnFrente = true;
            GameObject obj = hit.collider.gameObject;
            //if (obj.tag == "chest") {
            //StartCoroutine (_OpenChest ());
            //}
            if ((obj.tag == "movableObject" || obj.tag == "key") && !isDead)
            {
                if (Input.GetButtonUp("CogerObjeto"))
                {
                    this.objeto = obj;
                    this.objeto.transform.position = objectPosition.position;
                    cargandoObjeto = true;
                }
            }
        }
        else
        {
            objetoEnFrente = false;
        }
    }

	void FixedUpdate()
	{
		CheckForGrounded ();
		//apply gravity force
		rb.AddForce(0, gravity, 0, ForceMode.Acceleration);

		AirControl();
		if(canMove && !isBlocking && !isDead)
		{
			moveSpeed = UpdateMovement();
		}

		//check if falling
		if(rb.velocity.y < fallingVelocity )
		{
			isFalling = true;
			//animator.SetInteger("Jumping", 2);
			canJump = false;
		} 
		else
		{
			isFalling = false;
		}
        if (Input.GetButtonDown("CogerObjeto") && cargandoObjeto && !objetoEnFrente && !isDead)
        {
            this.objeto.transform.position = PutObjectPosition.position;
            cargandoObjeto = false;
            this.objeto = null;
        }

    }

	void LateUpdate()
	{
		//Get local velocity of charcter
		float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
		float velocityZel = transform.InverseTransformDirection(rb.velocity).z;

		//if character is alive and can move, set our animator
		if(!isDead && canMove) 
		{
			if (moveSpeed > 0) {
				animator.SetBool ("Moving", true);
				if (!audioPasos.isPlaying && isGrounded){
					audioPasos.Play ();
				}
			} 
			else 
			{
				animator.SetBool("Moving", false);
				audioPasos.loop = false;
			}
		}

        if (cargandoObjeto)
        {
            objeto.transform.position = objectPosition.position;
        }
  

    }
	void CameraRelativeMovement(){
		float inputDashVertical = Input.GetAxisRaw("DashVertical");
		float inputDashHorizontal = Input.GetAxisRaw("DashHorizontal");
		float inputHorizontal = Input.GetAxisRaw("Horizontal");
		float inputVertical = Input.GetAxisRaw("Vertical");

		//converts control input vectors into camera facing vectors
		Transform cameraTransform = sceneCamera.transform;
		//Forward vector relative to the camera along the x-z plane   
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		//Right vector relative to the camera always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		//directional inputs
		dv = inputDashVertical;
		dh = inputDashHorizontal;
		x = inputHorizontal;
		z = inputVertical;
		inputVec = x * right + z * forward;
	}

	float UpdateMovement()
	{
		CameraRelativeMovement();  
		Vector3 motion = inputVec;
		if(isGrounded)
		{
			//reduce input for diagonal movement
			if(motion.magnitude > 1)
			{
				motion.Normalize();
			}
			if(canMove && !isBlocking)
			{
				//set speed by walking / running
				if(isStrafing)
				{
					newVelocity = motion * walkSpeed;
				}
				else
				{
					newVelocity = motion * runSpeed;
				}

			}
		}
		else
		{
			//if we are falling use momentum
			newVelocity = rb.velocity;
		}
		if(!isStrafing || !canMove)
		{
			RotateTowardsMovementDir();
		}
		/*if(isStrafing && !isRelax)
		{
			//make character point at target
			Quaternion targetRotation;
			Vector3 targetPos = target.transform.position;
			targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x,0,transform.position.z));
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,(rotationSpeed * Time.deltaTime) * rotationSpeed);
		}*/
		newVelocity.y = rb.velocity.y;
		rb.velocity = newVelocity;
		//return a movement value for the animator
		return inputVec.magnitude;
	}

	void RotateTowardsMovementDir()
	{
		if(inputVec != Vector3.zero && !isStrafing && !isBlocking)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Artic") {
			tipo = "Arctic";
			my_rend.material = arctic;
			jumpSpeed = 8f;
			runSpeed = 12f;
		}
		if (other.gameObject.tag == "Desert") {

			tipo = "Desert";
			my_rend.material = desert;
			runSpeed = 8f;
			jumpSpeed = 14f;
		}
		if (other.gameObject.tag == "Normal") {
			tipo = "Normal";
			my_rend.material = normal;
			runSpeed = 6f;
			jumpSpeed = 12f;
		}
	}


	void Jumping()
	{
		if(isGrounded && !isDead)
		{
			if(canJump && Input.GetButtonDown("Jump"))
			{
				//otrosAudios.Play ();
				StartCoroutine(_Jump());
			}
		}
		else
		{    
			canDoubleJump = true;
			canJump = false;
			if(isFalling && !isDead)
			{
				//set the animation back to falling
				//animator.SetInteger("Jumping", 2);
				//prevent from going into land animation while in air
				if(!startFall)
				{
					//animator.SetTrigger("JumpTrigger");
					startFall = true;
				}
			}
				
			if(canDoubleJump && doublejumping && Input.GetButtonDown("Jump") && !doublejumped && isFalling && !isDead && tipo == "Desert")
			{
				// Apply the current movement to launch velocity
				rb.velocity += doublejumpSpeed * Vector3.up;
				animator.SetInteger("Jumping", 3);
				animator.SetTrigger("JumpTrigger");
				doublejumped = true;
			}
		}
			
	}
	void Attack()
	{
		if (canAction) {
			if (tipo == "Normal") {
				animator.SetTrigger ("AttackTrigger");
				StartCoroutine (_LockMovementAndAttack (0, 1.7f));
			} else if (tipo == "Arctic") {
				
				animator.SetTrigger ("AttackTriggerA");
				StartCoroutine (_Shoot());

				StartCoroutine (_LockMovementAndAttack (0, 2.35f));
			}
		}
	}




	void CheckForGrounded()
	{
		float distanceToGround;
		float threshold = .6f;
		RaycastHit hit;
		Vector3 offset = new Vector3 (0, .4f, 0);
		//Debug.DrawLine (transform.position + offset,transform.position ,Color.red);
		if(Physics.Raycast ((transform.position + offset), -Vector3.up, out hit, 100f)) 
		{
			distanceToGround = hit.distance;
			if (distanceToGround < threshold) {
				if (rb.velocity.y > -20) {
					isGrounded = true;
					canJump = true;
					startFall = false;
					doublejumped = false;
					canDoubleJump = false;
					isFalling = false;
					if (!isJumping && !isDead) {
						animator.SetInteger ("Jumping", 0);
					}
				} else {
					//StartCoroutine(_Death());
				}
			} 
			else 
			{
				isGrounded = false;
			}
		}

	}

	void AirControl()
	{
		if(!isGrounded)
		{
			CameraRelativeMovement();
			Vector3 motion = inputVec;
			motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? 0.7f:1;
			rb.AddForce(motion * inAirSpeed, ForceMode.Acceleration);
			//limit the amount of velocity we can achieve
			float velocityX = 0;
			float velocityZ = 0;
			if(rb.velocity.x > maxVelocity)
			{
				velocityX = GetComponent<Rigidbody>().velocity.x - maxVelocity;
				if(velocityX < 0)
				{
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.x < minVelocity)
			{
				velocityX = rb.velocity.x - minVelocity;
				if(velocityX > 0)
				{
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.z > maxVelocity)
			{
				velocityZ = rb.velocity.z - maxVelocity;
				if(velocityZ < 0)
				{
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
			if(rb.velocity.z < minVelocity)
			{
				velocityZ = rb.velocity.z - minVelocity;
				if(velocityZ > 0)
				{
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "KillZone" && !isDead) {
			StartCoroutine (_Death());
		}else if (other.gameObject.tag == "Arctic floor" && tipo!= "Arctic" && !isDead) {
			StartCoroutine (_Death());
		}
	}

	IEnumerator _Jump()
	{
		isJumping = true;
		animator.SetInteger("Jumping", 2);
		animator.SetTrigger("JumpTrigger");
		// Apply the current movement to launch velocity
		rb.velocity += jumpSpeed * Vector3.up;
		canJump = false;
		yield return new WaitForSeconds(.5f);
		isJumping = false;
	}

	IEnumerator _Death()
	{
		animator.SetTrigger("DeathTrigger");
		StartCoroutine(_LockMovementAndAttack(.1f, 1.5f));
		isDead = true;
        if (cargandoObjeto) {
            cargandoObjeto = false;
            this.objeto = null;

        }
		animator.SetBool("Moving", false);
		inputVec = new Vector3(0, 0, 0);
		yield return new WaitForSeconds(2.5f);
		this.transform.position = respawnCharacter.position;
		animator.SetTrigger ("IdleTrigger");
		isDead = false;
	}

	IEnumerator _Shoot(){
		yield return new WaitForSeconds(1.5f);
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			rock,
			rockThrowingPosition.position,
			rockThrowingPosition.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 50;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 10.0f); 
	}

	//method to keep character from moveing while attacking, etc
	public IEnumerator _LockMovementAndAttack(float delayTime, float lockTime)
	{
		yield return new WaitForSeconds(delayTime);
		canAction = false;
		canMove = false;
		animator.SetBool("Moving", false);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		inputVec = new Vector3(0, 0, 0);
		animator.applyRootMotion = true;
		yield return new WaitForSeconds(lockTime);
		canAction = true;
		canMove = true;
		animator.applyRootMotion = false;

	}
	void FootL(){
	}

	void FootR(){
	}

	void Jump(){
	}
	void Land(){
	}

}
