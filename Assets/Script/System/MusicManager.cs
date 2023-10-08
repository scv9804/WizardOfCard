using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
	public static MusicManager inst;

	[Header("소리 크기 조절")]
	[Range(0,1)]public float BGMSound = 0.25f;
	[Range(0,1)]public float EffectSound = 0.25f;


	[Header("BGM 공간")]
	public AudioSource backGruondSource;
	[Header("음악 리스트 : 스테이지 이름이랑 맞출것")]
	public AudioClip[] backGroundClips;

	[Header("효과음 넣을 곳")]
	[SerializeField] AudioSource[] effectSource;
	
	// 효과음
	[Header("효과음 - 적 몬스터")]
	public AudioClip slashSound;
	public AudioClip warCrySound;
	public AudioClip enemyDisappear;

	[Header("효과음 - UI")]
	public AudioClip Audio_Myturn;
	public AudioClip Audio_Enemyturn;

	[Space(10)]
	public AudioClip Audio_OnMouseDown;
	public AudioClip Audio_OnMouseUp;

	[Header("효과음 - 카드")]
	public AudioClip Audio_CardClick;

	[Space(10)]
	public AudioClip playerDefultSoundEffect;
	public AudioClip Audio_Barrier;

	[Header("효과음 - 상점")]
	public AudioClip Audio_Shop;
	public AudioClip Audio_Money;

	float times = 0.25f;
	bool isClicked = false;

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

    private void Update()
    {
		PlayClickSound();
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

	// <<22-11-28 장형용 :: 추가>>
	// <<22-12-01 장형용 :: 오류 뜨지 않도록 수정>>
	// 장형용 :: 20231007 :: 매커니즘 수정
	void PlayClickSound()
    {
        //if(Input.GetMouseButtonDown(0) && !isClicked && IsMouseOnCard())
        //      {
        //	EffectPlay(Audio_OnMouseDown);
        //	isClicked = true;
        //}

        //if(isClicked)
        //	times += Time.deltaTime;

        //if (times > 0.125f && !Input.GetMouseButton(0) && isClicked && IsMouseOnCard())
        //      {
        //	EffectPlay(Audio_OnMouseUp);

        //	times = 0;
        //	isClicked = false;
        //}

        if (Input.GetMouseButtonDown(0) && !isClicked)
        {
			OnClick();
		}
    }

	private void OnClick()
    {
		StartCoroutine(Main());

		IEnumerator Main()
        {
			EffectPlay(Audio_OnMouseDown);
			isClicked = true;

            var enumerator = CheckMouseButtonUp();

            StartCoroutine(enumerator);

			yield return new WaitForSeconds(0.1f);

			yield return new WaitUntil(() =>
			{
				return enumerator.Current == null;
			});

			EffectPlay(Audio_OnMouseUp);
			isClicked = false;
		}

        IEnumerator CheckMouseButtonUp()
        { 
            yield return new WaitUntil(() =>
			{
				return Input.GetMouseButtonUp(0);
			});

			yield return null;
		}
	}

	// <<22-12-01 장형용 :: 추가>>
	bool IsMouseOnCard()
    {
		//if (CardManager.Inst == null)
		//	return true;
		//else if (!CardManager.Inst.is_mouseOnCard)
		//	return true;
		//else
			return false;
    }

	public void PlayBarrierSound()
    {
		EffectPlay(Audio_Barrier);
	}

	public void PlayShopSound()
    {
		EffectPlay(Audio_Shop);
	}

	public void PlayMyTurnSound()
    {
		EffectPlay(Audio_Myturn);
	}

	public void PlayEnemyTurnSound()
	{
		EffectPlay(Audio_Enemyturn);
	}

	public void PlayBuyingSound()
    {
		EffectPlay(Audio_Money);
	}

	public void PlayCardClickSound()
	{
		EffectPlay(Audio_CardClick);
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
