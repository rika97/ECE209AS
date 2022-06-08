using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RoomAudioController : MonoBehaviour
{
    public int nSoundsSelected;
    private static Dictionary<GameObject, bool> adict;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        adict = new Dictionary<GameObject, bool>();
        List<GameObject> objList = new List<GameObject>();
        var myObjectArray = GameObject.FindGameObjectsWithTag("wSound");
        int nSoundObjects = myObjectArray.Length;

        foreach (GameObject obj in myObjectArray)
        {
            objList.Add(obj);
        }

        Assert.IsTrue(nSoundsSelected <= nSoundObjects);
        // Debug.Log("nSoundsSelected: " + nSoundsSelected);
        for (int i = 0; i < nSoundsSelected; i++)
        {
            // select random sound + remove to avoid duplicate
            int rand_number = Random.Range(0, nSoundObjects - i);
            GameObject selectedObject = objList[rand_number];
            Debug.Log("selected : " + i + ":"+ selectedObject.name);
            adict.Add(selectedObject, selectedObject.GetComponent<AudioSource>().isPlaying);
            objList.Remove(selectedObject);

            // delay launch audio clips ----
            float rand_delay = Random.Range(3.0f, 7.0f);
            // Debug.LogWarning("launching " + selectedObject.name + " in " + rand_delay.ToString() + " s.");
            selectedObject.GetComponent<ObjectAudioController>().StartWDelay(rand_delay);
        }

    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> keys = new List<GameObject>(adict.Keys);
        foreach (GameObject key in keys)
        {
            if (key.GetComponent<AudioSource>().isPlaying != adict[key]) // WILL ADVERTISE CHANGES
            {
                adict[key] = !adict[key];
                Debug.Log(string.Format("{0} nowPlaying {1}", key.name, adict[key]));
            }
        }

    }

    public Dictionary<GameObject, bool> PostSoundInformation()
    {
        return adict;
    }

    public void EnableViz()
    {
        foreach (KeyValuePair<GameObject, bool> kvp in adict)
        {
            //kvp.Key.GetComponent<RippleViz>().BeginRipple();
            kvp.Key.GetComponent<RippleViz>().rippleOn = true;
        }
    }

    public void DisableViz()
    {
        foreach (KeyValuePair<GameObject, bool> kvp in adict)
        {
            //kvp.Key.GetComponent<RippleViz>().StopRipple();
            kvp.Key.GetComponent<RippleViz>().rippleOn = false;
        }
    }

    public void StopSounds()
    {
        foreach (KeyValuePair<GameObject, bool> kvp in adict)
        {
            kvp.Key.GetComponent<AudioSource>().Stop();
        }
    }


    //void OnGUI()
    //{
    //    //if (GUILayout.Button("Start sounds", GUILayout.Width(300), GUILayout.Height(20)))
    //    //{

    //    //}
    //    if (GUILayout.Button("Timer objs", GUILayout.Width(300), GUILayout.Height(20)))
    //    {
    //        foreach (KeyValuePair<GameObject, bool> kvp in adict)
    //        {
    //            Debug.LogWarning(string.Format("SoundObject = {0}, TimerElapsed? = {1}", kvp.Key.name, kvp.Key.GetComponent<ObjectAudioController>().timer.Elapsed )) ;
    //        }
    //    }
    //    if (GUILayout.Button("Restart", GUILayout.Width(300), GUILayout.Height(20)))
    //    {

    //    }
    //}
}
