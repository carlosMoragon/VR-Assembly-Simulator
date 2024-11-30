using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI pointsText;
    private int realPoints = 90;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(time.IsActive())
        {
            float.TryParse(time.text, out float timeAsFloat);
            Debug.Log(timeAsFloat);

            if (Mathf.RoundToInt(timeAsFloat) % 7 == 0)
            {
                Debug.Log("The value " + Mathf.RoundToInt(timeAsFloat) + " is mult of 7");

                realPoints -= 10;
                pointsText.text = realPoints.ToString();
            }
        }
    }
}
