using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using TMPro;

using UnityEngine.UI;

namespace BETA
{
    // ==================================================================================================== CardObjectComponents

    public sealed class CardObjectComponents : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== Component

        // ================================================== Image

        [FoldoutGroup("이미지")]
        public Image FrameImage;

        [FoldoutGroup("이미지")]
        public Image ArtworkImage;

        // ================================================== Text

        [FoldoutGroup("텍스트")]
        public TMP_Text NameTMP;

        [FoldoutGroup("텍스트")]
        public TMP_Text CostTMP;

        [FoldoutGroup("텍스트")]
        public TMP_Text DescriptionTMP;
    }
}
