using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CircleOfDeath : MonoBehaviour
{
    

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController.Instance.Damage();
            PlayerController.Instance.inCircleofDeath = true;
            print(PlayerController.Instance.health);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerController.Instance.inCircleofDeath = false;
    }
    
}
