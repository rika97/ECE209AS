using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip clip;
    public AudioSource audioSource;

    public Transform gunBarrelTransform;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
          audioSource.Play();
          RaycastGun();
        }
    }

    private void RaycastGun()
    {
      RaycastHit hit;
      if (Physics.Raycast(gunBarrelTransform.position, gunBarrelTransform.forward, out hit))
      {
        if (hit.collider.gameObject.CompareTag("HitTarget"))
        {
          Destroy(hit.collider.gameObject);
        }
      }
    }
}
