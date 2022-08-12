using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StarterAssets{
public class RagdollScript : MonoBehaviour
{
    #region ReferenceVariable
    public Collider mainCollider; //capsules collider
   // public Collider[] col; // collider that are in child objects Ex - hips,spine etc..
    public Animator animator;//Animator commponent
    public Rigidbody[] rb; //rigidbody compoments that are in child object ex -hips,spine etc. 
    public NavMeshAgent agent;
    public EnemyController enemyController;
    public TargetScript1 targetScript;
    public GameObject BloodSplatter;
    GameObject BloodSplatterPos;
    GameObject EnemyDot;
    Rigidbody rigidbody1;
    [HideInInspector]
    public bool isZombieDead=false;
    #endregion

    void Start()
    {
        EnemyDot=transform.GetChild(2).gameObject;
        BloodSplatterPos=transform.GetChild(1).gameObject;
        rb=GetComponentsInChildren<Rigidbody>();
        animator=GetComponent<Animator>();
        rigidbody1=GetComponent<Rigidbody>();
        isZombieDead=false;
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        isZombieDead=false;
        foreach(var rigidbodies in rb)
        {
            rigidbodies.isKinematic=true;
        }
        animator.enabled=true;

    }

    public IEnumerator ActivateRagdoll()
    {
        isZombieDead=true;
        foreach(var rigidbodie in rb)
        {
            rigidbodie.isKinematic=false;
        }
        animator.enabled=false;
        agent.enabled=false;
        enemyController.enabled=false;
        mainCollider.enabled=false;
        targetScript.enabled=false;
        rigidbody1.useGravity=false;
        yield return new WaitForSeconds(1.5f);
        EnemyDot.SetActive(false);
        yield return new WaitForSeconds(4f);
        Instantiate(BloodSplatter,(new Vector3(BloodSplatterPos.transform.position.x,
                                               BloodSplatterPos.transform.position.y-0.14f,
                                               transform.position.z))
                                               ,Quaternion.Euler(new Vector3(90f,0,0))
                                );
        
    }
}
}
