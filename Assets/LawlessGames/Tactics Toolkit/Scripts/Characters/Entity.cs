using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// ******
using Sirenix.OdinInspector;

using Spine.Unity;

namespace TacticsToolkit
{
    //Parent Class for Characters and Enemys
    public class Entity : MonoBehaviour
    {
        [Header("Character Specific")]
        public List<AbilityContainer> abilitiesForUse;

        [Header("Level")]
        public int level;
        public int experience = 0;
        public int requiredExperience = 0;

        [Header("General")]
        public int teamID = 0;
        [HideInInspector]
        public OverlayTile activeTile;
        public CharacterClass characterClass;
        [HideInInspector]
        public CharacterStats statsContainer;
        [HideInInspector]
        public int initiativeValue;

        [HideInInspector]
        public bool isAlive = true;
        [HideInInspector]
        public bool isActive;
        public GameEvent endTurn;
        public Image healthBar;
        [HideInInspector]
        public int previousTurnCost = -1;

        private bool isTargetted = false;
        [HideInInspector]
        //public SpriteRenderer myRenderer;

        public GameConfig gameConfig;

        private int initiativeBase = 1000;
        private float i;

        // ******
        public GameEventGameObject EntityDie;

        public CharacterRenderer Renderer;

        private void Awake()
        {
            SpawnCharacter();
        }

        public void SpawnCharacter()
        {
            SetAbilityList();
            SetStats();
            requiredExperience = gameConfig.GetRequiredExp(level);

            //myRenderer = gameObject.GetComponent<SpriteRenderer>();
            initiativeValue = Mathf.RoundToInt(initiativeBase / GetStat(Stats.Speed).statValue);
        }

        //Setup the statsContainer and scale up the stats based on level. 
        public void SetStats()
        {
            if (statsContainer == null)
            {
                statsContainer = ScriptableObject.CreateInstance<CharacterStats>();
                statsContainer.Health = new Stat(Stats.Health, characterClass.Health.baseStatValue, this);
                statsContainer.Mana = new Stat(Stats.Mana, characterClass.Mana.baseStatValue, this);
                statsContainer.Strenght = new Stat(Stats.Strenght, characterClass.Strenght.baseStatValue, this);
                statsContainer.Endurance = new Stat(Stats.Endurance, characterClass.Endurance.baseStatValue, this);
                statsContainer.Speed = new Stat(Stats.Speed, characterClass.Speed.baseStatValue, this);
                statsContainer.Intelligence = new Stat(Stats.Intelligence, characterClass.Intelligence.baseStatValue, this);
                statsContainer.MoveRange = new Stat(Stats.MoveRange, characterClass.MoveRange, this);
                statsContainer.AttackRange = new Stat(Stats.AttackRange, characterClass.AttackRange, this);
                statsContainer.CurrentHealth = new Stat(Stats.CurrentHealth, characterClass.Health.baseStatValue, this);
                statsContainer.CurrentMana = new Stat(Stats.CurrentMana, characterClass.Mana.baseStatValue, this);

                //******
                statsContainer.Shield = new Stat(Stats.Shield, 0, this);
            }

            for (int i = 0; i < level; i++)
            {
                LevelUpStats();
            }
        }

        // Update is called once per frame
        public void Update()
        {
            //if (isTargetted)
            //{
            //    //Just a Color Lerp for when a character is targetted for an attack. 
            //    i += Time.deltaTime * 0.5f;
            //    myRenderer.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 0.5f, 0, 1), Mathf.PingPong(i * 2, 1));
            //}
        }

        //Get's all the available abilities from the characters class. 
        public void SetAbilityList()
        {
            abilitiesForUse = new List<AbilityContainer>();
            foreach (var ability in characterClass.abilities)
            {
                if (level >= ability.requiredLevel)
                    abilitiesForUse.Add(new AbilityContainer(ability));
            }
        }

        //Scale up attributes based on a weighted random. 
        public void LevelUpStats()
        {
            float v = Random.Range(0f, 1f);
            statsContainer.Health.ChangeStatValue(statsContainer.Health.statValue + Mathf.RoundToInt(characterClass.Health.baseStatModifier.Evaluate(v) * 10));
            v = Random.Range(0f, 1f);
            statsContainer.Mana.ChangeStatValue(statsContainer.Mana.statValue + Mathf.RoundToInt(characterClass.Mana.baseStatModifier.Evaluate(v) * 10));
            v = Random.Range(0f, 1f);
            statsContainer.Strenght.ChangeStatValue(statsContainer.Strenght.statValue + Mathf.RoundToInt(characterClass.Strenght.baseStatModifier.Evaluate(v) * 10));
            v = Random.Range(0f, 1f);
            statsContainer.Endurance.ChangeStatValue(statsContainer.Endurance.statValue + Mathf.RoundToInt(characterClass.Endurance.baseStatModifier.Evaluate(v) * 10));
            v = Random.Range(0f, 1f);
            statsContainer.Speed.ChangeStatValue(statsContainer.Speed.statValue + Mathf.RoundToInt(characterClass.Speed.baseStatModifier.Evaluate(v) * 10));
            v = Random.Range(0f, 1f);
            statsContainer.Intelligence.ChangeStatValue(statsContainer.Intelligence.statValue + Mathf.RoundToInt(characterClass.Intelligence.baseStatModifier.Evaluate(v) * 10));

            statsContainer.CurrentHealth.ChangeStatValue(statsContainer.Health.statValue);
            statsContainer.CurrentMana.ChangeStatValue(statsContainer.Mana.statValue);
        }

        //Scale down attributes based on a weighted random. 
        public void LevelDownStats()
        {
            float v = Random.Range(0f, 1f);
            statsContainer.Health.ChangeStatValue(statsContainer.Health.statValue - Mathf.RoundToInt(characterClass.Health.baseStatModifier.Evaluate(v) * 10));
            v = Random.Range(0f, 1f);
            statsContainer.Mana.ChangeStatValue(statsContainer.Mana.statValue - Mathf.RoundToInt(characterClass.Mana.baseStatModifier.Evaluate(v) * 10));
            v = Random.Range(0f, 1f);
            statsContainer.Strenght.ChangeStatValue(statsContainer.Strenght.statValue - Mathf.RoundToInt(characterClass.Strenght.baseStatModifier.Evaluate(v) * 10));
            v = Random.Range(0f, 1f);
            statsContainer.Endurance.ChangeStatValue(statsContainer.Endurance.statValue - Mathf.RoundToInt(characterClass.Endurance.baseStatModifier.Evaluate(v) * 10));
            v = Random.Range(0f, 1f);
            statsContainer.Speed.ChangeStatValue(statsContainer.Speed.statValue - Mathf.RoundToInt(characterClass.Speed.baseStatModifier.Evaluate(v) * 10));
            v = Random.Range(0f, 1f);
            statsContainer.Intelligence.ChangeStatValue(statsContainer.Intelligence.statValue - Mathf.RoundToInt(characterClass.Intelligence.baseStatModifier.Evaluate(v) * 10));

            statsContainer.CurrentHealth.ChangeStatValue(statsContainer.Health.statValue);
            statsContainer.CurrentMana.ChangeStatValue(statsContainer.Mana.statValue);
        }

        //Level up stats and get the new required experience for the next level. 
        public void LevelUpCharacter()
        {
            level++;
            LevelUpStats();
            requiredExperience = gameConfig.GetRequiredExp(level);
        }

        public void IncreaseExp(int value)
        {
            experience += value;

            while (experience >= requiredExperience)
            {
                experience -= requiredExperience;
                LevelUpCharacter();
            }
        }

        //Level down stats and get the new required experience for the next level. 
        public void LevelDownCharacter()
        {
            level--;
            LevelDownStats();
            requiredExperience = gameConfig.GetRequiredExp(level);
        }

        //Update the characters initiative after the perform an action. This is used for Dynamic Turn Order. 
        public void UpdateInitiative(int turnValue)
        {
            initiativeValue += Mathf.RoundToInt(turnValue / GetStat(Stats.Speed).statValue + 1);
            previousTurnCost = turnValue;
        }

        //Entity is being targets for an attack. 
        public void SetTargeted(bool focused = false)
        {
            isTargetted = focused;

            //if (isAlive)
            //{
            //    if (isTargetted)
            //    {
            //        myRenderer.color = new Color(1, 0, 0, 1);
            //    }
            //    else
            //    {
            //        myRenderer.color = new Color(1, 1, 1, 1);
            //    }
            //}
        }

        //Take damage from an attack or ability. 
        public void TakeDamage(int damage, bool ignoreDefence = false)
        {
            int damageToTake = ignoreDefence ? damage : CalculateDamage(damage);

            //Debug.Log($"{damageToTake} / {statsContainer.Shield.statValue}");
            $"{damageToTake} / {statsContainer.Shield.statValue}".Log();

            // ******
            if (statsContainer.Shield.statValue > damageToTake)
            {
                statsContainer.Shield.statValue -= damageToTake;
                damageToTake = 0;
            }
            else
            {
                damageToTake -= statsContainer.Shield.statValue;
                statsContainer.Shield.statValue = 0;
            }

            if (damageToTake > 0)
            {
                statsContainer.CurrentHealth.statValue -= damageToTake;
                CameraShake.Shake(0.125f, 0.1f);
                UpdateCharacterUI();

                // ******
                //Renderer.Flip(true);

                var isDead = GetStat(Stats.CurrentHealth).statValue <= 0;

                StartCoroutine(HitMotion(isDead));

                if (isDead)
                {
                    isAlive = false;
                    StartCoroutine(Die());
                    UnlinkCharacterToTile();

                    if (isActive)
                        endTurn.Raise();
                }
            }


            // ******
            IEnumerator HitMotion(bool isDead)
            {
                Renderer.Flip(false);

                Renderer.SetMotion("HIT");

                if (!isDead)
                {
                    "¾ÈÀÜ´Ù".Log();

                    yield return new WaitForSeconds(0.5f);

                    Renderer.Flip(true);
                }
                //else
                //{
                //    isAlive = false;
                //    yield return StartCoroutine(Die());
                //    UnlinkCharacterToTile();

                //    if (isActive)
                //        endTurn.Raise();

                //    Renderer.SpriteRenderer.color = new Color(0.35f, 0.35f, 0.35f, 1);
                //}

                yield return null;
            }
        }

        public IEnumerator AttackMotion()
        {
            Renderer.Flip(false);

            Renderer.SetMotion("ATTACK");

            yield return new WaitForSeconds(0.5f);

            Renderer.Flip(true);

            yield return null;
        }

        public void HealEntity(int value)
        {
            statsContainer.CurrentHealth.statValue += value;
            UpdateCharacterUI();
        }

        //basic example if using a defencive stat
        private int CalculateDamage(int damage)
        {
            float percentage = (((float)GetStat(Stats.Endurance).statValue / (float)damage) * 100) / 2;

            percentage = percentage > 75 ? 75 : percentage;

            int damageToTake = damage - Mathf.CeilToInt((float)(percentage / 100f) * (float)damage);
            return damageToTake;

            //******
            // ¾ê³× µ¥¹ÌÁö °è»ê ¹¹ ÀÌ¸® ÇÔ?
            //var percentage = GetPercentage(GetStat(Stats.Endurance).statValue, damage);

            //percentage = percentage > 75 ? 75 : percentage;

            //return damage - Mathf.CeilToInt(GetEndurance(GetStat(Stats.Endurance).statValue, damage));

            //******
            //float GetPercentage(float endurance, float damage)
            //{
            //    return endurance / damage * 50.0f;
            //}    
            
            //float ReduceDamage(float percentage, float damage)
            //{
            //    return percentage * damage / 100.0f;
            //}

            //float GetEndurance(float endurance, float damage)
            //{
            //    return endurance * 0.5f > damage * 0.75f ? damage * 0.75f : endurance * 0.5f;
            //}
        }

        //Get a perticular stat object. 
        public Stat GetStat(Stats statName)
        {
            switch (statName)
            {
                case Stats.Health:
                    return statsContainer.Health;
                case Stats.Mana:
                    return statsContainer.Mana;
                case Stats.Strenght:
                    return statsContainer.Strenght;
                case Stats.Endurance:
                    return statsContainer.Endurance;
                case Stats.Speed:
                    return statsContainer.Speed;
                case Stats.Intelligence:
                    return statsContainer.Intelligence;
                case Stats.MoveRange:
                    return statsContainer.MoveRange;
                case Stats.CurrentHealth:
                    return statsContainer.CurrentHealth;
                case Stats.CurrentMana:
                    return statsContainer.CurrentMana;
                case Stats.AttackRange:
                    return statsContainer.AttackRange;
                //******
                case Stats.Shield:
                    return statsContainer.Shield;
                default:
                    return statsContainer.Health;
            }
        }

        //What happens when a character dies. 
        public IEnumerator Die()
        {
            float DegreesPerSecond = 360f;
            Vector3 currentRot, targetRot = new Vector3();
            currentRot = transform.eulerAngles;
            targetRot.z = currentRot.z + 90; // calculate the new angle

            //foreach (Transform child in transform)
            //{
            //    child.gameObject.SetActive(false);
            //}

            // ******
            var canvas = transform.GetComponentInChildren<Canvas>();
            canvas.gameObject.SetActive(false);

            //healthBar.gameObject.SetActive(false);

            while (currentRot.z < targetRot.z)
            {
                currentRot.z = Mathf.MoveTowardsAngle(currentRot.z, targetRot.z, DegreesPerSecond * Time.deltaTime);
                transform.eulerAngles = currentRot;
                yield return null;
            }

            //GetComponent<SpriteRenderer>().color = new Color(0.35f, 0.35f, 0.35f, 1);

            Renderer.SpriteRenderer.color = new Color(0.35f, 0.35f, 0.35f, 1);

            // ******
            EntityDie.Raise(this.gameObject);
        }

        //Updates the characters healthbar. 
        private void UpdateCharacterUI()
        {
            healthBar.fillAmount = (float)statsContainer.CurrentHealth.statValue / (float)statsContainer.Health.statValue;
        }

        //Change characters mana
        public void UpdateMana(int value) => statsContainer.CurrentMana.statValue -= value;

        //Attach an effect to the Entity from a tile or ability. 
        public void AttachEffect(ScriptableEffect scriptableEffect)
        {
            if (scriptableEffect)
            {
                var statToEffect = GetStat(scriptableEffect.statKey);

                if (statToEffect.statMods.FindIndex(x => x.statModName == scriptableEffect.Name.ToString()) != -1)
                {
                    int modIndex = statToEffect.statMods.FindIndex(x => x.statModName == scriptableEffect.Name.ToString());
                    if(scriptableEffect.Type)
                        statToEffect.statMods[modIndex] = new StatModifier(scriptableEffect.statKey, (scriptableEffect.Value + statToEffect.statMods[modIndex].value ), scriptableEffect.Duration, scriptableEffect.Operator, scriptableEffect.Name.ToString());
                    else
                        statToEffect.statMods[modIndex] = new StatModifier(scriptableEffect.statKey, scriptableEffect.Value , scriptableEffect.Duration + statToEffect.statMods[modIndex].duration , scriptableEffect.Operator, scriptableEffect.Name.ToString());
                }
                else
                    statToEffect.statMods.Add(new StatModifier(scriptableEffect.statKey, scriptableEffect.Value, scriptableEffect.Duration, scriptableEffect.Operator, scriptableEffect.Name.ToString()));
            }
        }

        //Effects that don't have a duration can just be applied straight away. 
        public void ApplySingleEffects(Stats selectedStat)
        {
            Stat value = statsContainer.getStat(selectedStat);
            value.ApplyStatMods();
            UpdateCharacterUI();
        }

        //Apply all the currently attached effects. Happens when a new turn begins. 
        public void ApplyEffects()
        {
            var fields = typeof(CharacterStats).GetFields();

            foreach (var item in fields)
            {
                var type = item.FieldType;
                Stat value = (Stat)item.GetValue(statsContainer);

                value.ApplyStatMods();
            }

            UpdateCharacterUI();
        }

        //Gets Entities ability. 
        public AbilityContainer GetAbilityByName(string abilityName)
        {
            return abilitiesForUse.Find(x => x.ability.Name == abilityName);
        }

        public virtual void StartTurn()
        {
            ////******
            //statsContainer.Shield.statValue = 0;
        }

        public virtual void CharacterMoved()
        {

        }

        //When an Entity moves, link it to the tiles it's standing on. 
        public void LinkCharacterToTile(OverlayTile tile)
        {
            UnlinkCharacterToTile();
            tile.activeCharacter = this;
            tile.isBlocked = true;
            activeTile = tile;
        }

        //Unlink an entity from a previous tile it was standing on. 
        public void UnlinkCharacterToTile()
        {
            if (activeTile)
            {
                activeTile.activeCharacter = null;
                activeTile.isBlocked = false;
                activeTile = null;
            }
        }
    }
}
