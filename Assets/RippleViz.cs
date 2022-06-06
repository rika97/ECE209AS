using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class RippleViz : MonoBehaviour
{
    private bool showRings;
    private bool showSpheres;
    private GameObject ringModel;
    private GameObject sphereModel;
    private float rippleTime;
    private float sphereTime;
    public bool rippleOn;
    private int activeRings = 0;
    private int activeSpheres = 0;
    private Queue<GameObject> rings = new Queue<GameObject>();
    private Queue<GameObject> spheres = new Queue<GameObject>();
    private float ringScale;
    private float sphereScale;
    private AudioSource audioSource;
    private SoundVolumeGrabber volumeGrabber;


    // Start is called before the first frame update
    void Start()
    {
        ringModel = GameObject.Find("Ring");
        sphereModel = GameObject.Find("Sphere");
        rippleOn = false;

        Bounds bounds;
        if (gameObject.TryGetComponent(out Renderer renderer))
        {
            bounds = renderer.bounds;
        } else
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 1)
            {
                bounds = renderers[0].bounds;
                print(bounds.size);

            } else
            {
                bounds = CalcMultiBounds(renderers);
            }
        }
        ringScale = (new float[] { bounds.size.x, bounds.size.z }).Min() * .5f;
        sphereScale = (new float[] { bounds.size.x, bounds.size.y, bounds.size.z }).Max() * 1.5f;
        
        audioSource = gameObject.GetComponent<AudioSource>();
        volumeGrabber = gameObject.GetComponent<SoundVolumeGrabber>();
        
        if (bounds.center.y - bounds.extents.y < .1f)
        {
            showRings = true;
            showSpheres = false;
        } else
        {
            showRings = false;
            showSpheres = true;
        }
        Debug.Log("showSpheres: " + showSpheres);

    }

    // Update is called once per frame
    void Update()
    {
        if (rippleOn)
        {
            if (!audioSource.isPlaying)
            {
                StopRipple();
            }
            else
            {
                float t = Time.time;
                if (showRings)
                {
                    if (t - rippleTime > 1)
                    {
                        GameObject newRing = CreateRing(5);
                        rings.Enqueue(newRing);
                        rippleTime = t;
                        activeRings++;
                        if (activeRings > 5)
                        {
                            GameObject oldestRing = rings.Dequeue();
                            Destroy(oldestRing);
                            activeRings -= 1;
                        }
                    }
                    foreach (GameObject ring in rings)
                    {
                        ring.GetComponent<RingViz>().doUpdate();
                    }
                }

                if (showSpheres)
                {
                    if (t - sphereTime > 4)
                    {
                        GameObject newSphere = CreateSphere(8);
                        spheres.Enqueue(newSphere);
                        sphereTime = t;
                        activeSpheres++;
                        if (activeSpheres > 2)
                        {
                            GameObject oldestSphere = spheres.Dequeue();
                            Destroy(oldestSphere);
                            activeSpheres -= 1;
                        }
                    }

                    foreach (GameObject sphere in spheres)
                    {
                        sphere.GetComponent<SphereViz>().doUpdate();
                    }
                }
            }
        } else if (audioSource.isPlaying)
        {
            BeginRipple();
        }
    }

    public void BeginRipple()
    {
        rippleOn = true;
        rippleTime = Time.time - 1f;
        sphereTime = Time.time - 4f;
    }

    public void StopRipple()
    {
        rippleOn = false;
        foreach (GameObject ring in rings)
        {
            Destroy(ring);
        }
        foreach (GameObject sphere in spheres)
        {
            Destroy(sphere);
        }
        rings.Clear();
        spheres.Clear();
        activeRings = 0;
        activeSpheres = 0;
    }

    GameObject CreateRing(int lifespan)
    {
        Vector3 pos = gameObject.transform.position;
        float volume = volumeGrabber.postLoudness();
        GameObject newRing = Instantiate(ringModel);
        newRing.transform.position = new Vector3(pos.x, 0.11f, pos.z);
        newRing.transform.localScale = new Vector3(ringScale, ringScale, ringScale);
        newRing.GetComponent<RingViz>().lifespan = lifespan;
        newRing.GetComponent<RingViz>().scale = Mathf.Max(.02f / (.3f / volume), .005f);
        return newRing;
    }

    GameObject CreateSphere(int lifespan)
    {
        Vector3 pos = gameObject.transform.position;
        float volume = volumeGrabber.postLoudness();
        GameObject newSphere = Instantiate(sphereModel);
        newSphere.transform.position = new Vector3(pos.x, pos.y, pos.z);
        newSphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
        Debug.Log("1");
        newSphere.GetComponent<SphereViz>().lifespan = lifespan;
        Debug.Log("2");
        newSphere.GetComponent<SphereViz>().scale = Mathf.Max(.02f / (.3f / volume), .005f);
        return newSphere;
    }

    Bounds CalcMultiBounds(Renderer[] renderers)
    {
        Quaternion currentRotation = gameObject.transform.rotation;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Bounds bounds = new Bounds(gameObject.transform.position, Vector3.zero);
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        gameObject.transform.rotation = currentRotation;
        return bounds;
    }
}
