using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class HealthShieldTest : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;

    public int Shield;

    public float HealthPercentage;
    public float ShieldPercentage;

    public Image HealthBar;
    public Image ShieldBar;

    void Update()
    {
        HealthPercentage = GetHealthPercentage(MaxHealth, CurrentHealth);
        ShieldPercentage = HealthPercentage + GetShieldPercentage(MaxHealth, Shield);

        HealthBar.fillAmount = ShieldPercentage > 1.0f ? HealthPercentage / ShieldPercentage : HealthPercentage;
        ShieldBar.fillAmount = ShieldPercentage > 1.0f ? 1.0f : ShieldPercentage;
    }

    public float GetHealthPercentage(float max, float current)
    {
        return current / max;
    }

    public float GetShieldPercentage(float max, float shield)
    {
        return shield / max;
    }
}
