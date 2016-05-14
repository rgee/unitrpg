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
        private FightPhase _flankPhase;
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

            var maelleStats = new CharacterBuilder()
                .Id("maelle")
                .Name("Maelle")
                .Attributes(new AttributesBuilder()
                    .Move(5)
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Weapons("Shortsword")
                .Build();

            var liat = new BaseCombatant(liatStats, ArmyType.Friendly);
            var gatsu = new BaseCombatant(gatsuStats, ArmyType.Friendly);
            var maelle = new BaseCombatant(maelleStats, ArmyType.Friendly);


            _firstPhase = new FightPhase {
                Initiator = liat,
                Receiver = gatsu,
                Response = DefenderResponse.Dodge,
                Effects = new SkillEffects(),
                Skill = SkillType.Melee
            };

            _flankPhase = new FightPhase {
                Initiator = maelle,
                Receiver = gatsu,
                Response = DefenderResponse.GetHit,
                Effects = new SkillEffects(),
                Skill = SkillType.Melee
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

            var maelle = _unitManager.GetUnitByName("Maelle");
            maelle.InCombat = true;
            maelle.Facing = MathUtils.CardinalDirection.W;

            yield return StartCoroutine(_phaseAnimator.Animate(_firstPhase, liat, gatsu));
            yield return new WaitForSeconds(.7f);
            yield return StartCoroutine(_phaseAnimator.Animate(_flankPhase, maelle, gatsu));
        }
    }
}