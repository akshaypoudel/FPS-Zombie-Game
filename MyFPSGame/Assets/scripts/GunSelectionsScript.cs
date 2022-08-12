using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class GunSelectionsScript : MonoBehaviour
    {
        public GameObject[] guns;
        public GameObject[] ammoCount;
        [HideInInspector]
        public int selectedguns = 0;
        [HideInInspector]
        public int Ammocount=0;
        [HideInInspector]
        public bool isGunConvertable=true;
        PlayerHealthScript Player;
        void Update()
        {
            if(Keyboard.current.qKey.wasPressedThisFrame && isGunConvertable )
            {
                PreviousCharacter();
            }
            
            if(Keyboard.current.eKey.wasPressedThisFrame && isGunConvertable )
            {
                NextCharacter();
            }
        }
        public void NextCharacter()
        {
            guns[selectedguns].SetActive(false);
            selectedguns = (selectedguns + 1) % guns.Length;
            guns[selectedguns].SetActive(true);
            //-----------------------------------------------------------------
            //Ammo
            ammoCount[Ammocount].SetActive(false);
            Ammocount = (Ammocount + 1) % ammoCount.Length;
            ammoCount[Ammocount].SetActive(true);
        }
        public void PreviousCharacter()
        {
            guns[selectedguns].SetActive(false);
            selectedguns--;
            if(selectedguns<0)
            {
                selectedguns += guns.Length;
            }
            guns[selectedguns].SetActive(true);
            //---------------------------------------------------------
            //Ammo
            ammoCount[Ammocount].SetActive(false);
            Ammocount--;
            if(Ammocount<0)
            {
                Ammocount += ammoCount.Length;
            }
            ammoCount[Ammocount].SetActive(true);
            
        }
    }
}
