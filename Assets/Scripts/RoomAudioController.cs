using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class RoomAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource myAudioSource;
    [SerializeField] private int nSoundsSelected;

    // Start is called before the first frame update
    void Start()
    {
        // query desired objects using tags ---------------------------------------------------------------------- TODO: convert to list to pop when selected
        var myObjectArray = GameObject.FindGameObjectsWithTag("wSound");
        int nSoundObjects = myObjectArray.Length;

        Debug.Log("type! "+myObjectArray.GetType());
        Debug.Log("found "+nSoundObjects+" wSound objects");

        Assert.IsTrue(nSoundsSelected < nSoundObjects);

        int[] selection =  new int[nSoundsSelected];
        
        for (int i=0; i< nSoundsSelected; i++)
        {
            selection[i] = Random.Range(0,nSoundObjects-1);
            string selectedName =  myObjectArray[selection[i]].name;
            Debug.Log("selected : "+selectedName);

            // Simply play once
            // myAudioSource =  myObjectArray[selection[i]].GetComponent<AudioSource>();
            // myAudioSource.Play();

            // Call method inside and loop
            var myAudioScript = myObjectArray[selection[i]].GetComponent<TimedTriggerAudio>();
            myAudioScript.changeLoopPeriod(5*(i+1));
            myAudioScript.toggleLoop(true);
            
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
