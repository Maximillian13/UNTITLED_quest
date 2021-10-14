using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFadeControl : MonoBehaviour
{
	private SpriteRenderer spriteRend;
	private float alpha;
	private float timer;
	private bool fadeWhite;

	private bool allowRunning = true;

	// Start is called before the first frame update
	void Start()
	{
		spriteRend = this.GetComponent<SpriteRenderer>();
		alpha = 1;
		spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, alpha);
	}

	// Update is called once per frame
	void Update()
	{
		if (allowRunning == false)
			return;

		if (fadeWhite == false)
		{
			alpha -= Time.deltaTime;
			spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, alpha);
		}
		else
		{
			alpha += Time.deltaTime * 2;
			spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, alpha);
		}
	}

	public void FadeWhite()
	{
		fadeWhite = true;
		alpha = 0;
	}

	public void AllowRunning(bool shouldRun)
	{
		allowRunning = shouldRun;
	}
}
