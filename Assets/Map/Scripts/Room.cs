using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public Vector2 gridPos;
	public int type;
	public int RoomEventType;
	public bool doorTop, doorBot, doorLeft, doorRight, Checked = false, isStartRoom = false ;
	public int roomNumX , roomNumY;


	public Room(Vector2 _gridPos, int _type, int _RoomEventType )
	{
		gridPos = _gridPos;
		type = _type;
		RoomEventType = _RoomEventType;
	}
}