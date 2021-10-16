using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class WandControlGeneralInteraction : MonoBehaviour
{
	// This is a hash-set of all the objects that this wand is interacting with Hash-sets are like lists but with no duplicates inside of it
	private InputDevice hand;    // Todo: Old SteamVr Code
	private HashSet<InteractableObject> hoveredObject = new HashSet<InteractableObject>();
	private CubeButton button;
	private InteractableObject interactingItem;

	private Animator anim;
	private MeshRenderer mr;
	private Color originalColor;
	private Color cubeColor;
	private float colorLerp;

	public bool leftHand;
	private bool lastTriggerState;
	private bool foundHand;
	// Set Up
	void Start()
	{
		anim = this.transform.GetChild(0).GetComponent<Animator>();

		this.TryToFindHand();

		mr = this.transform.GetChild(0).GetChild(2).GetComponent<MeshRenderer>();
		originalColor = mr.materials[1].color;
	}

	void Update()
	{
		if (foundHand == false)
		{
			this.TryToFindHand();
			return;
		}

		bool triggerPressed;
		hand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerPressed);
		float triggerFloat;
		hand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerFloat);

		bool aButtonPress;
		hand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out aButtonPress);


		if (anim != null)
			anim.SetFloat("Blend", triggerFloat);

		if (aButtonPress == true && Time.timeSinceLevelLoad > 1f && SceneManager.GetActiveScene().buildIndex != 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		// If you press the trigger button
		if (triggerPressed == true && lastTriggerState == false)
		{
			// Calculate the minimum distance if there are multiple object you are interacting with
			interactingItem = this.GetClosestItem();

			// If you aren't holding anything
			this.CheckHoldIfNull();

			if (interactingItem != null)
			{
				interactingItem.StartInteraction(this);
				cubeColor = interactingItem.GetCubeColor().color;
			}
		}

		if (triggerPressed == true && lastTriggerState == false && button != null)
			button.PressButton();

		// If you release the trigger button
		if (triggerPressed == false && lastTriggerState == true)
			this.DropBox();

		// Hand Color
		if (interactingItem != null)
		{
			if (colorLerp < 1)
			{
				colorLerp += Time.deltaTime * 8;
				mr.materials[1].color = Color.Lerp(originalColor, cubeColor, colorLerp);
			}
		}
		else
		{
			if (colorLerp > 0)
			{
				colorLerp -= Time.deltaTime * 8;
				mr.materials[1].color = Color.Lerp(originalColor, cubeColor, colorLerp);
			}
		}

		lastTriggerState = triggerPressed;
	}

	private void TryToFindHand()
	{
		XRNode whichHand = leftHand ? XRNode.LeftHand : XRNode.RightHand;
		List<InputDevice> handDevices = new List<InputDevice>();
		InputDevices.GetDevicesAtXRNode(whichHand, handDevices);
		if (handDevices.Count == 1)
		{
			hand = handDevices[0];
			foundHand = true;
		}
		else
			Debug.Log("Have not found " + (leftHand ? "left" : "right") + " hand");
	}

	// If you aren't holding anything
	private void CheckHoldIfNull()
	{
		// Check if you grabbed anything
		if (interactingItem != null)
		{
			// End interaction if you are all ready holding it with other hand
			if (interactingItem.IsInteracting() == true)
			{
				interactingItem.EndInteraction(this);
			}
		}
	}

	public void DropBox()
	{
		// Drop the item
		if (interactingItem != null)
		{
			mr.materials[1].color = originalColor;
			interactingItem.EndInteraction(this);
		}
		if (interactingItem != null)
		{
			interactingItem = null;
		}

		mr.materials[1].color = originalColor;
	}

	public void SetOrigTest()
	{
		mr.materials[1].color = originalColor;
	}

	// Calculate the minimum distance if there are multiple object you are interacting with
	private InteractableObject GetClosestItem()
	{
		float minDistance = float.PositiveInfinity;
		float distance;

		InteractableObject closes = null;

		foreach (InteractableObject item in hoveredObject)
		{
			if (item != null)
			{
				distance = (item.transform.position - this.transform.position).sqrMagnitude;

				if (distance < minDistance)
				{
					minDistance = distance;
					closes = item;
				}
			}
		}

		return closes;
	}

	void OnTriggerEnter(Collider other)
	{
		// Find what its touching
		InteractableObject collidedObject = other.GetComponent<InteractableObject>();
		// Set it up to be carried
		if (collidedObject != null && other.GetType() != typeof(SphereCollider))
			hoveredObject.Add(collidedObject);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "BoxButton")
			button = other.GetComponent<CubeButton>();
	}

	void OnTriggerExit(Collider other)
	{
		// Find what its touching
		InteractableObject collidedObject = other.GetComponent<InteractableObject>();
		// Set it up to be dropped
		if (collidedObject != null && other.GetType() != typeof(SphereCollider))
		{
			// Set the color to be un-highlighted
			hoveredObject.Remove(collidedObject);
		}

		CubeButton cb = other.GetComponent<CubeButton>();
		if (cb != null)
		{
			button = null;
		}
	}
}
