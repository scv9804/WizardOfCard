using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityPattern : ScriptableObject
{
	public abstract bool Pattern(Entity _entity);
}
