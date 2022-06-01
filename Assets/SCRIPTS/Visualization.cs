using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Visualization : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public GameObject above;
    public GameObject below;
    public GameObject rippleCube;
    public GameObject square;
    public Camera center;
    public GameObject ringModel;
    private bool rippleOn;
    private int activeRings;
    private float rippleTime;
    private float angleFromForward;
    private float angleFromUp;
    private float halfFOV;
    private Queue<GameObject> rings = new Queue<GameObject>();
    // private int sound;
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
            //Debug.Log(plane.GetDistanceToPoint(point));
            if(plane.GetDistanceToPoint(point) < -1)
            {
                return false;
            }
        }
        return true;
    }

    private float CalculateAngleFromForward(Camera c, GameObject target)
    // calculate angle from forward and project down to xz plane
    {
        Vector3 from = Vector3.ProjectOnPlane(target.transform.position - c.transform.position, c.transform.up);
        Vector3 to = c.transform.forward;
        float projectedAngleFromForward = Vector3.SignedAngle(from,to,c.transform.up);
        return projectedAngleFromForward;
    }
    
    private float CalculateAngleFromUp(Camera c, GameObject target)
    // calculate angle from up and project forward to xy plane
    {
        Vector3 from = Vector3.ProjectOnPlane(target.transform.position - c.transform.position, c.transform.right);
        Vector3 to = c.transform.up;
        float projectedAngleFromUp = Vector3.SignedAngle(from,to,c.transform.right);
        return projectedAngleFromUp;
    }

    // Start is called before the first frame update
    void Start()
    {
        square = GameObject.Find("Square");
        left = GameObject.Find("Left");
        right = GameObject.Find("Right");
        above = GameObject.Find("Above");
        below = GameObject.Find("Below");
        rippleCube = GameObject.Find("Cube");
        center = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
        ringModel = GameObject.Find("Ring");
        rippleOn = false;
        activeRings = 0;
        halfFOV = 45;
        // sound = 0;
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
        //Debug.Log("cube pos: "  + rippleCube.transform.position);
        //Debug.Log("self pos: "  + center.transform.position);
        //Debug.Log("self forward: "  + center.transform.forward);
        angleFromForward = CalculateAngleFromForward(center, rippleCube);
        angleFromUp = CalculateAngleFromUp(center, rippleCube);
        // Debug.Log("angle: "  + CalculateAngleFromUp(center, rippleCube));

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

        // object on left
        if (angleFromForward > halfFOV && angleFromForward < 180)
        {
            left.SetActive(true);
            // leftTimer = 30;

        } else {
            left.SetActive(false);
        }
        
        // object on right
        if (angleFromForward < -halfFOV && angleFromForward > -180)
        {
            right.SetActive(true);
            // rightTimer = 30;
        } else {
            right.SetActive(false);
        }

        // object above
        if (angleFromUp > -90 + halfFOV && angleFromUp < 90)
        {
            above.SetActive(true);
            // leftTimer = 30;

        } else {
            above.SetActive(false);
        }
        
        // object below
        if (angleFromUp < -90 - halfFOV || angleFromUp > 90 && angleFromUp < 180)
        {
            below.SetActive(true);
            // rightTimer = 30;
        } else {
            below.SetActive(false);
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

        // leftTimer -= 1;
        // rightTimer -= 1;

        // if (leftTimer == 0)
        // {
        //     left.SetActive(false);
        // }
        // if (rightTimer == 0)
        // {
        //     right.SetActive(false);
        // }

        // if (IsVisible(center,rippleCube))
        if (angleFromForward > -60 && angleFromForward < 60 && angleFromUp > -120 && angleFromUp < -60)
        {
            //Debug.Log("cube in FOV");
            if (!rippleOn)
            {
                rippleOn = true;
                BeginRipple();
            }
            
        } else
        {
            //Debug.Log("cube not visible");
            StopRipple();
            rippleOn = false;
        


        }

        if (rippleOn)
        {
            UpdateRipple();
        }
        
    }

    void BeginRipple()
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
