using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FightPhaseExecutor {
    public event EventHandler OnTargetDied;
    public event EventHandler OnComplete;

    private Grid.Unit Attacker;
    private Grid.Unit Defender;
    private List<Hit> Hits;
    private int HitIndex = 0;

    public FightPhaseExecutor(Grid.Unit attacker, Grid.Unit defender, List<Hit> hits) {
        this.Attacker = attacker;
        this.Defender = defender;
        this.Hits = hits;
    }

    public void Run() {
        Attacker.OnAttackComplete += OnHit;
        Attack();
    }

    private void OnHit() { 
        Hit hit = Hits[HitIndex];
        Defender.TakeDamage(hit.Damage);
        HitIndex++;

        if (!Defender.IsAlive() && OnTargetDied != null) {
            OnTargetDied(this, EventArgs.Empty);
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
        Hit hit = Hits[HitIndex];
        bool killingBlow = !hit.Missed && hit.Damage > Defender.model.Health;
        if (hit.Missed) {
            Defender.Dodge();
        }

        Attacker.Attack(Defender, hit, killingBlow);
    }
}
