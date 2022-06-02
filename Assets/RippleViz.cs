using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleViz : MonoBehaviour
{
    private GameObject ringModel;
    private GameObject sphereModel;
    private float rippleTime;
    private bool rippleOn;
    private int activeRings = 0;
    private int activeSpheres = 0;
    private Queue<GameObject> rings = new Queue<GameObject>();
    private Queue<GameObject> spheres = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        ringModel = GameObject.Find("Ring");
        sphereModel = GameObject.Find("Sphere");
        rippleOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rippleOn)
        {
            if (!gameObject.GetComponent<AudioSource>().isPlaying)
            {
                StopRipple();
            }
            else
            {
                float t = Time.time;
                if (t - rippleTime > 1)
                {
                    GameObject newRing = CreateRing();
                    GameObject newSphere = Instantiate(sphereModel);
                    rings.Enqueue(newRing);
                    spheres.Enqueue(newSphere);
                    rippleTime = t;
                    activeRings++;
                    activeSpheres++;

                    if (activeRings > 5)
                    {
                        GameObject oldestRing = rings.Dequeue();
                        Destroy(oldestRing);
                    }
                    if (activeSpheres > 3)
                    {
                        GameObject oldestSphere = spheres.Dequeue();
                        Destroy(oldestSphere);
                    }
                }
                foreach (GameObject ring in rings)
                {
                    ring.GetComponent<RingViz>().doUpdate();
                }
            }
        } else if (gameObject.GetComponent<AudioSource>().isPlaying)
        {
            BeginRipple();
        }
    }

    void BeginRipple()
    {
        rippleOn = true;
        GameObject newRing = CreateRing();
        GameObject newSphere = Instantiate(sphereModel);
        rings.Enqueue(newRing);
        spheres.Enqueue(newSphere);
        rippleTime = Time.time;
        activeRings = 1;
        activeSpheres = 1;
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

    GameObject CreateRing()
    {
        Vector3 pos = gameObject.transform.position;
        GameObject newRing = Instantiate(ringModel);
        newRing.transform.position = new Vector3(pos.x, 0.11f, pos.z);
        return newRing;
    }
}
