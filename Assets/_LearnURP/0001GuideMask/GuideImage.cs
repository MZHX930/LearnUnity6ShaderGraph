using UnityEngine;
using UnityEngine.UI;

public class GuideImage : Image
{
    public Vector2 GuideRectMin = new Vector2(0.4f, 0.4f);
    public Vector2 GuideRectMax = new Vector2(0.6f, 0.6f);

    public bool IsPlayAnim = false;
    public float AnimTime = 0.5f;
    public float ElapsedTime = 0f;


    [ContextMenu("PlayAnim")]
    public void PlayAnim()
    {
        IsPlayAnim = true;
        ElapsedTime = 0f;
    }

    private void Update()
    {
        if (IsPlayAnim)
        {
            ElapsedTime += Time.deltaTime;
            material.SetFloat("_RectangleMin_x", Mathf.Lerp(0, GuideRectMin.x, ElapsedTime / AnimTime));
            material.SetFloat("_RectangleMin_y", Mathf.Lerp(0, GuideRectMin.y, ElapsedTime / AnimTime));
            material.SetFloat("_RectangleMax_x", Mathf.Lerp(1, GuideRectMax.x, ElapsedTime / AnimTime));
            material.SetFloat("_RectangleMax_y", Mathf.Lerp(1, GuideRectMax.y, ElapsedTime / AnimTime));

            if (ElapsedTime >= AnimTime)
            {
                IsPlayAnim = false;
            }
        }
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (IsPlayAnim)
        {
            return true;
        }

        if (!raycastTarget)
        {
            return false;
        }

        RectTransform rectTransform = transform as RectTransform;
        if (rectTransform == null)
        {
            return true;
        }

        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            screenPoint,
            eventCamera,
            out localPoint))
        {
            return false;
        }
        Rect rect = rectTransform.rect;
        if (rect.width <= 0f || rect.height <= 0f)
        {
            return true;
        }

        float u = (localPoint.x - rect.x) / rect.width;
        float v = (localPoint.y - rect.y) / rect.height;

        float minX = Mathf.Min(GuideRectMin.x, GuideRectMax.x);
        float maxX = Mathf.Max(GuideRectMin.x, GuideRectMax.x);
        float minY = Mathf.Min(GuideRectMin.y, GuideRectMax.y);
        float maxY = Mathf.Max(GuideRectMin.y, GuideRectMax.y);

        bool inGuideArea = u >= minX && u <= maxX && v >= minY && v <= maxY;
        return !inGuideArea;
    }
}
