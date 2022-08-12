using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StarterAssets{
public class EnemyController : MonoBehaviour
{
    public float lookRadius =10f;
   public Transform target;
    NavMeshAgent agent;
    [HideInInspector]
    public Animator animator;
    PlayerHealthScript playerHealth;
    RagdollScript ragdollScript;
    public float DamageToPlayer;
    bool CanAttack=true;
    public GameObject UiBloodSplash;

    // Start is called before the first frame update
    void Start()
    {
        // target=PlayerManager.instance.player.transform;
       agent = GetComponent<NavMeshAgent>();
       animator=GetComponent<Animator>();
       playerHealth=FindObjectOfType<PlayerHealthScript>();
       ragdollScript=GetComponent<RagdollScript>();
    }

    // Update is called once per frame
    void Update()
    {

        float distance =Vector3.Distance(target.position,transform.position);
        if(distance<=lookRadius)
        {
            animator.SetBool("isChasing",true);
            animator.SetBool("isAttacking",false);
            agent.SetDestination(target.position);
            if(agent.isStopped)
               animator.SetBool("isChasing",false);
            if(distance<=agent.stoppingDistance)
            {          
                animator.SetBool("isChasing",false);
                FaceTarget();
                animator.SetBool("isAttacking",true);  
                StartCoroutine(Attack());
            }
            
            
        }
    }
    void FaceTarget()
    {
        Vector3 direction=(target.position-transform.position).normalized;
        Quaternion lookRotation=Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation=Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*5f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,lookRadius);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        if(CanAttack && !playerHealth.isPlayerDied &&!ragdollScript.isZombieDead)
        {
            CanAttack=false;
            yield return new WaitForSeconds(0.5f);
            UiBloodSplash.SetActive(true);
            playerHealth.TakeDamage(DamageToPlayer);
            yield return new WaitForSeconds(0.5f);
            UiBloodSplash.SetActive(false);
            yield return new WaitForSeconds(2f);
            CanAttack=true;
        }
    }

    public void setTrue()
    {
        animator.SetBool("isPlayerDead",true);
    }
}



}

