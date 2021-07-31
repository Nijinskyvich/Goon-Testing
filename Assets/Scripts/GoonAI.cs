using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;




public class GoonAI : MonoBehaviour
{

    public NavMeshAgent agent;
    //= GetComponent<NavMeshAgent>
    public Vector3 FeetPosition;
    public float range = 10;
    public Vector3 randomPoint;
    public ThirdPersonCharacter character;
    public float health = 50f;
    public Rigidbody mainbody;
    public string status = "Stationary";
    public GameObject target;
    public float damage = 10;
    private float dist;
    private float lastAttackTime = 0;
    float attackCoolDown = 2;
    public float stopDistance = 5;

    void SetKinematic(bool state)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider cd in colliders)
        {   
            if (cd.isTrigger == false)
                {
                cd.enabled = state;
            }
            
        }
        GetComponent<Collider>().enabled = !state;
    }



    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    void Start()
    {
        SetKinematic(true);
        setColliderState(false);
        FeetPosition = transform.position;
        agent.updateRotation = false;
        target = GameObject.FindGameObjectWithTag("Player");
    }



    void Update()
    {
        if (status == "Chase")
        {
            dist = Vector3.Distance(transform.position, target.transform.position);
            
            if(dist < stopDistance)
            {
                agent.isStopped = true;
                if (Time.time - lastAttackTime > attackCoolDown)
                {
                    target.GetComponent<PlayerCollision>().TakeDamage(10);
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
                
                

            }
            

        }

        if (status == "Random")
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                if (RandomPoint(FeetPosition, range, out Vector3 destination))
                {
                    agent.SetDestination(destination);
                }
            }
        }

        if (status == "Idle")
        {
            agent.isStopped = true;
        }

        character.Move(agent.desiredVelocity, false, false);
        FeetPosition = transform.position;
    }

    public void TakeDamage (float amount)
    {
        health -= amount;
        if (health <=0f)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<ThirdPersonCharacter>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Animator>().enabled = false;
        SetKinematic(false);
        setColliderState(true);
        Destroy(gameObject, 5);
        TurnOff();
    }

    public void TurnOff()
    {
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Axe")
        {
            Die();
        }
    }
}
