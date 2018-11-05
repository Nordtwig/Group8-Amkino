using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLights : MonoBehaviour {

    public float Timer = 0;

    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= 2)
        {
            
            StartCoroutine(Blink());
            Timer = 0;
        }
    }

        IEnumerator Blink()
        {
        Light light = GetComponent<Light>();
        light.enabled = true;
            yield return new WaitForSeconds(0);
        light.enabled = false;
        }
    
}
