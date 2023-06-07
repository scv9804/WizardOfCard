using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    // ==================================================================================================== EventObserver

    public delegate void EventObserver(IEventParameter parameter);

    public delegate TResult EventObserver<TResult>(IEventParameter parameter);

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
