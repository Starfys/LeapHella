using UnityEngine;
using System.Collections;

public class spawnShield : MonoBehaviour {
	public GameObject shield;
	public Vector3 offSet;
	//bool isdown;
	// Use this for initialization
	void Start () {
		//isdown = false;

	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown ("a")/* && isdown==false*/) {
			GameObject sInst = Instantiate (shield, transform.position + offSet, Quaternion.identity)as GameObject;
			sInst.name = "_shield";
		//	isdown = true;
		} 
	}
}
