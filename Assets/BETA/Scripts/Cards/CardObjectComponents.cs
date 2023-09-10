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

        [FoldoutGroup("�̹���")]
        public Image FrameImage;

        [FoldoutGroup("�̹���")]
        public Image ArtworkImage;

        // ================================================== Text

        [FoldoutGroup("�ؽ�Ʈ")]
        public TMP_Text NameTMP;

        [FoldoutGroup("�ؽ�Ʈ")]
        public TMP_Text CostTMP;

        [FoldoutGroup("�ؽ�Ʈ")]
        public TMP_Text DescriptionTMP;
    }
}
