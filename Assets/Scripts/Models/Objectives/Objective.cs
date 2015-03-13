using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Objective : MonoBehaviour {
    public abstract bool IsComplete();
    public abstract bool IsFailed();
}