using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
	public static MusicManager inst;

	[Header("소리 크기 조절")]
	[Range(0,1)]public float BGMSound = 0.04f;
	[Range(0,1)]public float EffectSound = 0.04f;


	[Header("BGM 공간")]
	public AudioSource backGruondSource;
	[Header("음악 리스트 : 스테이지 이름이랑 맞출것")]
	public AudioClip[] backGroundClips;

	[Header("효과음 넣을 곳")]
	[SerializeField] AudioSource[] effectSource;

	[Header("효과음")]
	[SerializeField] AudioClip slashSound;
	[SerializeField] AudioClip warCrySound;
	[SerializeField] AudioClip playerDefultSoundEffect;
	[SerializeField] AudioClip enemyDisappear;

	
	private void Awake()
	{
		if (inst == null)
		{
			inst = this;
			DontDestroyOnLoad(inst);
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	void OnSceneLoaded(Scene _scene, LoadSceneMode _loadSceneManager)
	{
		for (int i = 0; i<backGroundClips.Length;i++)
		{
			if(_scene.name == backGroundClips[i].name)
			{
				BackGroundSoundPlay(backGroundClips[i]);
			}
		}
	}

	public void SlashSound()
	{
			EffectPlay(slashSound);
	}
	public void WarCrySound()
	{
			EffectPlay(warCrySound);
	}
	public void PlayerDefultSoundEffect()
	{
		EffectPlay(playerDefultSoundEffect);
	}
	public void EnemyDisappear() 
	{
		EffectPlay(enemyDisappear);
	}

	void EffectPlay(AudioClip clip)
	{
		for (int i = 0; i < effectSource.Length; i++)
		{
			if(!effectSource[i].isPlaying)
			{
				effectSource[i].clip = clip;
				effectSource[i].loop = false;
				effectSource[i].volume = EffectSound;
				effectSource[i].Play();
				return;
			}
		}
		Debug.LogError("모든 사운드 소스 배열 사용 중");
	}

	public void BackGroundSoundPlay(AudioClip clip)
	{
		backGruondSource.clip = clip;
		backGruondSource.loop = true;
		backGruondSource.volume = BGMSound;
		backGruondSource.Play();
	}

}
