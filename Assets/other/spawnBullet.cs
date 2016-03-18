using UnityEngine;
using System.Collections;

public class spawnBullet : MonoBehaviour {


	public GameObject prefab;
	public float respawnRate;

	private float wait;
	private Vector3 pos;
	// Use this for initialization
	void Start () {
		pos=GameObject.Find("Player").GetComponent<Transform>().position;
		wait = respawnRate;
	}

	// Update is called once per frame
	void Update () {
		if(wait <= 0.0)
		{
			//prefab.addComponent<>
			//spawn it;

			transform.LookAt(pos);

			Vector3 spawnpoint = transform.position+transform.forward*1.2f*Vector3.Magnitude(transform.localScale);
			//Debug.Log (spawnpoint);
			GameObject clone=Instantiate(prefab, spawnpoint , transform.rotation) as GameObject;
			Rigidbody cloneRigidBody = clone.GetComponent<Rigidbody> ();
			//Rigidbody clone=Instantiate(prefab, new Vector3(0, 0,0), Quaternion.identity) as Rigidbody;
			//clone.velocity = transform.TransformDirection(Vector3.right * 10);
			//Vector3.Lerp(clone.GetComponent<Transform>().position, pos,3);
			//position final - position inital -> this will give a unit vector
			//Vector3 v=(pos-clone.GetComponent<Transform> ().position )/Vector3.Magnitude(pos-clone.GetComponent<Transform> ().position );
			//Debug.Log(v);
			//for now, leave this

			//clone.velocity = transform.TransformDirection(  new Vector3(v.x,v.y,v.z) *10.0f);

			cloneRigidBody.freezeRotation = true;
			cloneRigidBody.velocity = 20 * clone.transform.forward;
			//clone.velocity = transform.TransformDirection(  new Vector3(0,0,1) *10);
			//clone.velocity = transform.TransformDirection(new Vector3(0,1,0) * 10);
			wait = respawnRate;
			Destroy (clone.gameObject, 100.0f);
		}
		else
		{
			wait -= Time.deltaTime;
		}

	}
}
