using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
	public static MusicManager inst;

	[Header("�Ҹ� ũ�� ����")]
	[Range(0,1)]public float BGMSound = 0.04f;
	[Range(0,1)]public float EffectSound = 0.04f;


	[Header("BGM ����")]
	public AudioSource backGruondSource;
	[Header("���� ����Ʈ : �������� �̸��̶� �����")]
	public AudioClip[] backGroundClips;

	[Header("ȿ���� ���� ��")]
	[SerializeField] AudioSource[] effectSource;

	[Header("ȿ����")]
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
		Debug.LogError("��� ���� �ҽ� �迭 ��� ��");
	}

	public void BackGroundSoundPlay(AudioClip clip)
	{
		backGruondSource.clip = clip;
		backGruondSource.loop = true;
		backGruondSource.volume = BGMSound;
		backGruondSource.Play();
	}

}
