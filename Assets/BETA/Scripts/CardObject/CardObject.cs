using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BETA
{
    // ==================================================================================================== CardObject

    public sealed class CardObject : Card.ObjectBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [Header("개체 ID")]
        [SerializeField] private string _instanceID;

        // =========================================================================== Component

        // ================================================== Image

        [Header("이미지")]
        [SerializeField] private Image _frameImage;
        [SerializeField] private Image _artworkImage;

        // ================================================== TextMeshPro

        [Header("텍스트")]
        [SerializeField] private TMP_Text _nameTMP;
        [SerializeField] private TMP_Text _costTMP;
        [SerializeField] private TMP_Text _descriptionTMP;

        // ==================================================================================================== Property

        // =========================================================================== Identifier

        public override string InstanceID
        {
            get
            {
                return _instanceID;
            }

            protected set
            {
                _instanceID = value;
            }
        }

        // =========================================================================== Component

        // ================================================== Image

        protected override Image FrameImage
        {
            get
            {
                return _frameImage;
            }
        }

        protected override Image ArtworkImage
        {
            get
            {
                return _artworkImage;
            }
        }

        // ================================================== TextMeshPro

        protected override TMP_Text NameTMP
        {
            get
            {
                return _nameTMP;
            }
        }

        protected override TMP_Text CostTMP
        {
            get
            {
                return _costTMP;
            }
        }

        protected override TMP_Text DescriptionTMP
        {
            get
            {
                return _descriptionTMP;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Awake()
        {
            
        }

        private void Update()
        {
            Refresh();
        }

        // =========================================================================== Instance

        public static CardObject Create(string instanceID)
        {
            var gameObject = Instantiate(Card.Original.Prefab, GameObject.Find("[ Cards ]").transform);

            var cardObject = gameObject.GetComponent<CardObject>();

            cardObject.InstanceID = instanceID;

            cardObject.transform.position = new Vector2(Random.Range(0, 1920), Random.Range(-0, 1080)); // TEMP

            return cardObject;
        }

        // =========================================================================== Component

        public override void Refresh()
        {
            base.Refresh();
        }
    }
}