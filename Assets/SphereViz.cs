using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereViz : MonoBehaviour
{
    private float timeStart;
    private float lifespan;
    private Material material;
    private bool fadeIn;

    // Start is called before the first frame update
    void Start()
    {
        timeStart = Time.time;
        lifespan = 4;
        material = gameObject.GetComponent<Renderer>().material;
        fadeIn = true;

        Color color = new Color(1f, 0, 0, 0);
        material.color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void doUpdate()
    {
        float t = Time.time;
        gameObject.transform.localScale += (new Vector3(.02f, 0.02f, .02f));
        float a;
        if (fadeIn)
        {
            a = (t - timeStart) / lifespan;
            if (a > 1f)
            {
                fadeIn = false;
                timeStart = t;
            }
        }
        else
        {
            a = 1.0f - (t - timeStart) / lifespan;
            //if (a < 0)
            //{
            //    fadeIn = true;
            //    timeStart = t;
            //}
        }
        //float a = a = 1.0f - (t - timeStart) / lifespan;
        Color color = new Color(1f, 0, 0, a/5f);
        material.color = color;
    }
}
