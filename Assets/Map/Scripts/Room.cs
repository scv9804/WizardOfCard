using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
	public Vector2 gridPos;
	public int type;
	public int RoomEventType; //0: normal 1: Boss 2: Shop 3: Event
	public bool doorTop, doorBot, doorLeft, doorRight, Checked = false, isStartRoom = false ;
	public int roomNumX , roomNumY;


	public Room(Vector2 _gridPos, int _type, int _RoomEventType )
	{
		gridPos = _gridPos;
		type = _type;
		RoomEventType = _RoomEventType;
	}
}