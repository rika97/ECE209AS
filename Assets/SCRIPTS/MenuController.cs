using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public GameObject SceneAudioParent;
    public GameObject HeadVisualization;
    public AudioLowPassFilter LPF;
    private bool VizToggle = true;
    private bool SoundToggle = true;

    // Start is called before the first frame update
    void Start()
    {
        SceneAudioParent = GameObject.FindGameObjectWithTag("SoundsParent");
        HeadVisualization = GameObject.FindGameObjectWithTag("HeadViz");
        LPF = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioLowPassFilter>();
    }

    public void NoViz()
    {
        VizToggle = false;
        SoundToggle = true;
    }
    public void SoundAndViz()
    {
        VizToggle = true;
        SoundToggle = true;
    }
    public void VizOnly()
    {
        VizToggle = true;
        SoundToggle = false;
    }
    public void L1()
    {
        int aNumber = Random.Range(1, 2);
        StartExperience(aNumber, VizToggle, SoundToggle);
    }
    public void L2()
    {
        int aNumber = Random.Range(2, 4);
        StartExperience(aNumber, VizToggle, SoundToggle);
    }

    public void L3()
    {
        int aNumber = Random.Range(4, 6);
        StartExperience(aNumber, VizToggle, SoundToggle);
    }

    public void Exit()
    {
        StopAudio();
    }


    // Use to test same functions from VR buttons
    void OnGUI()
    {
        if (GUILayout.Button("No Viz", GUILayout.Width(300), GUILayout.Height(20)))
        {
            VizToggle = false;
            SoundToggle = true;
        }
        if (GUILayout.Button("Sound+Viz", GUILayout.Width(300), GUILayout.Height(20)))
        {
            VizToggle = true;
            SoundToggle = true;
        }
        if (GUILayout.Button("Viz Only", GUILayout.Width(300), GUILayout.Height(20)))
        {
            VizToggle = true;
            SoundToggle = false;
        }

        if (GUILayout.Button("Level1", GUILayout.Width(300), GUILayout.Height(20)))
        {
            int aNumber = Random.Range(1, 2);
            Debug.LogWarning("starting experience w " + aNumber + " sounds");
            StartExperience(aNumber, VizToggle, SoundToggle);
        }
        if (GUILayout.Button("Level2", GUILayout.Width(300), GUILayout.Height(20)))
        {
            int aNumber = Random.Range(2, 4);
            Debug.LogWarning("starting experience w " + aNumber + " sounds");
            StartExperience(aNumber, VizToggle, SoundToggle);
        }
        if (GUILayout.Button("Level3", GUILayout.Width(300), GUILayout.Height(20)))
        {
            int aNumber = Random.Range(4, 6);
            Debug.LogWarning("starting experience w " + aNumber + " sounds");
            StartExperience(aNumber, VizToggle, SoundToggle);
        }
        if (GUILayout.Button("STOP", GUILayout.Width(300), GUILayout.Height(20)))
        {
            // TODO: recap scores and display
            StopAudio();
        }

    }

    public void StartExperience(int nSounds, bool wViz, bool wSound)
    {
        LPF.enabled = !wSound;
        SceneAudioParent.GetComponent<RoomAudioController>().nSoundsSelected = nSounds;
        SceneAudioParent.GetComponent<RoomAudioController>().enabled = true;
        if (wViz)
        {
            HeadVisualization.GetComponent<Canvas>().enabled = true;
            HeadVisualization.GetComponent<Visualization>().enabled = true;
            //HeadVisualization.SetActive(true);
            SceneAudioParent.GetComponent<RoomAudioController>().EnableViz();
        }
        else
        {
            HeadVisualization.GetComponent<Canvas>().enabled = false ;
            HeadVisualization.GetComponent<Visualization>().enabled = false;
            //HeadVisualization.SetActive(false);
            SceneAudioParent.GetComponent<RoomAudioController>().DisableViz();
        }
        //gameObject.SetActive(false);
    }
    
    void StopAudio()
    {
        HeadVisualization.GetComponent<Canvas>().enabled = false;
        HeadVisualization.GetComponent<Visualization>().enabled = false;
        SceneAudioParent.GetComponent<RoomAudioController>().StopSounds();
        SceneAudioParent.GetComponent<RoomAudioController>().enabled = false;
    }
}
