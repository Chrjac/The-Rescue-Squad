using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent (typeof(AudioSource))]
public class Gun : MonoBehaviour {

	public Transform bulletSpawn;
	public enum GunType{Semi, Auto};
	public GunType gunType;
	public float rpm;
	public float damage = 1;

	public LayerMask collisionMask;

	private LineRenderer tracer;

	private float fireRate;
	private float cooldown;

	public Transform shellEjector;
	public Rigidbody shell;
	public float gunId;
	public float reloadTime = 5;

	public Text ammo;
	public Text mags;
	public Text fireSelector;

	//Ammo
	public int totalAmmo = 120;
	public int ammoMag = 30;
	private int currentAmmo;

	private bool reloading;

	[HideInInspector]
	public GameGUI gui;
	public Slider slider;
	public float bulletspread = 0.1f;


	void Start()
	{
		fireSelector = GameObject.FindGameObjectWithTag("FireSelecter").GetComponent<Text>();
		ammo = GameObject.FindGameObjectWithTag("Ammo").GetComponent<Text>();
		mags = GameObject.FindGameObjectWithTag("Mags").GetComponent<Text>();
		fireRate = 60/rpm;
		if(GetComponent<LineRenderer>())
		{
			tracer = GetComponent<LineRenderer>();
		}
		fireSelector.text = "Auto";
		currentAmmo = ammoMag;
		ammo.text = currentAmmo.ToString();
		mags.text = totalAmmo.ToString();
		/*if(gui){
			gui.SetAmmoInfo(totalAmmo, currentAmmo);
		}
		*/
	}
	void Update(){
			if (Input.GetButtonDown ("Selecter")) {
				if (gunType == GunType.Semi) {
					gunType = GunType.Auto;
					fireSelector.text = "Auto";
					
				} 
				else {
					gunType = GunType.Semi;
					fireSelector.text = "Semi";
				}
			}

	}

	public void Shoot()
	{
		Vector3 spread = bulletSpawn.forward;
		spread.x += Random.Range (-bulletspread, bulletspread);
		spread.z += Random.Range (-bulletspread, bulletspread);


		if (CanShoot ()){
		Ray ray = new Ray (bulletSpawn.position, spread);
		RaycastHit hit;

		float shotDistance = 20;

		if(Physics.Raycast(ray, out hit, shotDistance, collisionMask))
			{
			shotDistance = hit.distance;
				if(hit.collider.GetComponent<Entity>()){
					hit.collider.GetComponent<Entity>().TakeDamage(damage);
				}
			}
		cooldown = Time.time + fireRate;
		currentAmmo --;
		ammo.text = currentAmmo.ToString();
		mags.text = totalAmmo.ToString();
		

			/*if(gui){
			gui.SetAmmoInfo(totalAmmo, currentAmmo);
			}
			*/

			audio.Play();


			if (tracer)
			{
				StartCoroutine("RenderTracer", ray.direction * shotDistance);
			}

			Rigidbody newShell = Instantiate(shell, shellEjector.position, Quaternion.identity) as Rigidbody;
			newShell.AddForce(shellEjector.forward * Random.Range(150f,200f) + bulletSpawn.forward * Random.Range (-10f, 10f));
		}
	}
	public void ShootAuto()
	{
		if (gunType == GunType.Auto) 
		{
			Shoot ();
		}
	}
	private bool CanShoot()
	{
		bool canShoot = true;

		if (Time.time < cooldown) 
		{
			canShoot = false;
		}

		if (currentAmmo == 0) 
		{
			canShoot = false;
		}
		if (reloading) 
		{
			canShoot = false;
		}

		return canShoot;
	}
	public bool Reload()
	{

		if (totalAmmo != 0 && currentAmmo != ammoMag){
			reloading = true;
			return true;



		}
		return false;
	}



	public void FinishedReloade()
	{

		reloading = false;
		currentAmmo = ammoMag;
		totalAmmo -= ammoMag;

		if (totalAmmo < 0) 
		{
			currentAmmo += totalAmmo;
			totalAmmo = 0;
		}
		ammo.text = currentAmmo.ToString();
		mags.text = totalAmmo.ToString();
		/*if(gui){
			gui.SetAmmoInfo(totalAmmo, currentAmmo);
		}
		*/
	}
	IEnumerator RenderTracer(Vector3 hitPoint)
	{
		tracer.enabled = true;
		tracer.SetPosition(0, bulletSpawn.position);
		tracer.SetPosition(1, bulletSpawn.position + hitPoint);
		//Fjerner tracer etter 1 frame
		yield return null;
		tracer.enabled = false;
	}
}
