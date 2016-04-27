using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    //The enemy to spawn
    public GameObject enemy;
    //The number of enemies left to spawn
    public int enemiesLeft;
    //The delay betweeen enemy spawns
    public float spawnInterval;
    //The player
    public GameObject player;
    //Time until next spawn
    private float timeUntilNext;
    //A 3dtext used to display how many enemies are left
    public TextMesh remainingDisplay;
	// Use this for initialization
	void Start () {
        timeUntilNext = spawnInterval;
        remainingDisplay.text = "Enemies left: " + enemiesLeft.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        if (enemiesLeft > 0)
        {
            if (timeUntilNext <= 0)
            {
                timeUntilNext = spawnInterval;
                GameObject spawnedEnemy = Instantiate(enemy, transform.position + transform.forward, transform.rotation) as GameObject;
                spawnedEnemy.GetComponent<AICharacterControl>().SetTarget(player.transform);
                enemiesLeft--;
                remainingDisplay.text = "Enemies left: " + enemiesLeft.ToString();
            }
            else
            {
                timeUntilNext -= Time.deltaTime;
            }
        }
        else
        {
            remainingDisplay.text = "You have vanquished the enemy";
        }
	}
}
