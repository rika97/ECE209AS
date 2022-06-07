using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* ----------------------------------------------------------
 * ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣴⣶⣿⣿⣷⣶⣄⣀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀
 * ⠀⠀⠀⠀⠀⠀⠀⠀⣰⣾⣿⣿⡿⢿⣿⣿⣿⣿⣿⣿⣿⣷⣦⡀⠀⠀⠀⠀⠀
 * ⠀⠀⠀⠀⠀⠀⢀⣾⣿⣿⡟⠁⣰⣿⣿⣿⡿⠿⠻⠿⣿⣿⣿⣿⣧⠀⠀⠀⠀
 * ⠀⠀⠀⠀⠀⠀⣾⣿⣿⠏⠀⣴⣿⣿⣿⠉⠀⠀⠀⠀⠀⠈⢻⣿⣿⣇⠀⠀⠀
 * ⠀⠀⠀⢀⣠⣼⣿⣿⡏⠀⢠⣿⣿⣿⠇⠀⠀⠀⠀⠀⠀⠀⠈⣿⣿⣿⡀⠀⠀
 * ⠀⠀⣰⣿⣿⣿⣿⣿⡇⠀⢸⣿⣿⣿⡀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⡇⠀⠀
 * ⠀⢰⣿⣿⡿⣿⣿⣿⡇⠀⠘⣿⣿⣿⣧⠀⠀⠀⠀⠀⠀⢀⣸⣿⣿⣿⠁⠀⠀
 * ⠀⣿⣿⣿⠁⣿⣿⣿⡇⠀⠀⠻⣿⣿⣿⣷⣶⣶⣶⣶⣶⣿⣿⣿⣿⠃⠀⠀⠀
 * ⢰⣿⣿⡇⠀⣿⣿⣿⠀⠀⠀⠀⠈⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⠟⠁⠀⠀⠀⠀
 * ⢸⣿⣿⡇⠀⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠉⠛⠛⠛⠉⢉⣿⣿⠀⠀⠀⠀⠀⠀
 * ⢸⣿⣿⣇⠀⣿⣿⣿⠀⠀⠀⠀⠀⢀⣤⣤⣤⡀⠀⠀⢸⣿⣿⣿⣷⣦⠀⠀⠀
 * ⠀⢻⣿⣿⣶⣿⣿⣿⠀⠀⠀⠀⠀⠈⠻⣿⣿⣿⣦⡀⠀⠉⠉⠻⣿⣿⡇⠀⠀
 * ⠀⠀⠛⠿⣿⣿⣿⣿⣷⣤⡀⠀⠀⠀⠀⠈⠹⣿⣿⣇⣀⠀⣠⣾⣿⣿⡇⠀⠀
 * ⠀⠀⠀⠀⠀⠀⠹⣿⣿⣿⣿⣦⣤⣤⣤⣤⣾⣿⣿⣿⣿⣿⣿⣿⣿⡟⠀⠀⠀
 * ⠀⠀⠀⠀⠀⠀⠀⠀⠉⠻⢿⣿⣿⣿⣿⣿⣿⠿⠋⠉⠛⠋⠉⠉⠁⠀⠀⠀⠀
 * ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠉⠉⠁
 *  ______   _______  _______  _______ 
 * (  __  \ (  ____ \(       )(  ___  )
 * | (  \  )| (    \/| () () || (   ) |
 * | |   ) || (__    | || || || |   | |
 * | |   | ||  __)   | |(_)| || |   | |
 * | |   ) || (      | |   | || |   | |
 * | (__/  )| (____/\| )   ( || (___) |
 * (______/ (_______/|/     \|(_______)
 *  _        _______  _______  _______ 
 * ( \      (  ___  )(  ___  )(  ____ )
 * | (      | (   ) || (   ) || (    )|
 * | |      | |   | || |   | || (____)|
 * | |      | |   | || |   | ||  _____)
 * | |      | |   | || |   | || (      
 * | (____/\| (___) || (___) || )      
 * (_______/(_______)(_______)|/       
 *                                    
 *                                                                         
 * - triggered onboot
 * - cycles every __ seconds (default 30s)
 * - ontimeout:
 *  -- 10s pause
 *  -- flash screen to say demo has ended
 *  -- relaunch experience with rand.range(1-4) sounds
 *  -- rand experience selection (viz+sound, viz+nosound) w probability 2/3 1/3
 * - turn off sounds via raycasting
 *  -- prototype raycasting behavior via screenbutton
 * 
 ------------------------------------------------------------*/

public class DemoLoop : MonoBehaviour
{
    public GameObject SceneAudioParent;
    public GameObject HeadVisualization;
    public AudioLowPassFilter LPF;
    private bool SoundToggle = true;

    public float timeRemaining = 30;
    public bool timerIsRunning = true;
    private void Start()
    {
        // Starts the timer automatically
        //timerIsRunning = true;

        SceneAudioParent = GameObject.FindGameObjectWithTag("SoundsParent");
        HeadVisualization = GameObject.FindGameObjectWithTag("HeadViz");
        LPF = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioLowPassFilter>();
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                if (SceneAudioParent.GetComponent<RoomAudioController>().enabled == false)
                {
                    int aNumber = Random.Range(1, 2);
                    float rand = Random.value;
                    if (rand <= .66f)
                    {
                        // soundOn
                        StartExperience(aNumber, true);
                    }
                    else
                    {
                        //soundOff
                        StartExperience(aNumber, false);
                    }
                    return;
                }
                timeRemaining = 30;
                //timerIsRunning = false;

                Dictionary<GameObject, bool> aDict = SceneAudioParent.GetComponent<RoomAudioController>().PostSoundInformation();
                bool audioActive = false;
                foreach (bool soundsOn in aDict.Values)
                {
                    if (soundsOn == true) { audioActive = true; break; }
                }
                if (!audioActive)
                {
                    // launch
                    int aNumber = Random.Range(1, 4);
                    float rand = Random.value;
                    if (rand <= .66f)
                    {
                        // soundOn
                        StartExperience(aNumber, true);
                    }
                    else
                    {
                        //soundOff
                        StartExperience(aNumber, false);
                    }
                }
            }
        }
    }

    void StartExperience(int nSounds, bool wSound)
    {
        LPF.enabled = !wSound;
        SceneAudioParent.GetComponent<RoomAudioController>().nSoundsSelected = nSounds;
        SceneAudioParent.GetComponent<RoomAudioController>().enabled = true;
    }

    void OnGUI()
    {
        if (GUILayout.Button("StopAudio", GUILayout.Width(300), GUILayout.Height(20)))
        {
            StopAudio();
        }
    }

        void StopAudio()
    {
        SceneAudioParent.GetComponent<RoomAudioController>().StopSounds();
        SceneAudioParent.GetComponent<RoomAudioController>().enabled = false;

    }
}
