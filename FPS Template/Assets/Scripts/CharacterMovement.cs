using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour 
{
	public float runSpeed=6.0f;
	public float walkSpeed=11.0f;
	public bool limitDiagonalSpeed= true;
	public bool toggleRun= false;
	public float jumpSpeed= 8.0f;
	public float gravity= 20.0f;
	public float fallingDamageThreshold= 10.0f;
	public bool slideWhenOverSlopeLimit= false;
	public bool slideOnTaggedObjects= false;
	public float slideSpeed= 12.0f;
	public bool airControl= false;
	public float antiBumpFactor= .75f;
	public int antiBunnyHopFactor= 1;

	private Vector3 moveDirection= Vector3.zero;
	private bool grounded= false;
	private CharacterController controller;
	private Transform myTransform;
	private float speed;
	private RaycastHit hit;
	private float fallStartLevel;
	private bool falling= false;
	private float slideLimit;
	private float rayDistance;
	private Vector3 contactPoint;
	private bool playerControl= false;
	private int jumpTimer;
	
	public float curStam = 100.0f;
	public float maxStam = 100.0f;
	public bool  isSprinting = false;
	public Texture2D StaminaBarFG;
	public Texture2D StaminaBarBG;
	public float staminaBarLength;
	public float percentOfStamina;
	public bool  stamCooldown = false;
	public Vector3 oldPos;
	
	void  Start ()
	{
		controller = GetComponent<CharacterController>();
		myTransform = transform;
		speed = walkSpeed;
		rayDistance = controller.height * .5f + controller.radius;
		slideLimit = controller.slopeLimit - .1f;
		jumpTimer = antiBunnyHopFactor;
		oldPos = transform.position;
	}
	void  FixedUpdate ()
	{
		float inputX= Input.GetAxis("Horizontal");
		float inputY= Input.GetAxis("Vertical");
		float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? .7071f : 1.0f;
		
		if (grounded) 
		{
			bool sliding= false;
			if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance))
			{
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
					sliding = true;
			}
			else
			{
				Physics.Raycast(contactPoint + Vector3.up, -Vector3.up,out hit);
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
					sliding = true;
			}
			if (falling)
			{
				falling = false;
				if (myTransform.position.y < fallStartLevel - fallingDamageThreshold)
					FallingDamageAlert(fallStartLevel - myTransform.position.y);
			}
			if (!toggleRun)
				speed = Input.GetButton("Run")? runSpeed : walkSpeed;
			if ((sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide"))
			{
				Vector3 hitNormal = hit.normal;
				moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
				Vector3.OrthoNormalize(ref hitNormal, ref moveDirection);
				moveDirection *= slideSpeed;
				playerControl = false;
			}
			else
			{
				
				moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
				moveDirection = myTransform.TransformDirection(moveDirection) * speed;
				playerControl = true;
				
			}
			if (!Input.GetButton("Jump"))
				jumpTimer++;
			else if (jumpTimer >= antiBunnyHopFactor)
			{
				moveDirection.y = jumpSpeed;
				jumpTimer = 0;
			}
		}
		else
		{
			if (!falling)
			{
				falling = true;
				fallStartLevel = myTransform.position.y;
			}
			if (airControl && playerControl)
			{
				moveDirection.x = inputX * speed * inputModifyFactor;
				moveDirection.z = inputY * speed * inputModifyFactor;
				moveDirection = myTransform.TransformDirection(moveDirection);
			}
		}
		moveDirection.y -= gravity * Time.deltaTime;
		grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
	}
	void  Update ()
	{

		if (toggleRun && grounded && Input.GetButtonDown("Run"))
			speed = (speed == walkSpeed? runSpeed : walkSpeed);
		percentOfStamina = curStam/maxStam;
		staminaBarLength = percentOfStamina*100;
		if (Input.GetKey("left shift") && curStam > 0 && stamCooldown == false)
		{
			runSpeed = 20.0f;
			isSprinting = true;
		}
		else
		{
			
			runSpeed = walkSpeed;
			isSprinting = false;
			
		}
		if (isSprinting == true && curStam >= 0)
		{
			curStam--;
		}
		if(isSprinting == false && curStam < 100.0f)
		{
			curStam++;
		}
		if(curStam == 0)
		{
			stamCooldown = true;
		}
		if(curStam == 100.0f)
		{
			stamCooldown = false;
		}
	}
	void  OnControllerColliderHit ( ControllerColliderHit hit  )
	{
		contactPoint = hit.point;
	}
	void  FallingDamageAlert ( float fallDistance  )
	{
		Debug.Log ("Ouch! Fell " + fallDistance + " units!");
	}
	void OnGUI()
	{
		GUI.DrawTexture(new Rect((Screen.width / 2) - 100, 10, staminaBarLength * 2, 10), StaminaBarFG);
		GUI.DrawTexture(new Rect((Screen.width / 2) - 100, 10, 200, 10), StaminaBarBG);
	}
	
}