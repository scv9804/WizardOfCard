using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpriteSelector : MonoBehaviour {
	
	public Sprite 	spU, spD, spR, spL,
			spUD, spRL, spUR, spUL, spDR, spDL,
			spULD, spRUL, spDRU, spLDR, spUDRL;
	public bool up, down, left, right;
	public int type; // 0: normal, 1: enter 2: SetActiveFalse 3: SetActiveTrue And NotSerchedYet
	public Color normalColor, enterColor , noSerchColor;
	Color mainColor;
	SpriteRenderer rend;

	public GameObject RoomIcon;
	GameObject spawnedBossIcon;

	void Start () {
		rend = GetComponent<SpriteRenderer>();
		mainColor = normalColor;
		PickSprite();
		PickColor();
	}

	//스프라이트 비교하여 문 선택하기.
	void PickSprite(){ 
		if (up){
			if (down){
				if (right){
					if (left){
						rend.sprite = spUDRL;
					}else{
						rend.sprite = spDRU;
					}
				}else if (left){
					rend.sprite = spULD;
				}else{
					rend.sprite = spUD;
				}
			}else{
				if (right){
					if (left){
						rend.sprite = spRUL;
					}else{
						rend.sprite = spUR;
					}
				}else if (left){
					rend.sprite = spUL;
				}else{
					rend.sprite = spU;
				}
			}
			return;
		}
		if (down){
			if (right){
				if(left){
					rend.sprite = spLDR;
				}else{
					rend.sprite = spDR;
				}
			}else if (left){
				rend.sprite = spDL;
			}else{
				rend.sprite = spD;
			}
			return;
		}
		if (right){
			if (left){
				rend.sprite = spRL;
			}else{
				rend.sprite = spR;
			}
		}else{
			rend.sprite = spL;
		}
	}

	// 색상 선택// 0: normal, 1: enter 2: SetActiveFalse 3: NotSerchedYet 4 : BossRoom

	public void PickColor()
	{
		if (type == 0)
		{
			this.gameObject.SetActive(true);
			spawnedBossIcon?.SetActive(true);
			mainColor = normalColor;
		}
		else if (type == 1)
		{
			this.gameObject.SetActive(true);
			spawnedBossIcon?.SetActive(true);
			mainColor = enterColor;
		}
		else if (type == 2)
		{
			this.gameObject.SetActive(false);
		}
		else if (type == 4)
		{
			this.gameObject.SetActive(false);
			SetBoss();
		}
		else if (type == 3)
		{
			this.gameObject.SetActive(true);
			spawnedBossIcon?.SetActive(true);
			mainColor = noSerchColor;
		}
	
		rend.color = mainColor;
	}



	public void SetBoss()
	{
		if (type == 4 && spawnedBossIcon == null)
		{
			spawnedBossIcon = Instantiate(RoomIcon, this.transform.position, Quaternion.identity);
			spawnedBossIcon.SetActive(false) ;
		}
	}



}