using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBoxProperties : MonoBehaviour, IBoxProperties
{
	// Getting stuff
	public AudioSource leaveSound;
	public AudioSource hum;
	public AudioSource desSound;
	private MeshRenderer mr;
	private Rigidbody rig;
	private Collider boxCol;
	private InteractableObject io;
	private SpriteRenderer[] numbersSprites;

	private bool leftStartBox;

	private Vector3 startingPosition;
	private int counter;

	// For fading out
	private float duration = 2;
	private float t;
	//private float tt;
	private bool fading;
    //private bool fadeIn;
	
    private YellowBoxProperties connectedSticky;

	void OnEnable()
	{
		// Setting everything
		rig = this.GetComponent<Rigidbody>();
		mr = this.GetComponent<MeshRenderer>();
		boxCol = this.GetComponent<Collider>();
		io = this.GetComponent<InteractableObject>();
		startingPosition = this.transform.position;
		numbersSprites = this.GetComponentsInChildren<SpriteRenderer>();
	}

	void FixedUpdate()
	{
		if (rig.IsSleeping() == true)
		{
			rig.WakeUp();
		}
		// Fade out and destroy this object if its done for
		if (fading == true)
		{
			float a = Mathf.Lerp(1, 0, t / duration);
			mr.materials[0].color = new Color(mr.materials[0].color.r, mr.materials[0].color.g, mr.materials[0].color.b, a);
			mr.materials[1].color = new Color(mr.materials[1].color.r, mr.materials[1].color.g, mr.materials[1].color.b, a);

			for (int i = 0; i < numbersSprites.Length; i++)
			{
				if (numbersSprites[i] != null)
					numbersSprites[i].color = new Color(1, 1, 1, a);
			}

			t += Time.deltaTime;
			if (t / duration >= 1.3f) 
				Destroy(this.gameObject);
		}

		// If the box gets really far away destroy it to reduce system stress
		if (counter == 60)
		{
			float curDist = Vector3.Distance(startingPosition, this.transform.position);
			if (curDist > 200)
				DestroyBox(false);
			counter = 0;
		}
		counter++;

	}

	/// <summary>
	/// Activates properties of the box (Nothing for this box)
	/// </summary>
	public void ActivateProperties(bool dontPlaySound)
	{
		if (dontPlaySound == false)
		{
			leaveSound.Play();
			hum.Play();
		}
		leftStartBox = true;
		return;
	}

	/// <summary>
	/// Fade out and destroy box
	/// </summary>
	public void DestroyBox(bool playSound)
	{
		if (fading == false)
		{
            if (connectedSticky != null)
                connectedSticky.DestroyFJConnections();
            if (playSound == true)
				desSound.Play();
			if (io != null)
			{
				io.TurnOffTrail();
				io.EndInteraction();
				io.enabled = false;
			}

			string color = "Green";
			if (this.name.Contains("Orange") == true)
				color = "Orange";
			Material[] ms = new Material[2];
			ms[0] = Resources.Load<Material>("Materials/Cube Rim");
			ms[1] = Resources.Load<Material>("Materials/Cube " + color);
			mr.materials = ms;

			fading = true;
			boxCol.enabled = false;
			rig.useGravity = false;
			rig.velocity = Vector3.zero;
			rig.AddTorque(new Vector3(UnityEngine.Random.Range(-5000, 5000), UnityEngine.Random.Range(-5000, 5000), UnityEngine.Random.Range(-5000, 5000)));
		}
	}

	public void OnBoxRelease(Transform wand)
	{
		return;
	}

	public void OnBoxGrab()
	{
		return;
	}

    // Set if there is a sticky block connected to this cube
    public void ConnectedToSticky(YellowBoxProperties ybp)
    {
        connectedSticky = ybp;
    }

	public bool LeftStartBox()
	{
		return leftStartBox;
	}

	public bool Fading()
	{
		return fading;
	}
}
