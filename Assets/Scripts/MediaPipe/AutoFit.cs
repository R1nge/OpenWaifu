using UnityEngine;

public class AutoFit : MonoBehaviour
{
    [System.Serializable]
    public enum FitMode
    {
        Expand,
        Shrink,
        FitWidth,
        FitHeight
    }

    [SerializeField] private FitMode fitMode;
    private Rect _parent;
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _parent = transform.parent.GetComponent<RectTransform>().rect;
    }

    private void LateUpdate()
    {
        if (_rect.rect.width == 0 || _rect.rect.height == 0)
        {
            return;
        }

        var (width, height) = GetBoundingBoxSize(_rect);

        var ratio = _parent.width / width;
        var h = height * ratio;

        if (fitMode == FitMode.FitWidth || (fitMode == FitMode.Expand && h >= _parent.height) ||
            (fitMode == FitMode.Shrink && h <= _parent.height))
        {
            _rect.offsetMin *= ratio;
            _rect.offsetMax *= ratio;
            return;
        }

        ratio = _parent.height / height;

        _rect.offsetMin *= ratio;
        _rect.offsetMax *= ratio;
    }

    private (float, float) GetBoundingBoxSize(RectTransform rectTransform)
    {
        var rect = rectTransform.rect;
        var center = rect.center;
        var topLeftRel = new Vector2(rect.xMin - center.x, rect.yMin - center.y);
        var topRightRel = new Vector2(rect.xMax - center.x, rect.yMin - center.y);
        var rotatedTopLeftRel = rectTransform.rotation * topLeftRel;
        var rotatedTopRightRel = rectTransform.rotation * topRightRel;
        var wMax = Mathf.Max(Mathf.Abs(rotatedTopLeftRel.x), Mathf.Abs(rotatedTopRightRel.x));
        var hMax = Mathf.Max(Mathf.Abs(rotatedTopLeftRel.y), Mathf.Abs(rotatedTopRightRel.y));
        return (2 * wMax, 2 * hMax);
    }
}