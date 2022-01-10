using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevelMusicControl : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");

		// There will only be two at most
		if (musics.Length == 2)
		{
			if (musics[0].GetComponent<MusicControl>().GetTimeAlive() > musics[1].GetComponent<MusicControl>().GetTimeAlive())
				Destroy(musics[1]);
			else
				Destroy(musics[0]);
		}
	}
}
