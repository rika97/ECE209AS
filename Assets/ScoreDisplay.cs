using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text TextComponent;
    
    private float playEverySeconds = 5;
    private float timePassed = 0;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        TextComponent = this.GetComponent<TMP_Text>();
        Debug.Log("SCOREDISPLAY- myText "+TextComponent.text);
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= playEverySeconds  )
        {
            timePassed = 0;
            TextComponent.text = "CUSTOMTEXT"+counter++;
        }
    }
}
