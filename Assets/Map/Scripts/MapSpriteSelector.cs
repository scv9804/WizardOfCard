using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpriteSelector : MonoBehaviour {
	
	public Sprite 	spU, spD, spR, spL,
			spUD, spRL, spUR, spUL, spDR, spDL,
			spULD, spRUL, spDRU, spLDR, spUDRL;
	public Sprite b_spU, b_spD, b_spR, b_spL,
		b_spUD, b_spRL, b_spUR, b_spUL, b_spDR, b_spDL,
		b_spULD, b_spRUL, b_spDRU, b_spLDR, b_spUDRL;
	public Sprite shop_spU, shop_spD, shop_spR, shop_spL,
		shop_spUD, shop_spRL, shop_spUR, shop_spUL, shop_spDR, shop_spDL,
		shop_spULD, shop_spRUL, shop_spDRU, shop_spLDR, shop_spUDRL;
	public Sprite even_spU, even_spD, even_spR, even_spL,
			even_spUD, even_spRL, even_spUR, even_spUL, even_spDR, even_spDL,
			even_spULD, even_spRUL, even_spDRU, even_spLDR, even_spUDRL;
	public Sprite nowPos_spU, nowPos_spD, nowPos_spR, nowPos_spL,
		nowPos_spUD, nowPos_spRL, nowPos_spUR, nowPos_spUL, nowPos_spDR, nowPos_spDL,
		nowPos_spULD, nowPos_spRUL, nowPos_spDRU, nowPos_spLDR, nowPos_spUDRL;

	public bool up, down, left, right;
	public int type; // 0: normal, 1: enter 2: SetActiveFalse 3: SetActiveTrue And NotSerchedYet
	enum DoorSide {L, R ,D ,U, UD, RL, UR, UL, DR, DL, ULD, RUL, DRU, LDR, UDRL };
	DoorSide door;


	public int RoomEventType; // 0: normal 1: Boss 2: Shop 3: Event
	public Color normalColor, enterColor , noSerchColor ;
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
						door = DoorSide.UDRL;
					}else{
						rend.sprite = spDRU;
						door = DoorSide.DRU;
					}
				}else if (left){
					rend.sprite = spULD;
					door = DoorSide.ULD;
				}
				else{
					rend.sprite = spUD;
					door = DoorSide.UD;
				}
			}else{
				if (right){
					if (left){
						rend.sprite = spRUL;
						door = DoorSide.RUL;
					}
					else{
						rend.sprite = spUR;
						door = DoorSide.UR;
					}
				}else if (left){
					rend.sprite = spUL;
					door = DoorSide.UL;
				}
				else{
					rend.sprite = spU;
					door = DoorSide.U;
				}
			}
			return;
		}
		if (down){
			if (right){
				if(left){
					rend.sprite = spLDR;
					door = DoorSide.LDR;
				}
				else{
					rend.sprite = spDR;
					door = DoorSide.DR;
				}
			}else if (left){
				rend.sprite = spDL;
				door = DoorSide.DL;
			}
			else{
				rend.sprite = spD;
				door = DoorSide.D;
			}
			return;
		}
		if (right){
			if (left){
				rend.sprite = spRL;
				door = DoorSide.RL;
			}
			else{
				rend.sprite = spR;
				door = DoorSide.R;
			}
		}else{
			rend.sprite = spL;
			door = DoorSide.L;
		}
	}

	// 색상 선택// 0: normal, 1: enter 2: SetActiveFalse 3: NotSerchedYet 4 : BossRoom

	public void PickColor()
	{
		if (type == 0)
		{
			this.gameObject.SetActive(true);
			//spawnedBossIcon?.SetActive(true);
			SetRoom();
			mainColor = normalColor;
		}
		else if (type == 1)
		{
			this.gameObject.SetActive(true);
			//spawnedBossIcon?.SetActive(true);
			SetNowPosRoomSprite();
			mainColor = normalColor;
		}
		else if (type == 2)
		{
			this.gameObject.SetActive(false);
		}
		else if (type == 3)
		{
			this.gameObject.SetActive(true);
	//		spawnedBossIcon?.SetActive(true);
			mainColor = noSerchColor;
		}
		else if (type == 4)
		{
			this.gameObject.SetActive(false);
			SetBossRoomSprite();
		}
		else if (type == 5)
		{
		
		}

		rend.color = mainColor;
	}

	// 0: normal 1: Boss 2: Shop 3: Event
	void SetRoom()
	{
		if (RoomEventType == 0)
		{
			SetDefultRoomSprite();
			
		}
		if (RoomEventType == 1)
		{
			SetBossRoomSprite();
		}
		if (RoomEventType == 2)
		{
			SetShopRoomSprite();
		}
		if (RoomEventType == 3)
		{
			SetEventRoomSprite();
		}
	}

	#region SetRoomDoorSprite
	public void SetNowPosRoomSprite()
	{
		switch (door)
		{
			case DoorSide.D:
				rend.sprite = nowPos_spD;
				break;
			case DoorSide.DL:
				rend.sprite = nowPos_spDL;
				break;
			case DoorSide.DR:
				rend.sprite = nowPos_spDR;
				break;
			case DoorSide.DRU:
				rend.sprite = nowPos_spDRU;
				break;
			case DoorSide.L:
				rend.sprite = nowPos_spL;
				break;
			case DoorSide.LDR:
				rend.sprite = nowPos_spLDR;
				break;
			case DoorSide.R:
				rend.sprite = nowPos_spR;
				break;
			case DoorSide.RL:
				rend.sprite = nowPos_spRL;
				break;
			case DoorSide.RUL:
				rend.sprite = nowPos_spRUL;
				break;
			case DoorSide.U:
				rend.sprite = nowPos_spU;
				break;
			case DoorSide.UD:
				rend.sprite = nowPos_spUD;
				break;
			case DoorSide.UDRL:
				rend.sprite = nowPos_spUDRL;
				break;
			case DoorSide.UL:
				rend.sprite = nowPos_spUL;
				break;
			case DoorSide.ULD:
				rend.sprite = nowPos_spULD;
				break;
			case DoorSide.UR:
				rend.sprite = nowPos_spUR;
				break;
		}
	}

	public void SetBossRoomSprite()
	{
		switch (door)
		{
			case DoorSide.D :
				rend.sprite = b_spD;
				break;
			case DoorSide.DL:
				rend.sprite = b_spDL;
				break;
			case DoorSide.DR:
				rend.sprite = b_spDR;
				break;
			case DoorSide.DRU:
				rend.sprite = b_spDRU;
				break;
			case DoorSide.L:
				rend.sprite = b_spL;
				break;
			case DoorSide.LDR:
				rend.sprite = b_spLDR;
				break;
			case DoorSide.R:
				rend.sprite = b_spR;
				break;
			case DoorSide.RL:
				rend.sprite = b_spRL;
				break;
			case DoorSide.RUL:
				rend.sprite = b_spRUL;
				break;
			case DoorSide.U:
				rend.sprite = b_spU;
				break;
			case DoorSide.UD:
				rend.sprite = b_spUD;
				break;
			case DoorSide.UDRL:
				rend.sprite = b_spUDRL;
				break;
			case DoorSide.UL:
				rend.sprite = b_spUL;
				break;
			case DoorSide.ULD:
				rend.sprite = b_spULD;
				break;
			case DoorSide.UR:
				rend.sprite = b_spUR;
				break;
		}
		
	}

	public void SetDefultRoomSprite()
	{
		switch (door)
		{
			case DoorSide.D:
				rend.sprite = spD;
				break;
			case DoorSide.DL:
				rend.sprite = spDL;
				break;
			case DoorSide.DR:
				rend.sprite = spDR;
				break;
			case DoorSide.DRU:
				rend.sprite = spDRU;
				break;
			case DoorSide.L:
				rend.sprite = spL;
				break;
			case DoorSide.LDR:
				rend.sprite = spLDR;
				break;
			case DoorSide.R:
				rend.sprite = spR;
				break;
			case DoorSide.RL:
				rend.sprite = spRL;
				break;
			case DoorSide.RUL:
				rend.sprite = spRUL;
				break;
			case DoorSide.U:
				rend.sprite = spU;
				break;
			case DoorSide.UD:
				rend.sprite = spUD;
				break;
			case DoorSide.UDRL:
				rend.sprite = spUDRL;
				break;
			case DoorSide.UL:
				rend.sprite = spUL;
				break;
			case DoorSide.ULD:
				rend.sprite = spULD;
				break;
			case DoorSide.UR:
				rend.sprite = spUR;
				break;
		}
	}

	public void SetEventRoomSprite()
	{
		switch (door)
		{
			case DoorSide.D:
				rend.sprite = even_spD;
				break;
			case DoorSide.DL:
				rend.sprite = even_spDL;
				break;
			case DoorSide.DR:
				rend.sprite = even_spDR;
				break;
			case DoorSide.DRU:
				rend.sprite = even_spDRU;
				break;
			case DoorSide.L:
				rend.sprite = even_spL;
				break;
			case DoorSide.LDR:
				rend.sprite = even_spLDR;
				break;
			case DoorSide.R:
				rend.sprite = even_spR;
				break;
			case DoorSide.RL:
				rend.sprite = even_spRL;
				break;
			case DoorSide.RUL:
				rend.sprite = even_spRUL;
				break;
			case DoorSide.U:
				rend.sprite = even_spU;
				break;
			case DoorSide.UD:
				rend.sprite = even_spUD;
				break;
			case DoorSide.UDRL:
				rend.sprite = even_spUDRL;
				break;
			case DoorSide.UL:
				rend.sprite = even_spUL;
				break;
			case DoorSide.ULD:
				rend.sprite = even_spULD;
				break;
			case DoorSide.UR:
				rend.sprite = even_spUR;
				break;
		}
	}

	public void SetShopRoomSprite()
	{
		switch (door)
		{
			case DoorSide.D:
				rend.sprite = shop_spD;
				break;
			case DoorSide.DL:
				rend.sprite = shop_spDL;
				break;
			case DoorSide.DR:
				rend.sprite = shop_spDR;
				break;
			case DoorSide.DRU:
				rend.sprite = shop_spDRU;
				break;
			case DoorSide.L:
				rend.sprite = shop_spL;
				break;
			case DoorSide.LDR:
				rend.sprite = shop_spLDR;
				break;
			case DoorSide.R:
				rend.sprite = shop_spR;
				break;
			case DoorSide.RL:
				rend.sprite = shop_spRL;
				break;
			case DoorSide.RUL:
				rend.sprite = shop_spRUL;
				break;
			case DoorSide.U:
				rend.sprite = shop_spU;
				break;
			case DoorSide.UD:
				rend.sprite = shop_spUD;
				break;
			case DoorSide.UDRL:
				rend.sprite = shop_spUDRL;
				break;
			case DoorSide.UL:
				rend.sprite = shop_spUL;
				break;
			case DoorSide.ULD:
				rend.sprite = shop_spULD;
				break;
			case DoorSide.UR:
				rend.sprite = shop_spUR;
				break;
		}
	}

	#endregion
}