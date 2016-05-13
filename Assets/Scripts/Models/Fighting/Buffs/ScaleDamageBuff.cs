using System;
using Models.Fighting.Effects;
using UnityEngine;

namespace Models.Fighting.Buffs {
    public class ScaleDamageBuff : AbstractBuff {
        private readonly float _scale;

        public ScaleDamageBuff(float scale, string name) : base(name) {
            _scale = scale;
        }

        public override IEffect Modify(IEffect effect) {
            var damage = effect as Damage;
            if (damage != null) {
                var amount = damage.Amount;
                var newAmount = amount * _scale;
                return new Damage(Mathf.FloorToInt(newAmount));
            }

            return base.Modify(effect);
        }
    }
}