using System.Collections;
using System.Collections.Generic;
using Combat;
using Grid;
using Models.Fighting;
using Models.Fighting.Characters;
using Models.Fighting.Effects;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
using Models.Fighting.Skills;
using UnityEngine;

namespace Assets.Testing {
    public class ShoveCombatScriptingTestScene : MonoBehaviour {
        private FightPhase _firstPhase;
        private FightPhaseAnimator _phaseAnimator;
        private UnitManager _unitManager;

        void Awake() {

            var janekStats = new CharacterBuilder()
                .Id("janek")
                .Name("Janek")
                .Attributes(new AttributesBuilder()
                    .Move(5)
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                    .Build())
                .Weapons("Chained Mace")
                .Build();

            var gatsuStats = new CharacterBuilder()
                .Id("Gatsu")
                .Name("Soldier")
                .Attributes(new AttributesBuilder()
                    .Health(1)
                    .Skill(12)
                    .Move(6)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Stats(new StatsBuilder()
                    .ParryChance(0)
                .Build())
                .Weapons("Chained Mace")
                .Build();

            var janek = new BaseCombatant(janekStats, ArmyType.Friendly);
            var gatsu = new BaseCombatant(gatsuStats, ArmyType.Enemy);

            janek.Position = new Vector2(1, 0);
            gatsu.Position = new Vector2(2, 0);

            var map = new Map();

            _firstPhase = new FightPhase {
                Initiator = janek,
                Receiver = gatsu,
                Response = DefenderResponse.GetHit,
                Skill = SkillType.Knockback,
                Effects = new SkillEffects (new List<IEffect> { new Shove(MathUtils.CardinalDirection.E, map) })
            };

            _phaseAnimator = GetComponent<FightPhaseAnimator>();
            _unitManager = CombatObjects.GetUnitManager();
            StartCoroutine(RunTest());
        }

        IEnumerator RunTest() {
            yield return new WaitForSeconds(2);

            var liat = _unitManager.GetUnitByName("Janek");
            liat.InCombat = true;
            liat.Facing = MathUtils.CardinalDirection.E;

            var gatsu = _unitManager.GetUnitByName("Soldier");
            gatsu.InCombat = true;
            gatsu.Facing = MathUtils.CardinalDirection.W;

            yield return new WaitForSeconds(1);
            yield return StartCoroutine(_phaseAnimator.Animate(_firstPhase));
        }
    }
}