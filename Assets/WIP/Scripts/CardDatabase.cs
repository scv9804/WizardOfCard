using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.ObjectModel;

namespace WIP
{
    [CreateAssetMenu(menuName = "WIP/Card/Database", fileName = "CardDatabase")]
    public class CardDatabase : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        // ================================================== Model

        [Header("원본 데이터 컨테이너")]
        [SerializeField] private List<CardOriginalData> _originals = new List<CardOriginalData>();

        [Header("구현 데이터 컨테이너")]
        [SerializeField] private List<CardHandlerData> _handlers = new List<CardHandlerData>();

        // =========================================================================== Asset

        // ================================================== Model

        [Header("에셋 데이터 컨테이너")]
        [SerializeField] private List<CardAssetData> _assets = new List<CardAssetData>();

        // ==================================================================================================== Property

        // =========================================================================== Data

        // ================================================== Model

        public ReadOnlyCollection<CardOriginalData> Originals
        {
            get
            {
                return _originals.AsReadOnly();
            }
        }

        public ReadOnlyCollection<CardHandlerData> Handlers
        {
            get
            {
                return _handlers.AsReadOnly();
            }
        }

        // =========================================================================== Asset

        // ================================================== Model

        public ReadOnlyCollection<CardAssetData> Assets
        {
            get
            {
                return _assets.AsReadOnly();
            }
        }
    }
}
