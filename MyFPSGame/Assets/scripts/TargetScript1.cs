using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace StarterAssets{
    public class TargetScript1 : MonoBehaviour
    {
        RagdollScript ragdollScript;
        public float health = 50f;
        private float currenthealth;

        void Start()
        {
            ragdollScript=GetComponent<RagdollScript>();
            currenthealth=health;
        }
        public void TakeDamage(float amount)
        {
            currenthealth-=amount;
            Debug.Log("Enemy health is: "+currenthealth);
                if(currenthealth>=0f && currenthealth<=10 )
                {
                    Die();
                }
                if(currenthealth<0f)
                {
                    Die();
                }
            
        }
        void Die()
        {
            StartCoroutine(ragdollScript.ActivateRagdoll());
            currenthealth=health;
        }
    }
}
