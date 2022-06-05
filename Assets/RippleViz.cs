using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class RippleViz : MonoBehaviour
{
    public bool showRings = true;
    public bool showSpheres = true;
    private GameObject ringModel;
    private GameObject sphereModel;
    private float rippleTime;
    private float sphereTime;
    private bool rippleOn;
    private int activeRings = 0;
    private int activeSpheres = 0;
    private Queue<GameObject> rings = new Queue<GameObject>();
    private Queue<GameObject> spheres = new Queue<GameObject>();
    private float scale;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        ringModel = GameObject.Find("Ring");
        sphereModel = GameObject.Find("Sphere");
        rippleOn = false;
        Bounds bounds = gameObject.GetComponent<Renderer>().bounds;
        scale = (new float[] { bounds.size.x, bounds.size.y, bounds.size.z }).Max()*1.5f;
        audioSource = gameObject.GetComponent<AudioSource>();

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
                        //int nRings = Convert.ToInt32(Math.Floor(Mathf.Min(volume, .99f) / 0.2)) + 1;
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
                        print("A");
                        //int nSpheres = Convert.ToInt32(Math.Floor(Mathf.Min(volume, .99f) / 0.5)) + 1;
                        GameObject newSphere = CreateSphere(8);
                        spheres.Enqueue(newSphere);
                        sphereTime = t;
                        activeSpheres++;
                        print("B");
                        if (activeSpheres > 2)
                        {
                            print("C");
                            GameObject oldestSphere = spheres.Dequeue();
                            Destroy(oldestSphere);
                            activeSpheres -= 1;
                            print("D");
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

    void BeginRipple()
    {
        rippleOn = true;
        //GameObject newRing = CreateRing();
        //GameObject newSphere = CreateSphere();
        //rings.Enqueue(newRing);
        //spheres.Enqueue(newSphere);
        rippleTime = Time.time - 1f;
        sphereTime = Time.time - 4f;
        //activeRings = 1;
        //activeSpheres = 1;
    }

    void StopRipple()
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
    }

    GameObject CreateRing(int lifespan)
    {
        Vector3 pos = gameObject.transform.position;
        float volume = audioSource.volume;
        GameObject newRing = Instantiate(ringModel);
        newRing.transform.position = new Vector3(pos.x, 0.11f, pos.z);
        newRing.GetComponent<RingViz>().lifespan = lifespan;
        newRing.GetComponent<RingViz>().scale = .02f / (1 / volume);
        return newRing;
    }

    GameObject CreateSphere(int lifespan)
    {
        Vector3 pos = gameObject.transform.position;
        float volume = audioSource.volume;
        GameObject newSphere = Instantiate(sphereModel);
        newSphere.transform.position = new Vector3(pos.x, pos.y, pos.z);
        newSphere.transform.localScale = new Vector3(scale, scale, scale);
        newSphere.GetComponent<SphereViz>().lifespan = lifespan;
        newSphere.GetComponent<SphereViz>().scale = .02f / (1 / volume);
        return newSphere;
    }
}
