using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTriggerAudio : MonoBehaviour
{
    private float playEverySeconds = 25;
    private float timePassed = 0;
	
	
    [SerializeField] private AudioSource myAudioSource;
 
	
    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		timePassed += Time.deltaTime;
        if (timePassed >= playEverySeconds)
        {
            timePassed = 0;
            myAudioSource.Play();
        }
    }
}
