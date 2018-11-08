using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLights : MonoBehaviour {

    float Timer;
    public float RandomTime;

    void Update()
    {
        RandomTime = Random.Range(1, 10);
        Timer += Time.deltaTime;
        if (Timer >= RandomTime)
        {
            
            StartCoroutine(Blink());
            Timer = 0;
        }
    }

        IEnumerator Blink()
        {
        Light light = GetComponent<Light>();
        light.enabled = false;
            yield return new WaitForSeconds(0.2f);
        light.enabled = true;
        }

   
    
}
