using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.ObjectModel;

namespace WIP
{
    [CreateAssetMenu(menuName = "WIP/Card/AssetData", fileName = "_AssetData")]
    public class CardAssetData : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Asset

        // ================================================== Component

        [Header("������ ��������Ʈ")]
        [SerializeField] private List<Sprite> _frameSprite = new List<Sprite>(Card.MAX_UPGRADE_LEVEL + 1);

        [Header("������ ��������Ʈ")]
        [SerializeField] private List<Sprite> _artworkSprite = new List<Sprite>(Card.MAX_UPGRADE_LEVEL + 1);

        // ================================================== Effect

        [Header("���� ����Ʈ ��������Ʈ")]
        [SerializeField] private Sprite _attackEffectSprite;

        [Header("�ǰ� ����Ʈ ��������Ʈ")]
        [SerializeField] private Sprite _hitEffectSprite;

        // ==================================================================================================== Property

        // =========================================================================== Asset

        // ================================================== Component

        public ReadOnlyCollection<Sprite> FrameSprite
        {
            get
            {
                return _frameSprite.AsReadOnly();
            }
        }

        public ReadOnlyCollection<Sprite> ArtworkSprite
        {
            get
            {
                return _artworkSprite.AsReadOnly();
            }
        }

        // ================================================== Effect

        public Sprite AttackEffectSprite
        {
            get
            {
                return _attackEffectSprite;
            }
        }

        public Sprite HitEffectSprite
        {
            get
            {
                return _hitEffectSprite;
            }
        }
    }
}
