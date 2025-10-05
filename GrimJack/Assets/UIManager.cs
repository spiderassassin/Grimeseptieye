using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UIManager : MonoBehaviour
{
    public PostProcessVolume volume;
    private Vignette vignette;
    public float overlay_speed;
    public float max_value = 0.5f;
    public float min_value = 0.1f;
    public bool increasing = true;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.Instance.inCircleofDeath == true)
        {
            if (increasing)
            {
                if (vignette.intensity.value <= max_value)
                {

                    vignette.intensity.value += overlay_speed * Time.deltaTime;
                }
                else
                {
                    increasing = false;
                }
            }

            else
            {
                if (vignette.intensity.value >= min_value)
                {
                    vignette.intensity.value -= overlay_speed * Time.deltaTime;
                }
                else
                {
                    increasing = true;
                }
            }

        }
        else
        {
            print("OUT");
            if (vignette.intensity.value >= 0f)
            {
                vignette.intensity.value -= overlay_speed * Time.deltaTime;
            }
                
            
        }
    }
    
}
