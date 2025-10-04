using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room1 : MonoBehaviour
{
    public DialogueTrigger trigger;

    private void OnTriggerEnter(Collider other)
    {
        trigger.TriggerDialogue();
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
