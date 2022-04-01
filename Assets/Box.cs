using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Box
{
    private List<BoardLine> lines = null;
    public ReadOnlyCollection<BoardLine> Lines => lines.AsReadOnly();

    public bool Completed { get; set; }

    public Box(BoardLine tH, BoardLine rV, BoardLine bH, BoardLine lV)
    {
        lines = new List<BoardLine>() { tH, rV, bH, lV };
    }
}
