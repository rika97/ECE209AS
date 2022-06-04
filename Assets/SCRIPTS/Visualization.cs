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

    public GameObject cube;
    public GameObject cube2;
    public RoomAudioController props;
    private Dictionary<GameObject, bool> adict;
    public Dictionary<GameObject, GameObject> propDict = new Dictionary<GameObject, GameObject>();
    public List<GameObject> propsList;
    public GameObject indicatorModel;
    public Camera center;
    public GameObject canvas;
    private float angleLeftRight;
    private float angleUpDown;
    // private Queue<GameObject> rings = new Queue<GameObject>();
    // private int sound;
    // private int leftTimer;
    // private int rightTimer;
    private int priority;

    Vector3 bottomLeft = new Vector3(-.8f,-.7f,0f);
    Vector3 bottomRight = new Vector3(.8f,-.7f,0f);
    Vector3 topLeft = new Vector3(-.8f,.7f,0f);
    Vector3 topRight = new Vector3(.8f,.7f,0f);
    
    private bool findPointerCanvasPosition(out Vector3 intersection, GameObject c, GameObject target){

        // transform 3D positions onto normal plane
        Vector3 a1 = Vector3.zero;
        Vector3 a2 = target.transform.position;

        // target
        Debug.DrawLine(c.transform.position, a2);

        //normal
        Debug.DrawLine(c.transform.position, c.transform.position + c.transform.forward);
        Vector3 forward = new Vector3(0f,0f,1f);
        a2 = Vector3.ProjectOnPlane(c.transform.InverseTransformPoint(target.transform.position), forward);
        // // projected
        Debug.DrawLine(c.transform.position, c.transform.TransformPoint(a2));

        float theta = Vector3.SignedAngle(new Vector3(1f,0f,0f),a2,forward);
        theta = (theta * Mathf.PI)/180f;
        float a = 0.8f;
        float b  = 0.7f;
        float x = a*Mathf.Cos(theta);
        float y = b*Mathf.Sin(theta);
        intersection = new Vector3(x,y,0f);
        return true;
    }


    private float findIntersectionAngle(Vector3 intersection){
        Vector3 up = new Vector3(0f,1f,0f);
        float intersectionAngle = Vector3.SignedAngle(up,intersection,new Vector3(0f,0f,1f));
        return intersectionAngle;
    }

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

    private float CalculateLeftRightAngle(Camera c, GameObject target)
    // calculate angle from forward and project down to xz plane
    {
        Vector3 from = Vector3.ProjectOnPlane(target.transform.position - c.transform.position, c.transform.up);
        Vector3 to = c.transform.forward;
        float projectedAngleFromForward = Vector3.SignedAngle(from,to,c.transform.up);
        return projectedAngleFromForward;
    }
    
    private float CalculateUpDownAngle(Camera c, GameObject target)
    // calculate angle from forward and project forward to xy plane
    {
        Vector3 from = Vector3.ProjectOnPlane(target.transform.position - c.transform.position, c.transform.right);
        Vector3 to = c.transform.forward;
        float projectedAngleFromUp = Vector3.SignedAngle(from,to,c.transform.right);
        return projectedAngleFromUp;
    }

    // Start is called before the first frame update
    void Start()
    {   
        propsList = new List<GameObject>();
        props = GameObject.FindObjectOfType<RoomAudioController>();
        indicatorModel = GameObject.Find("IndicatorModel");
        left = GameObject.Find("Left");
        right = GameObject.Find("Right");
        above = GameObject.Find("Above");
        below = GameObject.Find("Below");
        cube = GameObject.Find("Cube");
        cube2 = GameObject.Find("Cube2");
        propDict.Add(cube,null);
        propDict.Add(cube2,null);
        center = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
        canvas = GameObject.Find("Canvas");
        // leftTimer = 0;
        // rightTimer = 0;
        // priority = 1;
        indicatorModel.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
        above.SetActive(false);
        below.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<GameObject, GameObject> entry in propDict){
            GameObject prop = entry.Key;
            GameObject indicator = entry.Value;
            if (!indicator){
                indicator = Instantiate(indicatorModel);
                indicator.transform.parent = canvas.transform;
                indicator.transform.localScale = new Vector3(1, 1, 1);
                propDict[prop] = indicator;
            }
            Debug.Log("Dictionary entry: " + prop + "," + indicator);

            angleLeftRight = CalculateLeftRightAngle(center, prop);
            angleUpDown = CalculateUpDownAngle(center, prop);
            bool isVisible = angleLeftRight > -55 && angleLeftRight < 55 && angleUpDown > -45 && angleUpDown < 45;
            bool isPlaying = prop.GetComponent<AudioSource>().isPlaying;
            if (isPlaying){
                if (isVisible)
                {
                    indicator.SetActive(false);
                } else
                {
                    indicator.SetActive(true);
                    Vector3 intersection;
                    // update indicator
                    if (findPointerCanvasPosition(out intersection, canvas,  prop)){
                        indicator.transform.localPosition = intersection;

                        // scale alpha w/distance
                        float dist = Vector3.Distance(canvas.transform.position, prop.transform.position);
                        float newAlpha = ((Mathf.Abs(angleLeftRight)/360) + Mathf.Abs(angleLeftRight)/360)*.9f;
                        // Debug.Log("angleUpDown: " + angleUpDown);
                        // Debug.Log("distance: " + dist + "alpha: " + newAlpha);
                        Image indicatorImage = indicator.GetComponent<Image>();
                        Color newColor = indicatorImage.color;
                        newColor.a = newAlpha;
                        indicatorImage.color = newColor;

                        // point indicator in direction of sound
                        float intersectionAngle = findIntersectionAngle(intersection);
                        Debug.Log(intersectionAngle + 180);
                        indicator.transform.eulerAngles = new Vector3(indicator.transform.eulerAngles.x,indicator.transform.eulerAngles.y,intersectionAngle+90);
                    }
                }
            } else {
                indicator.SetActive(false);
            }
        }
        adict = props.PostSoundInformation();
        Debug.Log("Prop Dict");
        foreach (KeyValuePair<GameObject, bool> kvp in adict)
        {
            Debug.Log( string.Format("SoundObject = {0}, SoundOn? = {1}, Volume = {2}, Location = {3}", kvp.Key, kvp.Value, kvp.Key.GetComponent<SoundVolumeGrabber>().postLoudness().ToString(),kvp.Key.transform.position));
        }
        Debug.Log("End of Prop Dict");
    }
}
