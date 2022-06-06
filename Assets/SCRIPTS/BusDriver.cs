using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDriver : MonoBehaviour
{

    private bool toggle = false;
    private AudioSource myAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myAudioSource.isPlaying && !toggle)
        {
            // enable animation
            toggle = true;
            this.GetComponent<Animator>().enabled = true;
            this.GetComponent<Animator>().Play(0);
        }
        //if(this.GetComponent<Animator>().end)
    }

    public void OnAnimationEnd()
    {
        // reset
        this.GetComponent<Animator>().enabled = false;
        toggle = false;
    }
}
