using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	//Handler
	public float rotationSpeed = 450;
	public float walkSpeed = 5;
	public float runSpeed = 8;
	private float acceleration = 5;
	private Vector3 currentVelocityMod;

	public Quaternion targetRotation;
	
	private CharacterController controller;
	private Camera cam;
	public Gun[] guns;
	private Gun currentGun;

	private bool reloading;
	public Transform handHold;



	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
		cam = Camera.main;
		EquipGun (0);
	}
	
	// Update is called once per frame
	void Update () {
		ControlMouse ();
		//ControlWasd ();

		//Gun Input
		if (currentGun) {
				
			if (Input.GetButtonDown ("Shoot")) {
				currentGun.Shoot ();
			} 
			else if (Input.GetButton ("Shoot"))
			{
				currentGun.ShootAuto();
			}
			if (Input.GetButtonDown ("Reload"))
			{
				reloading = true;
			}
			if (reloading)
			{

			}
		}
		for (int i = 0; i<guns.Length; i++)
		{
			if(Input.GetKeyDown((i+1) + "") || Input.GetKeyDown("[" + (i+1) + "]")){
				EquipGun(i);
				break;
			}
		}
	}
	void EquipGun(int i)
	{
		if (currentGun) 
		{
			Destroy(currentGun.gameObject);
		}
		currentGun = Instantiate(guns[i], handHold.position, handHold.rotation) as Gun;
		currentGun.transform.parent = handHold;
	}
	void ControlMouse(){

		Vector3 mousePos = Input.mousePosition;
		mousePos = cam.ScreenToWorldPoint(new Vector3 (mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));
		targetRotation = Quaternion.LookRotation (mousePos - new Vector3(transform.position.x,0, transform.position.z));
		transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);



		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		currentVelocityMod = Vector3.MoveTowards (currentVelocityMod, input, acceleration * Time.deltaTime);
		Vector3 motion = currentVelocityMod;
		motion *= (Mathf.Abs (input.x) == 1 && Mathf.Abs (input.z) == 1)?.7f:1;
		motion *= (Input.GetButton("Run"))?runSpeed:walkSpeed;
		motion += Vector3.up * -8;

		controller.Move (motion * Time.deltaTime);


	}
	void ControlWasd(){
		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		
		if (input != Vector3.zero) {
			
			targetRotation = Quaternion.LookRotation (input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
		}
		
		Vector3 motion = input;
		motion *= (Mathf.Abs (input.x) == 1 && Mathf.Abs (input.z) == 1)?.7f:1;
		motion *= (Input.GetButton("Run"))?runSpeed:walkSpeed;
		motion += Vector3.up * -8;
		controller.Move (motion * Time.deltaTime);

	}
}
