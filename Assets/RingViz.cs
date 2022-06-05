using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingViz : MonoBehaviour
{
    public float lifespan;
    public float scale;
    private float timeCreated;
    private Material material;
    
    // Start is called before the first frame update
    void Start()
    {
        timeCreated = Time.time;
        lifespan = 5;
        material = gameObject.GetComponentInChildren<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void doUpdate()
    {
        float t = Time.time;
        gameObject.transform.localScale += (new Vector3(scale, 0f, scale));
        float a = 1.0f - (t - timeCreated) / lifespan;
        Color color = new Color(1f, 0, 0, a/1.5f);
        material.color = color;
    }
}
