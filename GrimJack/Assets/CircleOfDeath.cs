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
            print(PlayerController.Instance.health);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
