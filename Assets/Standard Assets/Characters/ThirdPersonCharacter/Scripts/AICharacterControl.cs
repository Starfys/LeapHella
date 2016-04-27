using System;
using UnityEngine;

[RequireComponent(typeof (NavMeshAgent))]
public class AICharacterControl : MonoBehaviour
{
    //For navigation
    public Transform target; // target to aim for
    private NavMeshAgent agent; //The navmeshagent
    //For health handling
    public float maxHealth;
    public float currentHealth;
    public GameObject healthBar;
    private Vector3 healthBarScale; 
    //For animation
    private Animator myAnim;

    public enum CharacterState
    {
        idle,
        walking,
        attacking,
        dead
    }

    public CharacterState curState;

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<NavMeshAgent>();
        myAnim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBarScale = new Vector3(1, 1, 1);
        agent.updateRotation = false;
        agent.updatePosition = true;
    }


    private void Update()
    {
        healthBarScale.x = Mathf.Clamp(currentHealth / maxHealth, 0.0f, maxHealth);
        healthBar.GetComponent<RectTransform>().localScale = healthBarScale;
        if (currentHealth <= 0.0f)
        {
            curState = CharacterState.dead;
        }
        switch (curState)
        {
            case CharacterState.idle:
                if (target != null)
                {
                    agent.SetDestination(target.position);
                    myAnim.Play("Walk");
                    curState = CharacterState.walking;
                }
                break;
            case CharacterState.walking:
                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    agent.Move(agent.desiredVelocity);
                    transform.LookAt(target);

                }
                else
                {
                    agent.Stop();
                    myAnim.Play("Attack");
                    curState = CharacterState.attacking;
                }
                break;
            case CharacterState.attacking:
                break;
            case CharacterState.dead:
                myAnim.Play("Death");
                Destroy(gameObject, 5.0f);
                break;
        }
  
    }
    //Returns the amount of overkill
    public float takeDamage(float amount)
    {
        //TODO:crits
        currentHealth -= amount;
        //Return the overkil
        if (currentHealth < 0)
        {
            return -currentHealth;
        }
        return 0f;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

}
