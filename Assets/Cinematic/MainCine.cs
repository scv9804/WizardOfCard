using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class MainCine : MonoBehaviour
{

	public PlayableDirector playable;
	public TimelineAsset timeline;
	float time;

	private void Update()
	{
		if (Input.anyKey)
		{
			time = 0;
		}
		time += Time.deltaTime;
		if (time > 1f)
		{
			playable.Play();
			Debug.Log(true);
		}
		else
		{
			playable.Stop();
		}
	}


}
