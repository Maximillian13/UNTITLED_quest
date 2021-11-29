using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSkipControl : MonoBehaviour
{
	// Receptacles and shelf (Shelf element = 0, Rec = rest)
	public GameObject[] recepticles;
	public GameObject normalReceptical;

	// Disable it all
	void OnEnable()
	{
		for (int x = 0; x < recepticles.Length; x++)
			recepticles[x].SetActive(false);
	}

	// Re-enable based on what levels they have been to
	void Start()
	{
		int levelsCompleted = PlayerPrefs.GetInt("Level");

		// Todo: This needs to be updated to fit the new levels 
		if (levelsCompleted >= 1)
		{
			recepticles[0].SetActive(true);
			normalReceptical.SetActive(false);
		}

		if (levelsCompleted >= 12)
			recepticles[1].SetActive(true);
		if (levelsCompleted >= 21)
			recepticles[2].SetActive(true);
		if (levelsCompleted >= 30)
			recepticles[3].SetActive(true);
		if (levelsCompleted >= 39)
			recepticles[4].SetActive(true);
		if (levelsCompleted >= 51)
			recepticles[5].SetActive(true);
		if (levelsCompleted >= 61)
			recepticles[6].SetActive(true);
		if (levelsCompleted >= 63)
			recepticles[7].SetActive(true);
		if (levelsCompleted >= 71)
			recepticles[8].SetActive(true);
		if (levelsCompleted >= 79)
			recepticles[9].SetActive(true);
		if (levelsCompleted >= 90)
			recepticles[10].SetActive(true);
		if (levelsCompleted >= 98)
			recepticles[11].SetActive(true);
		if (levelsCompleted >= 106)
		{
			recepticles[12].SetActive(true);
			recepticles[13].SetActive(true);
		}
	}
}
