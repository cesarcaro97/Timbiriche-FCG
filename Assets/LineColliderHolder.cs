using UnityEngine;

public class LineColliderHolder : MonoBehaviour
{
    public BoardLine ParentLine { get; set; }


    private void OnMouseDown()
    {
        OnClick();
    }

    public void OnClick()
    {
        ParentLine.OnClick();
    }
}
