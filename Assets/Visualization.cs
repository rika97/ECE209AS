using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Visualization : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public GameObject rippleCube;
    public Camera center;
    public GameObject ringModel;
    private bool rippleOn;
    private int activeRings;
    private float rippleTime;
    private Queue<GameObject> rings = new Queue<GameObject>();
    private int sound;
    private int leftTimer;
    private int rightTimer;
    private int priority;
    private Image leftImage;
    private Image rightImage;

    private bool IsVisible(Camera c, GameObject target)
    // has a margin of 1 so that if object is just outside of FOV ripples still happen
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = target.transform.position;
        foreach(var plane in planes)
        {   
            Debug.Log(plane.GetDistanceToPoint(point));
            if(plane.GetDistanceToPoint(point) < -1)
            {
                return false;
            }
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        left = GameObject.Find("Left");
        right = GameObject.Find("Right");
        rippleCube = GameObject.Find("Cube");
        center = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
        ringModel = GameObject.Find("Ring");
        rippleOn = false;
        activeRings = 0;

        sound = 0;
        leftTimer = 0;
        rightTimer = 0;
        priority = 1;

        leftImage = left.GetComponent<Image>();
        rightImage = right.GetComponent<Image>();

        left.SetActive(false);
        right.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("cube pos: "  + rippleCube.transform.position);
        Debug.Log("self pos: "  + center.transform.position);
        Debug.Log("self forward: "  + center.transform.forward);
        Debug.Log("cube - self: "  + (rippleCube.transform.position - center.transform.position));

        if (Input.GetKeyUp(KeyCode.Q))
        {
            sound = 1;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            sound = 2;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            sound = 3;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            sound = 4;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            sound = 5;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            sound = 6;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            sound = 7;
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            sound = 8;
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            priority = 1;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            priority = 2;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            priority = 3;
        }

        if (sound == 1 || sound == 2 || sound == 4 || sound == 6 || sound == 7)
        {
            if (sound == 4 || sound == 6 || sound == 7)
            {
                left.SetActive(true);
            }
            leftTimer = 30;
        }
        else if (sound == 3 || sound == 5 || sound == 8)
        {
            if (sound == 5 || sound == 8)
            {
                right.SetActive(true);
            }
            rightTimer = 30;
        }

        Color allColor = new Color(0, 0, 0);
        if (priority == 1)
        {
            allColor = new Color(1f, 0, 0, 1);
        }
        else if (priority == 2)
        {
            allColor = new Color(1f, .65f, 0, 1);
        }
        else if (priority == 3)
        {
            allColor = new Color(1f, 1f, 0, 1);
        }

        leftTimer -= 1;
        rightTimer -= 1;

        if (leftTimer == 0)
        {
            left.SetActive(false);
        }
        if (rightTimer == 0)
        {
            right.SetActive(false);
        }

        sound = 0;

        if (IsVisible(center,rippleCube))
        {
            Debug.Log("cube in FOV");
            if (!rippleOn)
            {
                rippleOn = true;
                DoRipple();
            }
            
        } else
        {
            Debug.Log("cube not visible");
            StopRipple();
            rippleOn = false;
        }

        if (rippleOn)
        {
            UpdateRipple();
        }
        
    }

    void DoRipple()
    {
        rippleOn = true;
        GameObject newRing = Instantiate(ringModel);
        rings.Enqueue(newRing);
        rippleTime = Time.time;
        activeRings = 1;
    }

    void UpdateRipple()
    {
        float t = Time.time;
        if (t - rippleTime > 1)
        {
            GameObject newRing = Instantiate(ringModel);
            rings.Enqueue(newRing);
            rippleTime = t;
            activeRings++;
            if (activeRings > 5)
            {
                GameObject oldestRing = rings.Dequeue();
                Destroy(oldestRing);
            }
        }

        foreach (GameObject ring in rings)
        {
            ring.transform.localScale += (new Vector3(.02f, 0f, .02f));
        }
    }

    void StopRipple()
    {
        rippleOn = false;
        foreach (GameObject ring in rings)
        {
            Destroy(ring);
        }
        rings.Clear();
    }
}
