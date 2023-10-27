using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ================================================================================ EventListener

public delegate void EventListener();

public delegate void EventListener<T0>(T0 parameter0);

public delegate void EventListener<T0, T1>(T0 parameter0, T1 parameter1);

public delegate void EventListener<T0, T1, T2>(T0 parameter0, T1 parameter1, T2 parameter2);

public delegate void EventListener<T0, T1, T2, T3>(T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3);