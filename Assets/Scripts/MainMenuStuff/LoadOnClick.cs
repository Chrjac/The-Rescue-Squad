using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

	public GameObject loadingImage;
	public GameObject mppanel;

	public void SelectName(){
		mppanel.SetActive (true);
		Debug.Log("Loading MP panel");
	}
	public void LoadGame(int level){
		Application.LoadLevel (level);
	}
}
