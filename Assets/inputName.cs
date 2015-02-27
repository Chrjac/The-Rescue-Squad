using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class inputName : MonoBehaviour {

	public GameObject mppanel;
	public GameObject mppanelBrowser;

	public InputField playername;
	public string sSave;
	public Text tSave;
	
	public void SetSavedName(){
		tSave = GameObject.FindGameObjectWithTag("PlayerName").GetComponent<Text>();
		sSave = playername.text;
		tSave.text = playername.text;
		if (sSave == "") {
			tSave.text = "Please enter a Nickname";
				}
		else{
			mppanel.SetActive (false);
			mppanelBrowser.SetActive (true);
		}
	}
}
