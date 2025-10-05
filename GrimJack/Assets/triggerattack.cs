using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerattack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TriggerAttack()
    {
        PlayerController.Instance.Attack();
    }
}
