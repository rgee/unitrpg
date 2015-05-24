using System;
using System.Collections.Generic;

public class FightPhaseExecutor {
    private readonly Grid.Unit Attacker;
    private readonly Grid.Unit Defender;
    private readonly List<Hit> Hits;
    private int HitIndex;

    public FightPhaseExecutor(Grid.Unit attacker, Grid.Unit defender, List<Hit> hits) {
        Attacker = attacker;
        Defender = defender;
        Hits = hits;
    }

    public event EventHandler OnTargetDied;
    public event EventHandler OnComplete;

    public void Run() {
        Attacker.OnAttackComplete += OnHit;
        Attack();
    }

    private void OnHit() {
        var hit = Hits[HitIndex];
        Defender.TakeDamage(hit.Damage);
        HitIndex++;

        if (!Defender.IsAlive() && OnTargetDied != null) {
            OnTargetDied(this, EventArgs.Empty);
            Attacker.OnAttackComplete -= OnHit;
        } else if (HitIndex >= Hits.Count) {
            if (OnComplete != null) {
                OnComplete(this, EventArgs.Empty);
            }
            Attacker.OnAttackComplete -= OnHit;
        } else {
            Attack();
        }
    }

    private void Attack() {
        var hit = Hits[HitIndex];
        var killingBlow = !hit.Missed && hit.Damage > Defender.model.Health;
        if (hit.Missed) {
            Defender.Dodge();
        }

        Attacker.Attack(Defender, hit, killingBlow);
    }
}