using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class die : MonoBehaviour
{
    public bool final;
    public GameObject nextdialogue;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dienow()
    {
        if (final)
        {
            Destroy(parent);
            SceneManager.LoadScene(3);
        }
        else
        {
            nextdialogue.SetActive(true);
            Destroy(parent);
        }
        
    }
}
