using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WIP
{
    [CreateAssetMenu(menuName = "WIP/Radious", fileName = "_CardRadious")]
    public class Radious : ScriptableObject
    {
        [Header("범위 vect3 형식으로 입력")]
        public List<Vector3> radiousList;
    }
}

