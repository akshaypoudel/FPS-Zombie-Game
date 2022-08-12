using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// this Code is written using Unity's New Input System

namespace StarterAssets
{
    public class GunScript : MonoBehaviour
    {
        #region GunSettings
        public float damage = 10f; // shoot damage
        public float range = 1000f; //firing range
        public float Firerate=20f; //firing speed
        public float impactForce=30f; //shooting force
        public float nextTimeToFire=0f; //how much time to wait before firing again
        #endregion

        #region scriptsReference
        FirstPersonController controller; // FPS Character
        RagdollScript ragdollScript;
        GunSelectionsScript gunSelectionsScript;
        PlayerHealthScript Player;
        #endregion

        #region GameObjectsReferences
        public Camera fpsCam; // main camera
        public ParticleSystem muzzleflash; //muzzle flash particle system
        public GameObject impact; // impact(hole after shooting)
        public AudioClip clip; // gun sound
        Animator animator;
        public GameObject BloodEffect;
        #endregion

        #region Reloading
        // Reloading--------------------------------------------------
        public int maxAmmo=10;
        public int AmmoInHand=500; 
        [HideInInspector]
        public int currentAmmo=0;
        public float reloadTime=3f;
        [HideInInspector]
        bool isReloading=false;
        public GameObject ReloadingSign;
        public GameObject NoAmmoSign;
        bool isAmmoEmpty=false;
        [HideInInspector]
        public int AmmoCount=0;
        bool AmmoGoingToBeEmpty=false;
        bool isPressingR=false;
        private int RequiredAmmo=0;
        #endregion

        #region Recoil
        //Recoil-------------------------------------------------------------------------
        float recoilIntensityCounter = 0;
        float recoilMaxIntensity = 50;

        [SerializeField]
        float recoilXOffset;
        [SerializeField]
        float recoilYOffset;
        [SerializeField]
        bool InvertY = false;     
        #endregion   

        //-------------------------------------------------------------------------------

        void Start()
        {
            controller=FindObjectOfType<FirstPersonController>();
            animator=GetComponent<Animator>();
            gunSelectionsScript=FindObjectOfType<GunSelectionsScript>();
            ragdollScript=FindObjectOfType<RagdollScript>();
            currentAmmo=maxAmmo;                  

        }
        private void Update()
        {  
            if(isReloading)
                return;
            if(currentAmmo<=0 && AmmoInHand>0 && AmmoGoingToBeEmpty==false)
            {
                StartCoroutine(Reload());
                return;
            }
            if(Mouse.current.leftButton.isPressed && controller._controller.isGrounded
                && !Keyboard.current.shiftKey.isPressed &&isAmmoEmpty==false )
            {
                animator.SetTrigger("shoot");
                Shoot();
            }
            else
            {
                if (recoilIntensityCounter < 0)
                    recoilIntensityCounter = 0;
                else
                    recoilIntensityCounter--;
            }
            //*************************************
            //aiming logic
            if(Mouse.current.rightButton.isPressed )
            {
                animator.SetBool("isAiming",true);
            }
            else
            {
                animator.SetBool("isAiming",false);
            }
            //***************************************
            // if R key was pressed (Reloading)
            if(Keyboard.current.rKey.wasPressedThisFrame && isAmmoEmpty==false && currentAmmo!=0 && AmmoInHand>0 && currentAmmo!=maxAmmo)
            {
                isPressingR=true;
                StartCoroutine(Reload());
            }
            //*****************************************
            //if Total ammo is less than 0 , than show message(Low Ammo...)
            if(AmmoInHand<=0 )
                NoAmmoSign.SetActive(true);
            else
                NoAmmoSign.SetActive(false);
            //*************************************
        }
        public void Shoot()
        {
            if(Time.time>=nextTimeToFire) // if time is greater than next time to fire than start Firing
            {
                nextTimeToFire=Time.time+1f/Firerate; // setting the value of next time to fire
                muzzleflash.Play();
                
                //Ammo Logic(when total ammo is empty then if current ammo is less than 0 than stop firing)
                //****************************************************
                if(currentAmmo>0) 
                    currentAmmo--;
                else if(AmmoGoingToBeEmpty)
                {
                    isAmmoEmpty=true;
                }
                //*******************************************
                AudioSource.PlayClipAtPoint(clip,transform.position);
                RaycastHit hit;


                Ray ray = fpsCam.ScreenPointToRay(Recoil());
                
                
                //making Bullet Effect with RayCast Hit
                if(Physics.Raycast(ray,out hit,range))
                {
                //Force of the bullet is applying to any objects(rigidbodies only) it hits
                    if(hit.rigidbody!=null)
                    {
                        hit.rigidbody.AddForce(-hit.normal*impactForce);
                    }

                //if the Gameobject is enemy than turn off the ImpactEffect on Enemy Otherwise Turn it ON
                    if(hit.collider.tag=="Enemy")                       
                    {
                        // Instantiate(BloodEffect,hit.point,Quaternion.identity/*LookRotation(hit.normal,hit.point)*/);
                        GameObject impactGo=Instantiate(BloodEffect,hit.point,Quaternion.LookRotation(hit.normal,hit.point));
                        Destroy(impactGo,1f);   
                          
                        TargetScript1 targetScript1=hit.transform.GetComponent<TargetScript1>();
                        targetScript1.TakeDamage(damage);

                    }

                    else if(hit.transform.name=="PlayerCapsule 1")
                    {

                    }
                    else
                    {
                        GameObject impactGo=Instantiate(impact,hit.point,Quaternion.LookRotation(hit.normal,hit.point));
                        Destroy(impactGo,7f);
                    }
                }
            }
        }
        Vector2 Recoil()
        {
            recoilIntensityCounter++;
            if (recoilIntensityCounter > recoilMaxIntensity)
                recoilIntensityCounter = recoilMaxIntensity;

            float xoffset = UnityEngine.Random.Range(0, recoilIntensityCounter);
            float yoffset = UnityEngine.Random.Range(0, recoilIntensityCounter);
            if (UnityEngine.Random.Range(0f, 1f) <= 0.5f)
                xoffset = -xoffset;
            if (InvertY)
            {
                if (UnityEngine.Random.Range(0f, 1f) <= 0.5f)
                    yoffset = -yoffset;
            }
    

            float x = (Screen.width / 2) + xoffset*4;
            float y = (Screen.height / 2) + yoffset*4;

            return new Vector2(x, y);
        }
        IEnumerator Reload()
        {
            isReloading=true;
            gunSelectionsScript.isGunConvertable=false; //Gun can not be changed when Reloading
            //******
            animator.SetBool("isReloading",true);
            controller.SprintSpeed=6f;
            ReloadingSign.SetActive(true);
            yield return new WaitForSeconds(reloadTime - 0.25f);
            animator.SetBool("isReloading",false);
            yield return new WaitForSeconds(0.25f);
            ReloadingSign.SetActive(false);
            controller.SprintSpeed=10f;
            //*******
            //every time the gun reloads check if the total ammo is less than max ammo otherwise subtract current ammo from total ammo
                if(AmmoInHand<maxAmmo && AmmoInHand!=0 && isPressingR==false)
                {
                    currentAmmo=AmmoInHand;
                    AmmoInHand=0;
                    AmmoGoingToBeEmpty=true;
                }
                else if(isPressingR)
                {
                    if(AmmoInHand<maxAmmo && AmmoInHand!=0) //if ammo in hand is less than max ammo than execute this statement
                    {
                        RequiredAmmo=maxAmmo-currentAmmo;//how much ammo is required to full the magazine
                        if(AmmoInHand>RequiredAmmo) //if total ammo is greater than required ammo
                        {
                            currentAmmo+=RequiredAmmo;
                            AmmoInHand-=RequiredAmmo;
                        }
                        else
                        {
                            currentAmmo+=AmmoInHand;
                            AmmoInHand=0;
                        }
                        AmmoGoingToBeEmpty=true;
                        isPressingR=false;
                    }
                    else
                    {
                        AmmoInHand -= maxAmmo-currentAmmo;
                        currentAmmo = maxAmmo;
                        isPressingR = false;
                    }
                }
                else{
                    currentAmmo=maxAmmo;
                    AmmoInHand-=currentAmmo;
                }
            //*****************************************
            isReloading=false;// setting Reloading = false
            gunSelectionsScript.isGunConvertable=true;
        }
    
        
    }
}
