using UnityEngine;
using System.Collections;

public class sc : MonoBehaviour {

	// Use this for initialization


	// Update is called once per frame
	void Update(){
		if (Input.GetMouseButtonDown(1)) {
			gameObject.AddComponent<Rigidbody> ();
		}
	}
}
