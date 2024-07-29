using UnityEngine;
using UnityEngine.UI;

public class RadialLayout : LayoutGroup
{
    [SerializeField, Tooltip("The distance from the center to each child element.")]
    float fDistance;

    [SerializeField, Range(0f, 360f), Tooltip("The minimum angle for the radial layout, defining the starting point of the layout arc.")]
    float MinAngle;

    [SerializeField, Range(0f, 360f), Tooltip("The maximum angle for the radial layout, defining the ending point of the layout arc.")]
    float MaxAngle;

    [SerializeField, Range(0f, 360f), Tooltip("The initial angle where the first child element will be placed.")]
    float StartAngle;

    bool isInitialized = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        isInitialized = true;

        if (isInitialized)
        {
            CalculateRadial();
        }
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }

    public override void CalculateLayoutInputVertical()
    {
        CalculateRadial();
    }

    public override void CalculateLayoutInputHorizontal()
    {
        CalculateRadial();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();

        if (isInitialized)
        {
            CalculateRadial();
        }
    }
#endif

    void CalculateRadial()
    {
        m_Tracker.Clear();

        if (transform.childCount == 0)
            return;

        float fOffsetAngle = ((MaxAngle - MinAngle)) / (transform.childCount - 1);

        float fAngle = StartAngle;
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = (RectTransform)transform.GetChild(i);
            if (child != null)
            {
                //Adding the elements to the tracker stops the user from modifiying their positions via the editor.
                m_Tracker.Add(this, child,
                DrivenTransformProperties.Anchors |
                DrivenTransformProperties.AnchoredPosition |
                DrivenTransformProperties.Pivot);
                Vector3 vPos = new (Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0);
                child.localPosition = vPos * fDistance;

                //Force objects to be center aligned, this can be changed however I'd suggest you keep all of the objects with the same anchor points.
                child.anchorMin = child.anchorMax = child.pivot = new Vector2(0.5f, 0.5f);
                fAngle += fOffsetAngle;
            }
        }
    }
}
