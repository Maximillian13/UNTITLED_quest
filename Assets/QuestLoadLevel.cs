using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestLoadLevel : MonoBehaviour
{
	public GameObject cube;
	private EyeFadeControl eyeFade;
	private bool done;

	// The point of this is to give the quest a second to load from a fresh start so the game doesnt start laggy
	void Start()
	{
		eyeFade = GameObject.Find("XR Rig").transform.Find("Camera Offset").Find("Main Camera").Find("EyeCover").GetComponent<EyeFadeControl>();
		eyeFade.AllowRunning(false); // Prevent eye from fading
		cube.SetActive(false); // Get rid of box
	}


	void Update()
	{
		// After 2 second enable the box and let the eyes fade
		if (Time.timeSinceLevelLoad > 2 && done == false)
		{
			eyeFade.AllowRunning(true);
			cube.SetActive(true);
			done = true;
		}
	}

}
