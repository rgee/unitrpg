using Models.Combat.Inventory;
using strange.extensions.signal.impl;
using UnityEngine;

public static class CombatEventBus {
    public static Signal<Hit> Hits = new Signal<Hit>();
    public static Signal<Grid.Unit> Deaths = new Signal<Grid.Unit>();
    public static Signal<Models.Combat.Unit> ModelDeaths = new Signal<Models.Combat.Unit>();
    public static Signal<Grid.Unit, Vector2> Moves = new Signal<Grid.Unit, Vector2>();
    public static Signal<Models.Combat.Unit, Vector2> MoveSignal = new Signal<Models.Combat.Unit, Vector2>();
    public static Signal<Models.Combat.Unit> DeathSignal = new Signal<Models.Combat.Unit>();

    public static Signal HealthBarToggles = new Signal();
    public static Signal Pauses = new Signal();
}