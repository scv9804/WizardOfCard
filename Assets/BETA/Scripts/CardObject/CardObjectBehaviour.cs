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
        public abstract class ObjectBehaviour : MonoBehaviour
        {
            // ==================================================================================================== Property

            // =========================================================================== Identifier

            public abstract string InstanceID
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

            // =========================================================================== Component

            public virtual void Refresh()
            {
                if (InstanceID is null || !Instance.Data.ContainsKey(InstanceID))
                {
                    return;
                }

                int serialID = Instance.Data[InstanceID].SerialID;
                int level = Instance.Data[InstanceID].Level;

                var data = Original.Data.Create(serialID, level);

                FrameImage.sprite = data.FrameSprite;
                ArtworkImage.sprite = data.ArtworkSprite;

                NameTMP.text = Instance.Data[InstanceID].Name;
                CostTMP.text = Instance.Data[InstanceID].Cost.ToString();
                DescriptionTMP.text = Instance.Data[InstanceID].Description;
            }
        }
    }
}