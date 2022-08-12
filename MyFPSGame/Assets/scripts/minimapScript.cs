using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapScript : MonoBehaviour
{
    public Transform Player;
    void LateUpdate()
    {
        Vector3 move=Player.position ;
        move.y=transform.position.y  ;
        transform.position=move      ;

        transform.rotation=Quaternion.Euler(90f,Player.eulerAngles.y,0f);

    }
}
