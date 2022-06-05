using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RippleViz : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        ringModel = GameObject.Find("Ring");
        sphereModel = GameObject.Find("Sphere");
        rippleOn = false;
        //GameObject newSphere = CreateSphere();
        //spheres.Enqueue(newSphere);
        Bounds bounds = gameObject.GetComponent<Renderer>().bounds;
        scale = (new float[] { bounds.size.x, bounds.size.y, bounds.size.z }).Max()*1.5f;
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
                if (t - sphereTime > 4)
                {

                    GameObject newSphere = CreateSphere();
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
                foreach (GameObject ring in rings)
                {
                    ring.GetComponent<RingViz>().doUpdate();
                }
                foreach (GameObject sphere in spheres)
                {
                    sphere.GetComponent<SphereViz>().doUpdate();
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
        GameObject newSphere = CreateSphere();
        rings.Enqueue(newRing);
        spheres.Enqueue(newSphere);
        rippleTime = Time.time;
        sphereTime = Time.time;
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

    GameObject CreateSphere()
    {
        Vector3 pos = gameObject.transform.position;
        GameObject newSphere = Instantiate(sphereModel);
        newSphere.transform.position = new Vector3(pos.x, pos.y, pos.z);
        newSphere.transform.localScale = new Vector3(scale, scale, scale);
        return newSphere;
    }
}
