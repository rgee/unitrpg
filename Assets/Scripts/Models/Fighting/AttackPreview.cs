namespace Models.Fighting {
   public class AttackPreview {
      public int Damage { get; private set; } 
      public int AttackCount { get; private set; }
      public int HitChance { get; private set; }
      public int CritChance { get; private set; }
      public int GlanceChance { get; private set; }
      
      public AttackPreview(int damage, int attackCount, int hitChance, int critChance, int glanceChance) {
         Damage = damage;
         AttackCount = attackCount;
         HitChance = hitChance;
         CritChance = critChance;
         GlanceChance = glanceChance;
      }
   }
}