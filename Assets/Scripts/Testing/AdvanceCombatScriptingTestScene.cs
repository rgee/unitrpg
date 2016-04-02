using System.Collections;
using System.Collections.Generic;
using Combat;
using Grid;
using Models.Fighting;
using Models.Fighting.Characters;
using Models.Fighting.Effects;
using Models.Fighting.Execution;
using Models.Fighting.Skills;
using UnityEngine;
using Advance = Models.Fighting.Effects.Advance;

namespace Assets.Testing {
    public class AdvanceCombatScriptingTestScene : MonoBehaviour {
        private FightPhase _firstPhase;
        private FightPhaseAnimator _phaseAnimator;
        private UnitManager _unitManager;

        void Awake() {
            var liatStats = new CharacterBuilder()
                .Id("Liat")
                .Name("Liat")
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Move(6)
                    .Skill(99)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(70)
                .Build())
                .Stats(new StatsBuilder()
                    .ParryChance(0)
                .Build())
                .Weapons("Campaign Backblade", "Slim Recurve")
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

            var liat = new BaseCombatant(liatStats, ArmyType.Friendly);
            var gatsu = new BaseCombatant(gatsuStats, ArmyType.Friendly);


            _firstPhase = new FightPhase {
                Initiator = liat,
                Receiver = gatsu,
                Response = DefenderResponse.GetHit,
                Skill = SkillType.Advance,
                Effects = new SkillEffects (new List<IEffect> { new Advance(liat)})
            };

            _phaseAnimator = GetComponent<FightPhaseAnimator>();
            _unitManager = CombatObjects.GetUnitManager();
            StartCoroutine(RunTest());
        }

        IEnumerator RunTest() {
            yield return new WaitForSeconds(2);

            var liat = _unitManager.GetUnitByName("Liat");
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