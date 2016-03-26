using System.Collections;
using Combat;
using Models.Fighting;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Skills;
using UnityEngine;

namespace Assets.Testing {
    public class CombatScriptingTestScene : MonoBehaviour {
        private FightPhase _firstPhase;
        private FightPhaseAnimator _phaseAnimator;

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
                .Name("Gatsu")
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
            _firstPhase.Response = DefenderResponse.GetHit;
            _firstPhase.Skill = SkillType.Melee;

            _phaseAnimator = GetComponent<FightPhaseAnimator>();
            StartCoroutine(RunTest());
        }

        IEnumerator RunTest() {
            yield return new WaitForSeconds(2);
            yield return StartCoroutine(_phaseAnimator.Animate(_firstPhase));
        }
    }
}