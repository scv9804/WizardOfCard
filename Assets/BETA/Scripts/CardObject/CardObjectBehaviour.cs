using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BETA
{
    // ==================================================================================================== Card.ObjectBehaviour

    public sealed partial class Card
    {
        public abstract class ObjectBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
        {
            // ==================================================================================================== Property

            // =========================================================================== Identifier

            public abstract string InstanceID
            {
                get; protected set;
            }

            // =========================================================================== CardObject

            public abstract bool CanDrag
            {
                get; protected set;
            }

            // =========================================================================== Component

            // ================================================== Image

            protected abstract Image FrameImage
            {
                get;
            }

            protected abstract Image ArtworkImage
            {
                get;
            }

            // ================================================== TextMeshPro

            protected abstract TMP_Text NameTMP
            {
                get;
            }

            protected abstract TMP_Text CostTMP
            {
                get;
            }

            protected abstract TMP_Text DescriptionTMP
            {
                get;
            }

            // ==================================================================================================== Method

            // =========================================================================== EventSystems

            public abstract void OnBeginDrag(PointerEventData eventData);

            public abstract void OnDrag(PointerEventData eventData);

            public abstract void OnEndDrag(PointerEventData eventData);

            // =========================================================================== Component

            public virtual void Refresh()
            {
                if (InstanceID is null || !Instance.Data.ContainsKey(InstanceID))
                {
                    return;
                }

                int serialID = Instance.Data[InstanceID].SerialID;
                int level = Instance.Data[InstanceID].Level;

                var originalData = Original.Data.Create(serialID, level);
                var instanceData = Instance.Data[InstanceID];

                FrameImage.sprite = originalData.FrameSprite;
                ArtworkImage.sprite = originalData.ArtworkSprite;

                NameTMP.text = instanceData.Name;
                CostTMP.text = instanceData.Cost.ToString();
                DescriptionTMP.text = instanceData.Description;
            }
        }
    }
}