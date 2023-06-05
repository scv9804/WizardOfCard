using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    // ==================================================================================================== OnEvent

    public delegate void OnEvent(IEventParameter parameter);

    // ==================================================================================================== IEventParameter

    public interface IEventParameter { }

    // ==================================================================================================== ParameterConveter

    public static class ParameterConveter
    {
        // ==================================================================================================== Method

        // =========================================================================== Parameter

        public static void Casting<TParameter>(this IEventParameter parameter, Action<TParameter> callback) where TParameter : class, IEventParameter
        {
            TParameter converted = parameter as TParameter;

            if (converted != null)
            {
                callback?.Invoke(converted);
            }
        }
    }
}
