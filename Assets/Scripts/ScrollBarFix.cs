using UnityEngine.UI;
public class ScrollBarFix : ScrollRect
{
    override protected void LateUpdate()
    {
        base.LateUpdate();
        if (this.verticalScrollbar)
        {
            this.verticalScrollbar.size = 0.3f;
        }
    }

    override public void Rebuild(CanvasUpdate executing)
    {
        base.Rebuild(executing);
        if (this.verticalScrollbar)
        {
            this.verticalScrollbar.size = 0.3f;
        }
    }
}