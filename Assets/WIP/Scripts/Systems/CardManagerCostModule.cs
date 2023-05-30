using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{

    // ==================================================================================================== CardManagerCostModule

    public class CardManagerCostModule
    {
        // ==================================================================================================== Field

        // =========================================================================== Decorator

        private List<ICardManagerCostModuleDecorator> _decorators = new List<ICardManagerCostModuleDecorator>()
        {
            new DefaultCostModuleDecorator()
        };

        // =========================================================================== Result

        private int _cost;

        private bool _isEnough;

        // ==================================================================================================== Property

        // =========================================================================== Result

        public int Cost
        {
            get
            {
                return _cost;
            }

            set
            {
                _cost = value;
            }
        }

        public bool IsEnough
        {
            get
            {
                return _isEnough;
            }

            set
            {
                _isEnough = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Decorator

        public void AddDecorator(ICardManagerCostModuleDecorator decorator)
        {
            _decorators.Add(decorator);
        }

        public void RemoveDecorator(ICardManagerCostModuleDecorator decorator)
        {
            _decorators.Remove(decorator);
        }

        // =========================================================================== Module

        public void Estimate()
        {
            Clear();

            for (int i = 0; i < _decorators.Count; i++)
            {
                _decorators[i].Estimate(this);

                if (IsEnough)
                {
                    break;
                }
            }
        }

        public void Execute()
        {
            Clear();
        }

        private void Clear()
        {
            Cost = 0;

            IsEnough = false;
        }
    }

    // ==================================================================================================== DefaultCostModuleDecorator

    public class DefaultCostModuleDecorator : ICardManagerCostModuleDecorator
    {
        // ==================================================================================================== Method

        // =========================================================================== Cost

        public void Estimate(CardManagerCostModule module)
        {
            ////////////////////////////////////////////////// BETA
            module.IsEnough = true;
            ////////////////////////////////////////////////// BETA

            //if (module.Cost >= EntityManager.Inst.playerEntity.Status_Aether)
            //{
            //    module.IsEnough = true;
            //}
        }
    }

    // ==================================================================================================== ICardManagerCostModuleDecorator

    public interface ICardManagerCostModuleDecorator
    {
        // ==================================================================================================== Method

        // =========================================================================== Cost

        public void Estimate(CardManagerCostModule module);
    }
}

