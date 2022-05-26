using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class RoomAudioController : MonoBehaviour
{
    [SerializeField] private readonly int nSoundsSelected;
    [SerializeField] private GameObject textDisplay;

    private static Dictionary<GameObject, bool> adict;


    // Start is called before the first frame update
    void Start()
    {
        adict = new Dictionary<GameObject, bool>();

        List<GameObject> objList = new List<GameObject>();

        var myObjectArray = GameObject.FindGameObjectsWithTag("wSound");
        int nSoundObjects = myObjectArray.Length;

        Debug.Log("found "+nSoundObjects+" wSound objects!");

        foreach(GameObject obj in myObjectArray)
        {
            objList.Add(obj);
        }

        Assert.IsTrue(nSoundsSelected < nSoundObjects);
        
        for (int i=0; i< nSoundsSelected; i++)
        {
            // select random sound + remove to avoid duplicate
            int rand_number = Random.Range(0,nSoundObjects-i);
            GameObject selectedObject = objList[rand_number];
            Debug.Log("selected : "+ selectedObject.name);
            objList.Remove(selectedObject);

            // Call loop method inside objects wSound
            var myAudioScript = selectedObject.GetComponent<TimedTriggerAudio>();
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
 
/*        foreach (KeyValuePair<string, GameObject> kvp in adict)
        {
            Debug.Log("in dict print");
            Debug.Log( string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
            Debug.Log( string.Format("Transform: {0}", kvp.Value.transform.position)); // display object position
            Debug.Log( string.Format("Is audio playing right now? {0}", kvp.Value.GetComponent<AudioSource>().isPlaying)); // display object position
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> keys = new List<GameObject>(adict.Keys);
        foreach (GameObject key in keys)
        {
            if(key.GetComponent<AudioSource>().isPlaying != adict[key]) // on change
            {
                adict[key] = !adict[key];
                Debug.Log( string.Format("{0} nowPlaying {1}", key.name, adict[key]) );
            }
        }

    }

    public Dictionary<GameObject, bool> PostSoundInformation()
    {
        textDisplay.GetComponent<ScoreDisplay>().changeText();
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
