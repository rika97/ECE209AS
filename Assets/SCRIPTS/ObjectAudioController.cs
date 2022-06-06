using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ObjectAudioController : MonoBehaviour
{
    private float playEverySeconds = 5;
    private float timePassed = 0;
    private bool toggle = false;
    private AudioSource myAudioSource;

    public Stopwatch timer;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
        timer = new Stopwatch();
    }

    // Update is called once per frame
    void Update()
    {
        //timePassed += Time.deltaTime;
        //if (timePassed >= playEverySeconds && toggle)
        //{
        //    timePassed = 0;
        //    myAudioSource.Play();
        //}
    }

    public void StartWDelay(float delayTime)
    {
        StartCoroutine(DelayAction(delayTime));
    }

    IEnumerator DelayAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        myAudioSource.Play();
        timer.Start();
    }


    //float PostTimer()
    //{
    //    return timer.Elapsed;
    //}
}
