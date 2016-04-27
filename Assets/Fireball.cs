using UnityEngine;
using System.Collections;
using Leap;
using UnityEngine.UI;

public class Fireball : MonoBehaviour {
    //Time to live
    public float timeToLive = 5.0f;
    //Damage value
    public float damageValue = 10;
	// Use this for initialization
	void Start () {
        //For increased performance!
        Destroy(gameObject, timeToLive);
	}
	
	// Update is called once per frame
	void Update () {

	}
    void OnCollisionEnter(Collision col)
    {
        AICharacterControl enemyHealth = col.gameObject.GetComponent<AICharacterControl>();
        if (enemyHealth != null)
        {
            //Do damage and keep overkill
            damageValue = enemyHealth.takeDamage(damageValue);
            //If there was no overkill
             if(damageValue <= 0)
             {
                Destroy(gameObject);
             }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
