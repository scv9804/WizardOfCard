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
		// �Ÿ�������
		int multOrder = order * 10;

		//10ĭ ������
		foreach (var renderer in backRenderers)
		{
			renderer.sortingLayerName = sortingLayerName;
			renderer.sortingOrder = multOrder;
		}

		//����ī��� +1�ؼ� ������.
		foreach (var renderer in middleRenderers)
		{
			renderer.sortingLayerName = sortingLayerName;
			renderer.sortingOrder = multOrder + 1;
		}

	}


}
