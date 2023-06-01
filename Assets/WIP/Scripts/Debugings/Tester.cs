using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Globalization;

namespace WIP
{
    public class Tester : MonoBehaviour
    {
        public List<Area> Areas = new List<Area>();

        public List<string> texts = new List<string>();

        private void Awake()
        {

        }

        // GetCard에도 callback 적용하기
                    
        void CardTest()
        {
            // 물의 룬

            // 비용 0, [망각] 적 몬스터 하나마다 체력을 2/4/5 만큼 회복하고, 카드를 1장 드로우합니다.

            for (int i = 0; i < 3; i++) // 3 = EnemyTargetsCount
            {
                Heal();
                Draw();
            }

            // 붕괴

            // 비용 2, 데미지를 12/16/18 줍니다. 쉴드를 가진 대상에게 2/2/3배의 데미지를 줍니다.

            for (int i = 0; i < 3; i++)
            {
                if (TargetShield())
                {
                    ShieldBreak();
                }

                // TargetShield() => ShieldBreak()

                Attack();
            }
        }

        void Heal()
        {

        }

        void Draw()
        {

        }

        bool TargetShield()
        {
            return true;
        }

        void ShieldBreak()
        {

        }

        void Attack()
        {

        }
    }

    [Serializable] public struct Area
    {
        [SerializeField] private float _x;
        [SerializeField] private float _z;

        public float X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        public float Z
        {
            get
            {
                return _z;
            }

            set
            {
                _z = value;
            }
        }
    }
}