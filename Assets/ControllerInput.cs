using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip clip;
    public AudioSource audioSource;

    public Transform gunBarrelTransform;
    public RoomAudioController roomAudioController;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        roomAudioController = GameObject.FindObjectOfType<RoomAudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            RaycastGun();
        }
    }

    private void RaycastGun()
    {
        RaycastHit hit;
        Dictionary<GameObject, bool> adict = roomAudioController.PostSoundInformation();
        if (Physics.Raycast(gunBarrelTransform.position, gunBarrelTransform.forward, out hit))
        {
            GameObject targetObject = hit.collider.gameObject;
            Debug.Log("hit: " + targetObject);
            if (targetObject.CompareTag("wSound"))
            {
                audioSource.Play();
                if (targetObject.GetComponent<AudioSource>().isPlaying)
                {
                    // System.TimeSpan x = targetObject.GetComponent<ObjectAudioController>().timer.Elapsed;
                    // Debug.LogWarning("HitObject - timerElapsed: " + x.ToString());
                    targetObject.GetComponent<AudioSource>().Stop();
                    //adict[targetObject] = true;
                }
            }
        }
    }
}
