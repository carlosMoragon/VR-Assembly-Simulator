using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI timeText;
	[SerializeField] private float remainTime = 60;
	int min = 0;
	int sec = 0;
	public GameObject Menu;
	public GameObject timeYes;
	public GameObject OriginXRR;

	// Update is called once per frame
	void Update()
	{
		if (!Menu.active)
        {
			if (remainTime > 0)
			{
				remainTime -= Time.deltaTime;
				min = Mathf.FloorToInt(remainTime / 60);
				sec = Mathf.FloorToInt(remainTime % 60);
			}
			else if (remainTime <= 0)
			{
				// Debug.Log("Exiting game...");
				Menu.SetActive(true);
				//OriginXRL.SetActive(false);
				//OriginXRR.SetActive(false);
			}
			timeText.text = string.Format("{0:00}:{1:00}", min, sec);
		}
	}
}
