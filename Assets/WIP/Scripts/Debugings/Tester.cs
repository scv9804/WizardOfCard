using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    public class Tester : MonoBehaviour
    {
        private void Awake()
        {

        }

        void CardTest()
        {
            // ���� ��

            // ��� 0, [����] �� ���� �ϳ����� ü���� 2/4/5 ��ŭ ȸ���ϰ�, ī�带 1�� ��ο��մϴ�.

            for (int i = 0; i < 3; i++) // 3 = EnemyTargetsCount
            {
                Heal();
                Draw();
            }

            // �ر�

            // ��� 2, �������� 12/16/18 �ݴϴ�. ���带 ���� ��󿡰� 2/2/3���� �������� �ݴϴ�.

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
}