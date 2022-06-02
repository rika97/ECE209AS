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
    public GameObject canvas;
    private float angleFromForward;
    private float angleFromUp;
    private Queue<GameObject> rings = new Queue<GameObject>();
    private Queue<GameObject> spheres = new Queue<GameObject>();
    // private int sound;
    private int leftTimer;
    private int rightTimer;
    private int priority;
    private Image leftImage;
    private Image squareImage;
    private Image rightImage;
    Vector3 bottomLeft = new Vector3(-.75f,-.54f,0f);
    Vector3 bottomRight = new Vector3(.75f,-.54f,0f);
    Vector3 topLeft = new Vector3(-.75f,.54f,0f);
    Vector3 topRight = new Vector3(.75f,.54f,0f);

    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){
        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if( Mathf.Abs(planarFactor) < 0.0001f 
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) 
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            // Debug.Log("line intersection " + intersection);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }
    public static bool LineSegmentIntersection(out Vector3 intersection, Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2){
        Vector3 from = a2 - a1;
        Vector3 line = b2 - b1;
        if (LineLineIntersection(out intersection, a1, from, b1, line)){
            float aSqrMagnitude = from.sqrMagnitude;
            float bSqrMagnitude = line.sqrMagnitude;

            if (    (intersection - a1).sqrMagnitude <= aSqrMagnitude  
                 && (intersection - a2).sqrMagnitude <= aSqrMagnitude  
                 && (intersection - b1).sqrMagnitude <= bSqrMagnitude 
                 && (intersection - b2).sqrMagnitude <= bSqrMagnitude)
            {
                // Debug.Log("segment intersection " + intersection);
                return true;
            } 
        }
        intersection = Vector3.zero;
        return false;
    }
    private bool findPointerCanvasPosition(out Vector3 intersection, GameObject c, GameObject target){

        // transform 3D positions onto normal plane
        Vector3 a1 = Vector3.zero;
        // fix this local/world CoC issue!!!
        Vector3 a2 = target.transform.position;
        // Vector3 a2 = Vector3.ProjectOnPlane(target.transform.position, c.transform.forward);

        // target
        Debug.DrawLine(c.transform.position, a2);

        //normal
        Debug.DrawLine(c.transform.position, c.transform.position + c.transform.forward);
        Vector3 forward = new Vector3(0f,0f,1f);
        a2 = Vector3.ProjectOnPlane(c.transform.InverseTransformPoint(target.transform.position), forward);
        // // projected
        // Debug.DrawLine(c.transform.position, c.transform.TransformPoint(a2));

        if (LineSegmentIntersection(out intersection, a1, a2, topRight, topLeft)){
            // Debug.Log("top segment intersection " + intersection);
            return true;
        // bottom
        } else if (LineSegmentIntersection(out intersection, a1, a2, bottomRight, bottomLeft)){
            // Debug.Log("bottom segment intersection " + intersection);
            return true;
        // left
        } else if (LineSegmentIntersection(out intersection, a1, a2, topLeft, bottomLeft)){
            // Debug.Log("left segment intersection " + intersection);
            return true;
        // right
        } else if (LineSegmentIntersection(out intersection, a1, a2, topRight, bottomRight)){
            // Debug.Log("right intersection");
            return true;
        } 
        // Debug.Log("NO intersection");
        intersection = Vector3.zero;
        return false;
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
        canvas = GameObject.Find("Canvas");
        Debug.Log("bottomLeft " + bottomLeft);
        Debug.Log("bottomRight " + bottomRight);
        Debug.Log("topLeft " + topLeft);
        Debug.Log("topright " + topRight);
        // sound = 0;
        leftTimer = 0;
        rightTimer = 0;
        priority = 1;

        leftImage = left.GetComponent<Image>();
        rightImage = right.GetComponent<Image>();
        squareImage = square.GetComponent<Image>();

        left.SetActive(true);
        right.SetActive(true);
        above.SetActive(true);
        below.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        angleFromForward = CalculateAngleFromForward(center, rippleCube);
        angleFromUp = CalculateAngleFromUp(center, rippleCube);
        if (angleFromForward > -60 && angleFromForward < 60 && angleFromUp > -120 && angleFromUp < -60)
        {
            square.SetActive(false);
        } else
        {
            square.SetActive(true);
            Vector3 intersection;
            // update indicator
            if (findPointerCanvasPosition(out intersection, canvas,  rippleCube)){
                square.transform.localPosition = intersection;

                // scale alpha w/distance
                float dist = Vector3.Distance(canvas.transform.position, rippleCube.transform.position);
                float newAlpha = 1f - dist/9f;
                Debug.Log("distance: " + dist + "alpha: " + newAlpha);
                Color newColor = squareImage.color;
                newColor.a = newAlpha;
                squareImage.color = newColor;

                // point indicator in direction of sound
                float intersectionAngle = findIntersectionAngle(intersection);
                Debug.Log(intersectionAngle + 180);
                square.transform.eulerAngles = new Vector3(square.transform.eulerAngles.x,square.transform.eulerAngles.y,intersectionAngle+180);
            }
        }
    }
}
