using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA
{
    // ==================================================================================================== Card.ManagerBehaviour

    public sealed partial class Card
    {
        public abstract class ManagerBehaviour<TManager> : MonoSingleton<TManager> where TManager : ManagerBehaviour<TManager>
        {
            // ==================================================================================================== Property

            // =========================================================================== Card

            public abstract List<Card> Owned
            {
                get;
            }

            public abstract List<Card> Deck
            {
                get;
            }

            public abstract List<Card> Hand
            {
                get;
            }

            public abstract List<Card> Discard
            {
                get;
            }

            public abstract List<Card> Exiled
            {
                get;
            }

            // ==================================================================================================== Method

            // =========================================================================== Event

            protected override void Awake()
            {
                base.Awake();

                ReadAllData();
            }

            protected override void OnApplicationQuit()
            {
                base.OnApplicationQuit();

                ReadAllData();
            }

            // =========================================================================== Card (BETA)

            public abstract void Acquire(Card card);

            // =========================================================================== Data

            protected abstract void Clear();

            // =========================================================================== BETA

            public abstract void OnCardManagerAwake();
        }
    }
}
