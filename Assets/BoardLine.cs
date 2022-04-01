using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BoardLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer = null;

    public LineRenderer LineRenderer => lineRenderer;

    public Transform DotA { get; private set; }
    public Transform DotB { get; private set; }

    public bool WasClicked { get; set; }


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetDots(Transform a, Transform b)
    {
        DotA = a;
        DotB = b;

        var positions = new Vector3[]
        {
            a.position + Vector3.forward,
            b.position + Vector3.forward
        };
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
        AddCollider(1);
    }


    public bool HasSameDots(Transform a, Transform b)
    {
        return (DotA == a || DotA == b) && (DotB == b || DotB == a);
    }

    public void AddCollider(int part)
    {
        try
        {
            var start = lineRenderer.GetPosition(0);
            var end = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            var a = (lineRenderer.positionCount - 1) / part;
            for (int i = 1; i <= part; i++)
            {
                if (i == 1)
                    AddColliderToLine(start, lineRenderer.GetPosition(Mathf.CeilToInt(a * 1)));
                else if (i == part)
                    AddColliderToLine(lineRenderer.GetPosition(Mathf.CeilToInt(a * (i - 1))), end);
                else
                    AddColliderToLine(lineRenderer.GetPosition(Mathf.CeilToInt(a * (i - 1))), lineRenderer.GetPosition(Mathf.CeilToInt(a * i)));
            }
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    private void AddColliderToLine(Vector3 start, Vector3 end)
    {
        var startPos = start;
        var endPos = end;
        BoxCollider2D col = new GameObject("Collider").AddComponent<BoxCollider2D>();
        col.transform.parent = lineRenderer.transform;
        float lineLength = Vector3.Distance(startPos, endPos);
        col.size = new Vector3(lineLength, 0.175f, 0.25f);
        Vector3 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint;
        float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));
        if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        {
            angle *= -1;
        }
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        col.transform.Rotate(0, 0, angle);

        var lh = col.gameObject.AddComponent<LineColliderHolder>();
        lh.ParentLine = this;
    }

    public void OnClick()
    {
        GameManager.Instance.OnPlayerPlay(this);
    }
}