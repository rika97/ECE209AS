using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTriggerAudio : MonoBehaviour
{
    private float playEverySeconds = 5;
    private float timePassed = 0;
	
    private bool toggle = false;
	
    // [SerializeField] private AudioSource myAudioSource;
    private AudioSource myAudioSource;
 
	
    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		timePassed += Time.deltaTime;
        if (timePassed >= playEverySeconds && toggle )
        {
            timePassed = 0;
            myAudioSource.Play();
        }
    }

    public void toggleLoop(bool on) {
        if(on){
            myAudioSource.Play();
            toggle = true;
        }else{
            toggle = false;
        }
    }

    public void changeLoopPeriod(int period) {
        playEverySeconds = period;
    }
}
