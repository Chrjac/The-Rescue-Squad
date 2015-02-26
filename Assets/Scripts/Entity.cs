using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Entity : MonoBehaviour {

	public float health;
	public Slider slider;
	public Slider sliderstam;
	public Text hp;

	public int a = 0;
	public int stam = 100;
	public bool running = false; 
	private bool resting = false;

	void Start(){
		hp = GameObject.FindGameObjectWithTag("hp").GetComponent<Text>();
		if (sliderstam) {
			sliderstam.value = stam;
		}
	}
	void Update(){

		if (sliderstam && Input.GetButton ("Run") && !resting) {
			if(sliderstam.value < 2){
				resting = true;
			}
			sliderstam.value -= Time.deltaTime*20;
			running = true;
		} 
		else if(sliderstam && sliderstam.value < 100){
			if(sliderstam.value > 10){
				resting = false;
			}
			sliderstam.value += Time.deltaTime*5;
			running = false;
		}
		if (slider) {
				
		slider.value = health;
		hp.text = health.ToString();
		} 
		}
	[RPC]
	public virtual void TakeDamage(float dmg){
		health -= dmg;

		if (health <= 0) {
			Die();
			if(slider){
			slider.value = 0;
			}
			hp.text = a.ToString() ;
		}
	}
	public virtual void Die(){
		if (GetComponent<PhotonView> ().instantiationId == 0) {
			Destroy (gameObject);
		} 
		else {
			if(PhotonNetwork.isMasterClient){
			PhotonNetwork.Destroy (gameObject);
			}
		}
	}





}