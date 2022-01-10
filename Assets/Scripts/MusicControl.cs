using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{

	private float timeAlive = 0;

	// Dont destroy the music until the end 
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	void Update()
	{
		timeAlive += Time.deltaTime;
	}

	public float GetTimeAlive()
	{
		return timeAlive;
	}

}
