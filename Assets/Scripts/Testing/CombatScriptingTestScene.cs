using System.Collections;
using Combat;
using Grid;
using Models.Fighting;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Skills;
using UnityEngine;

namespace Assets.Testing {
    public class CombatScriptingTestScene : MonoBehaviour {
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


            _firstPhase = new FightPhase();
            _firstPhase.Initiator = liat;
            _firstPhase.Receiver = gatsu;
            _firstPhase.Response = DefenderResponse.Dodge;
            _firstPhase.Skill = SkillType.Melee;

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

            yield return StartCoroutine(_phaseAnimator.Animate(_firstPhase));
        }
    }
}