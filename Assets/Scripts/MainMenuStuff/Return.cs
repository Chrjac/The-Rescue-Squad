using UnityEngine;
using System.Collections;

public class Return : MonoBehaviour {

	public GameObject mppanel;
	public GameObject mppanelBrowser;

	public void GoBack(){
		mppanel.SetActive (false);
		mppanelBrowser.SetActive (false);
	}
}
