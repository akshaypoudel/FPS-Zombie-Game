using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace StarterAssets{

public class PlayerHealthScript : MonoBehaviour
{
    #region GAmeSettings
    public float MaxHealth;
    private float CurrentHealth;
    public FirstPersonController controller;
    public GameObject DieScreen;
    public GameObject ReloadingScreen;
    public GameObject AmmoSign;
    public GameObject[] guns;
    public GameObject[] ammoCount;

    public HealthBar bar;
    Animator animator;
    [HideInInspector]
    public bool isPlayerDied=false;

    EnemyController enemyController;
    #endregion
    void Start()
    {
        CurrentHealth = MaxHealth;
        bar.setMaxhealth(MaxHealth);
        Time.timeScale=1f;
        isPlayerDied=false;
        enemyController=FindObjectOfType<EnemyController>();
    }

    public void TakeDamage(float DamageAmount)
    {
        if(CurrentHealth>0)
        {
            CurrentHealth-=DamageAmount;
            Debug.Log("Player Health is: "+CurrentHealth);
            bar.setHealth(CurrentHealth);
            if(CurrentHealth>=0 && CurrentHealth<=5)
            {
                PlayerDie();
            }
            else if(CurrentHealth<0)
            {
                PlayerDie();
            }
        }
    }
    
    public void PlayerDie()
    {
        isPlayerDied=true;
        DieScreen.SetActive(true);
        Time.timeScale=0f;
        Cursor.lockState=CursorLockMode.None;
        enemyController.setTrue();
        enemyController.UiBloodSplash.SetActive(false);
        ReloadingScreen.SetActive(false);
        AmmoSign.SetActive(false);
        controller.enabled=false;

        for(int i=0;i<guns.Length;i++)
        {
            guns[i].SetActive(false);
        }

        for(int i=0;i<ammoCount.Length;i++)
        {
            ammoCount[i].SetActive(false);
        } 

    }
}
}
