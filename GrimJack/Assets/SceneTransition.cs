using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public AudioSource playbutton; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Play()
    {
        playbutton.Play();

        yield return new WaitForSeconds(3);
        

        SceneManager.LoadScene(1);


    }

    public void waiter()
    {
        StartCoroutine(Play()); 

    }


}
