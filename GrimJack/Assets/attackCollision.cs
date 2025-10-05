using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackCollision : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            
            if(PlayerController.Instance.isAttacking == true)
            {
                print("aaa");
                other.gameObject.GetComponent<EnemyBehavior>().TakeDamage(30f);
                PlayerController.Instance.isAttacking = false;
            }
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
