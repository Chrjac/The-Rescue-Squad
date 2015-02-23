using UnityEngine;
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

	//Ammo
	private int totalAmmo = 30;
	private int ammoMag = 10;
	private int currentAmmo;

	private bool reloading;

	[HideInInspector]
	public GameGUI gui;



	void Start()
	{
		fireRate = 60/rpm;
		if(GetComponent<LineRenderer>())
		{
			tracer = GetComponent<LineRenderer>();
		}
		currentAmmo = ammoMag;
		/*if(gui){
			gui.SetAmmoInfo(totalAmmo, currentAmmo);
		}
		*/
	}


	public void Shoot()
	{
		if (CanShoot ()){
		Ray ray = new Ray (bulletSpawn.position, bulletSpawn.forward);
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
