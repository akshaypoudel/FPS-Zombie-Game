using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarterAssets{
public class CanvasAmmoCount : MonoBehaviour
{
    Text text;
    GunScript gun; 

    void Start()
    {
        text    =    GetComponent<Text>();
        gun     =    FindObjectOfType<GunScript>();
    }
    void Update()
    {
        text.text=gun.currentAmmo+"/"+gun.AmmoInHand;
    }
}
}
