using System;
using System.Collections.Generic;

public class FightPhaseExecutor {
    private readonly Grid.Unit Attacker;
    private readonly Grid.Unit Defender;
    private readonly Hit _hit;

    public FightPhaseExecutor(Grid.Unit attacker, Grid.Unit defender, Hit hit) {
        Attacker = attacker;
        Defender = defender;
        _hit = hit;
    }

    ~FightPhaseExecutor() {
        Attacker.OnAttackComplete -= End;
        Defender.OnDodgeComplete -= End;
        Attacker.OnAttackStart -= DefenderDodge;
        Attacker.OnAttackComplete -= OnHit;
    }

    public event EventHandler OnTargetDied;
    public event EventHandler OnComplete;

    public void Run() {
        Attacker.OnAttackComplete += OnHit;
        Attack();
    }

    private void OnHit() {
        Defender.TakeDamage(_hit.Damage);

        if (!Defender.IsAlive() && OnTargetDied != null) {
            OnTargetDied(this, EventArgs.Empty);
            End();
        } else if (Defender.IsDodging) {
            Defender.OnDodgeComplete += End;
        } else {
            End();
        }
    }

    private void End() {
        if (OnComplete != null) {
            OnComplete(this, EventArgs.Empty);
        }
        Attacker.OnAttackComplete -= OnHit;
        Defender.OnDodgeComplete -= End;
    }

    private void Attack() {
        var killingBlow = !_hit.Missed && _hit.Damage > Defender.model.Health;
        if (_hit.Missed) {
            Attacker.OnAttackStart += DefenderDodge;
        }

        Attacker.Attack(Defender, _hit, killingBlow);
    }

    private void DefenderDodge() {
        Defender.Dodge();
        Attacker.OnAttackStart -= DefenderDodge;
    }
}