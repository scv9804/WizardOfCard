using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLayer : MonoBehaviour
{
	[SerializeField]
	Renderer[] backRenderers;
	[SerializeField]
	Renderer[] middleRenderers;
	[SerializeField]
	string sortingLayerName;
	int originOrder;

	public void SetOriginOrder(int originOrder)
	{
		this.originOrder = originOrder;
		SetOrder(originOrder);
	}

	public void SetMostFrontOrder(bool isMostFront)
	{
		SetOrder(isMostFront ? 100 : originOrder);
	}

	public void SetOrder(int order)
	{
		// 거리벌리기
		int multOrder = order * 10;

		//10칸 앞으로
		foreach (var renderer in backRenderers)
		{
			renderer.sortingLayerName = sortingLayerName;
			renderer.sortingOrder = multOrder;
		}

		//다음카드는 +1해서 앞으로.
		foreach (var renderer in middleRenderers)
		{
			renderer.sortingLayerName = sortingLayerName;
			renderer.sortingOrder = multOrder + 1;
		}

	}


}
