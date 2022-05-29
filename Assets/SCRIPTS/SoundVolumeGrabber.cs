using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolumeGrabber : MonoBehaviour
{

    public float updateStep = 0.1f;
    public int sampleDataLength = 256;

    private AudioSource audioSource;
    private float currentUpdateTime = 0f;

    private float clipLoudness;
    private float[] clipSampleData;

    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();

        if (!audioSource)
        {
            Debug.LogError(GetType() + ".Awake: there was no AudioSource set");
        }
        clipSampleData = new float[sampleDataLength];
    }

    // Update is called once per frame
    void Update()
    {
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); // 1024 samples @ 44khz ~80ms
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength;
        }
    }

    public float postLoudness()
    {
        Debug.Log("loudness " + audioSource.clip.ToString() +": "+ clipLoudness);

        return clipLoudness;
    }

    void OnGUI()
    {
        //if (GUILayout.Button("postLoudness"))
            if (GUI.Button(new Rect(0, 20, 100, 30), "postLoudness"))
            {
            postLoudness();
        }
    }
}
