using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class waypoints : MonoBehaviour {
	public List<GameObject> waypointsList= new List<GameObject>();

	private bool isLoop;
	private int i;
	// Use this for initialization
	void Start () {
		i = 0;
		transform.position=Vector3.Lerp (transform.position,waypointsList [i].GetComponent<Transform> ().position , 0.10F);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position=Vector3.Lerp (transform.position,waypointsList [0].GetComponent<Transform> ().position , 0.10F);
		if (waypointsList.Count > 2) {
			//Debug.Log (waypointsList [1].GetComponent<Transform> ().position);
			Vector3 pos = transform.position;
			Vector3 nextPos = waypointsList [i].GetComponent<Transform> ().position;
			transform.position = Vector3.Lerp (pos, nextPos, 0.02F);
			if ( isCloseEnough(pos,nextPos,0.2f)) {
				//Vector3 pos = waypointsList [i % waypointsList.Count].GetComponent<Transform> ().position;
				i = (i+ 1) % (waypointsList.Count);
				//Debug.Log (i);

			}
				
		}
	}
	bool isCloseEnough(Vector3 fro, Vector3 to, float near){
		return Mathf.Abs(Vector3.Magnitude(to-fro))< near;
	}
}
