using System;
using Models.Fighting.Execution;
using Models.Fighting.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CombatForecast {
    public class CombatForecast : MonoBehaviour {
        private enum CombatantParam {
            Attacker,
            Defender
        };

        public void SetForecast(FightForecast forecast) {
            var attackerName = transform.FindChild("Forecast/Attacker Name").GetComponent<Text>();
            attackerName.text = forecast.AttackerForecast.Attacker.Name.ToUpper();

            var defenderName = transform.FindChild("Forecast/Defender Name").GetComponent<Text>();
            defenderName.text = forecast.AttackerForecast.Defender.Name.ToUpper();

            PopulateChances(forecast.AttackerForecast.Chances, CombatantParam.Attacker);
            PopulateChances(forecast.DefenderForecast.Chances, CombatantParam.Defender);

            var attackerHealth = forecast.AttackerForecast.Attacker.Health;
            var defenderHealth = forecast.AttackerForecast.Defender.Health;

            var attackerHealthText = GetAmount("Health", CombatantParam.Attacker).GetComponent<Text>();
            attackerHealthText.text = attackerHealth.ToString();

            var defenderHealthText = GetAmount("Health", CombatantParam.Defender).GetComponent<Text>();
            defenderHealthText.text = defenderHealth.ToString();
        }

        private void PopulateChances(SkillChances chances, CombatantParam param) {
            var hitChance = chances.HitChance;
            var glanceChance = chances.GlanceChance;
            var critChance = chances.CritChance;

            var hitAmount = GetAmount("Hit", param);
            hitAmount.GetComponent<Text>().text = hitChance.ToString();

            var glanceAmount = GetAmount("Glance", param);
            glanceAmount.GetComponent<Text>().text = glanceChance.ToString();

            var critAmount = GetAmount("Crit", param);
            critAmount.GetComponent<Text>().text = critChance.ToString();
        }

        private GameObject GetAmount(string objectName, CombatantParam param) {
            
            var parent = param == CombatantParam.Attacker
                ? transform.FindChild("Forecast/Attacker Parameters")
                : transform.FindChild("Forecast/Defender Parameters");

            return parent.FindChild(objectName + " Amount").gameObject;
        }
    }
}