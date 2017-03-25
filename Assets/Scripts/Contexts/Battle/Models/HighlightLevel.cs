using System;

namespace Contexts.Battle.Models
{
    public enum HighlightLevel {
        
        GlobalEnemyMove = 0,
        SpecificEnemyMove = 1,
        PlayerMove = 2,
        PlayerAttack = 3,
        PlayerHover = 4,
        PlayerInteract = 10000 
    }
}