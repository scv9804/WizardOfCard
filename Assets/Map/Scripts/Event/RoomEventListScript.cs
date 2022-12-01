using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomEventListScript : MonoBehaviour
{
	public abstract GameObject Event();
	public abstract void ExitRoom();
}
