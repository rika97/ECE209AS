using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Visualization : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public GameObject map;
    public GameObject map1;
    public GameObject map2;
    public GameObject map3;
    public GameObject map4;
    public GameObject map5;
    public GameObject map6;
    public GameObject map7;
    public GameObject map8;
    public GameObject rippleCube;
    public GameObject leftMap;
    public GameObject rightMap;
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

    // Start is called before the first frame update
    void Start()
    {
        left = GameObject.Find("Left");
        right = GameObject.Find("Right");
        map = GameObject.Find("Map");
        map1 = GameObject.Find("Map1");
        map2 = GameObject.Find("Map2");
        map3 = GameObject.Find("Map3");
        map4 = GameObject.Find("Map4");
        map5 = GameObject.Find("Map5");
        map6 = GameObject.Find("Map6");
        map7 = GameObject.Find("Map7");
        map8 = GameObject.Find("Map8");
        rippleCube = GameObject.Find("Cube");
        ringModel = GameObject.Find("Ring");
        rippleOn = true;
        activeRings = 0;

        sound = 0;
        leftTimer = 0;
        rightTimer = 0;
        priority = 1;

        leftImage = left.GetComponent<Image>();
        rightImage = right.GetComponent<Image>();

        left.SetActive(false);
        right.SetActive(false);
        
        map1.SetActive(false);
        map2.SetActive(false);
        map3.SetActive(false);
        map4.SetActive(false);
        map5.SetActive(false);
        map6.SetActive(false);
        map7.SetActive(false);
        map8.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            sound = 1;
            map = map1;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            sound = 2;
            map = map2;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            sound = 3;
            map = map3;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            sound = 4;
            map = map4;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            sound = 5;
            map = map5;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            sound = 6;
            map = map6;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            sound = 7;
            map = map7;
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            sound = 8;
            map = map8;
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
            leftMap = map;
            leftMap.SetActive(true);
        }
        else if (sound == 3 || sound == 5 || sound == 8)
        {
            if (sound == 5 || sound == 8)
            {
                right.SetActive(true);
            }
            rightTimer = 30;
            rightMap = map;
            rightMap.SetActive(true);
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

        Color color = allColor;
        color.a = leftTimer / 30f;
        leftImage.color = color;
        if (leftMap != null)
        {
            leftMap.GetComponent<Image>().color = color;
        }

        color = allColor;
        color.a = rightTimer / 30f;
        rightImage.color = color;
        if (rightMap != null)
        {
            rightMap.GetComponent<Image>().color = color;
        }

        leftTimer -= 1;
        rightTimer -= 1;

        if (leftTimer == 0)
        {
            left.SetActive(false);
            leftMap = null;
        }
        if (rightTimer == 0)
        {
            right.SetActive(false);
            rightMap = null;
        }

        sound = 0;

        if (Input.GetKeyUp(KeyCode.V))
        {
            if (rippleOn)
            {
                StopRipple();
            }
            else
            {
                DoRipple();
            }
            
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
