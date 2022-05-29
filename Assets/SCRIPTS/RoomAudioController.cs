using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RoomAudioController : MonoBehaviour
{
    [SerializeField] private int nSoundsSelected;

    private static Dictionary<GameObject, bool> adict;

    // Start is called before the first frame update
    void Start()
    {
        adict = new Dictionary<GameObject, bool>();

        List<GameObject> objList = new List<GameObject>();

        var myObjectArray = GameObject.FindGameObjectsWithTag("wSound");
        int nSoundObjects = myObjectArray.Length;

        Debug.Log("found " + nSoundObjects + " wSound objects!");

        foreach (GameObject obj in myObjectArray)
        {
            objList.Add(obj);
        }

        Assert.IsTrue(nSoundsSelected < nSoundObjects);

        for (int i = 0; i < nSoundsSelected; i++)
        {
            // select random sound + remove to avoid duplicate
            int rand_number = Random.Range(0, nSoundObjects - i);
            GameObject selectedObject = objList[rand_number];
            Debug.Log("selected : " + selectedObject.name);
            objList.Remove(selectedObject);

            // Call loop method inside objects wSound ---- 
            var myAudioScript = selectedObject.GetComponent<ObjectAudioController>();
            myAudioScript.changeLoopPeriod(Random.Range(0, 20));
            myAudioScript.toggleLoop(true);


            if (adict.ContainsKey(selectedObject))
            {
                adict[selectedObject] = selectedObject.GetComponent<AudioSource>().isPlaying;
            }
            else
            {
                adict.Add(selectedObject, selectedObject.GetComponent<AudioSource>().isPlaying);
            }

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
        foreach (KeyValuePair<GameObject, bool> kvp in adict)
        {
            Debug.Log("in dict print");
            Debug.Log( string.Format("SoundObject = {0}, SoundOn? = {1}, Volume = {2}, Location = {3}", kvp.Key, kvp.Value, kvp.Key.GetComponent<SoundVolumeGrabber>().postLoudness().ToString(),kvp.Key.transform.position));
        }
        return adict;
    }


    void OnGUI()
    {
        if (GUILayout.Button("PostSoundInformation"))
        {
            PostSoundInformation();
        }
    }
}
