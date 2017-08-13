using UnityEngine;

[RequireComponent(typeof(tk2dUILayout))]
public class CenterPortraitAligner : MonoBehaviour, IPortraitAligner {

    public virtual void Align(GameObject portrait, Vector3 scale) {

        Utils.GameObjectUtils.SetLayerRecursively(portrait, transform.gameObject.layer);

        portrait.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        
        var layout = GetComponent<tk2dUILayout>();
        var height = (layout.GetMinBounds() - layout.GetMaxBounds()).y;
        var halfLayoutWidth = (layout.GetMinBounds() - layout.GetMaxBounds()).x/2;
        var bounds = GameObjectUtils.CalculateBounds(portrait);
        var halfPortraitWidth = (bounds.max - bounds.min).x/2;

        var viewBottomCenter = -halfLayoutWidth;
        var portraitBottomCenter = Mathf.Sign(scale.x) < 0 ? -halfPortraitWidth : halfPortraitWidth;

        portrait.transform.SetParent(transform);
        portrait.transform.localPosition = new Vector3(0, height, 0);
    }
}
