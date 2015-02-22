using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

	private float lifeTime = 10;
	private Material mat;
	private Color originalCol;
	private float fadePercent;
	private float shellTime;
	private bool fading;





	void Start () {
		mat = renderer.material;
		originalCol = mat.color;
		shellTime = Time.time + lifeTime;

		StartCoroutine("ShellFade");
	}
	IEnumerator ShellFade()
	{
		while (true) 
		{
			yield return new WaitForSeconds (.2f);
			if(fading){
				fadePercent += Time.deltaTime;
				mat.color = Color.Lerp(originalCol, Color.clear, fadePercent);

				if (fadePercent >= 1) {
				Destroy (gameObject);
				}
			}
		
			else{
				if(Time.time > shellTime)
				{
					fading = true;
				}
			}
		}
	}
	void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Ground")
		{
			rigidbody.Sleep();
		}
	}
}
