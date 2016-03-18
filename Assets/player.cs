
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class player : MonoBehaviour {
	public float Health;
	// Use this for initialization
	void Start () {
		GameObject.Find ("PlayerGui").GetComponent<Text>().text="Health : " + Health.ToString();

	}

	// Update is called once per frame
	void Update () {

	}
	void OnCollisionEnter(Collision col){
		//Debug.Log ("HERE");
		Health -= 10;	
		GameObject.Find ("PlayerGui").GetComponent<Text>().text="Health : " + Health.ToString();
		Destroy (col.gameObject);
	}

}