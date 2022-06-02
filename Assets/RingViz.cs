using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingViz : MonoBehaviour
{
    private float timeCreated;
    private float lifespan;
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
        gameObject.transform.localScale += (new Vector3(.02f, 0f, .02f));
        float a = 1.0f - (t - timeCreated) / lifespan;
        Color color = new Color(1f, 0, 0, a);
        material.color = color;
    }
}
