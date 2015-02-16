using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * This AI brain on every turn just tries to seek out a target.
 * Can hold off until the target is within a certain range, as well.
 */
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Grid.Unit))]
public class SingleMindedFury : MonoBehaviour, AIStrategy {
    public GameObject Target;
    public int AggroRange = int.MaxValue;
    public Grid.UnitManager UnitManager;

    private Seeker Seeker;
    private Grid.Unit Unit;

    public void Awake() {
        Seeker = GetComponent<Seeker>();
        Unit = GetComponent<Grid.Unit>();
    }


    public IEnumerator act() {
        Pathfinding.Path path = null;
        Seeker.StartPath(transform.position, Target.transform.position, (p) => {
            path = p;
        });
        while (path == null) {
            yield return new WaitForEndOfFrame();
        }


        if (!path.error)
        {
            yield return StartCoroutine(Unit.MoveAlongPath(path.vectorPath));
        }
        else
        {
            yield break;
        }
    }
}