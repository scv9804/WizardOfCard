using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using UnityEngine.UI;

namespace WIP
{
    public class CardComponents : MonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== Image

        [Header("이미지")]
        [SerializeField] private Image _frameImage;
        [SerializeField] private Image _artworkImage;

        // =========================================================================== TextMeshPro

        [Header("텍스트")]
        [SerializeField] private TMP_Text _nameTMP;
        [SerializeField] private TMP_Text _costTMP;
        [SerializeField] private TMP_Text _descriptionTMP;

        // ==================================================================================================== Property

        // =========================================================================== Image

        public Image FrameImage
        {
            get
            {
                return _frameImage;
            }
        }

        public Image ArtworkImage
        {
            get
            {
                return _artworkImage;
            }
        }

        // =========================================================================== TextMeshPro

        public TMP_Text NameTMP
        {
            get
            {
                return _nameTMP;
            }
        }

        public TMP_Text CostTMP
        {
            get
            {
                return _costTMP;
            }
        }

        public TMP_Text DescriptionTMP
        {
            get
            {
                return _descriptionTMP;
            }
        }
    }
}
