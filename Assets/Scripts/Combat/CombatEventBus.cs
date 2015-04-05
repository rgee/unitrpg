using strange.extensions.signal.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CombatEventBus {
    public static Signal<Hit> Hits = new Signal<Hit>();
    public static Signal<Grid.Unit> Deaths = new Signal<Grid.Unit>();
    public static Signal<Grid.Unit, Vector2> Moves = new Signal<Grid.Unit, Vector2>();
}