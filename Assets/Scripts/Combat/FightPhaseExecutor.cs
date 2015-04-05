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
    private int CurrentHit = 0;

    public FightPhaseExecutor(Grid.Unit attacker, Grid.Unit defender, List<Hit> hits) {
        this.Attacker = attacker;
        this.Defender = defender;
        this.Hits = hits;
    }

    public void Run() {
        Attacker.OnAttackComplete += () => {
            Hit hit = Hits[CurrentHit];
            Defender.TakeDamage(hit.Damage);
            CurrentHit++;

            if (!Defender.IsAlive() && OnTargetDied != null) {
                OnTargetDied(this, EventArgs.Empty);
            } else if (CurrentHit >= Hits.Count) {
                if (OnComplete != null) {
                    OnComplete(this, EventArgs.Empty);
                }
            } else {
                Attack();
            }
        };

        Attack();
    }

    private void Attack() {
        Hit hit = Hits[CurrentHit];
        bool killingBlow = !hit.Missed && hit.Damage > Defender.model.Health;
        if (hit.Missed) {
            Defender.Dodge();
        }

        Attacker.Attack(Defender, hit, killingBlow);
    }
}
