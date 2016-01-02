using System.Collections;
using UnityEngine;

namespace Assets.Models.Combat {
    public interface IMovementEventHandler {
        IEnumerator HandleMovement(Grid.Unit unit, Vector2 destination);
    }
}