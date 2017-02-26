using UnityEngine;

namespace Models.Fighting.Battle {
    /// <summary>
    /// Information about the location of a point of interest on the map.
    /// </summary>
    public interface IPointOfInterest {
        Vector3 FocalPoint { get; }
        
        /// <summary>
        /// How far in world units the camera should be from
        /// the focal point before it must move.
        /// </summary>
        float Tolerance { get; }
    }
}