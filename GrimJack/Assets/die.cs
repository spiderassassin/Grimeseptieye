using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class die : MonoBehaviour
{
    public bool final;
    public bool guitar = false;
    public GameObject bgmusic;
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
        if (guitar)
        {
            bgmusic.SetActive(true);
        }
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
