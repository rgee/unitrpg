using UnityEngine;

[RequireComponent(typeof(tk2dUILayout))]
public class CenterPortraitAligner : MonoBehaviour, IPortraitAligner {

    public virtual void Align(GameObject portrait, Facing facing, Vector3 scale) {

        Utils.GameObjectUtils.SetLayerRecursively(portrait, transform.gameObject.layer);

        var xScale = facing == Facing.Left ? scale.x : -scale.x;
        portrait.transform.localScale = new Vector3(xScale, scale.y, scale.z);
        
        var layout = GetComponent<tk2dUILayout>();
        var height = (layout.GetMinBounds() - layout.GetMaxBounds()).y;
        var halfLayoutWidth = (layout.GetMinBounds() - layout.GetMaxBounds()).x/2;
        var bounds = GameObjectUtils.CalculateBounds(portrait);
        var halfPortraitWidth = (bounds.max - bounds.min).x/2;

        var viewBottomCenter = -halfLayoutWidth;
        var portraitBottomCenter = facing == Facing.Left ? -halfPortraitWidth : halfPortraitWidth;

        portrait.transform.SetParent(transform);
        portrait.transform.localPosition = new Vector3(viewBottomCenter + portraitBottomCenter, height, 0);
    }
}
