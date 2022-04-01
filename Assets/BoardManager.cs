using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


public class BoardManager : MonoBehaviour
{
    public const int DOTS_DISTANCE = 2;

    [SerializeField]
    private Transform dotPrefab = null;
    [SerializeField]
    private BoardLine linePrefab = null;

    private GameObject boardHolder = null;

    public ReadOnlyCollection<Box> GenerateBoard(Vector2 size)
    {
        if (size.x <= 1 && size.y <= 1)
            throw new Exception("La dimension del tablero no puede ser inferior o igual a 1");

        var dots = new Transform[(int)size.y, (int)size.x];

        boardHolder = new GameObject("Board");
        for (int row = (int)size.y - 1; row >= 0; row--)
        {
            for (int colum = 0; colum < size.x; colum++)
            {
                Transform dot = Instantiate(dotPrefab, Vector2.up * row * DOTS_DISTANCE + Vector2.right * colum * DOTS_DISTANCE, Quaternion.identity, boardHolder.transform);
                dots[row, colum] = dot;
            }
        }
        return GenerateBoxes(dots);

    }

    private ReadOnlyCollection<Box> GenerateBoxes(Transform[,] dots)
    {
        Vector2 size = new Vector2(dots.GetLength(1), dots.GetLength(0));
        int boxesCount = ((int)size.x - 1) * ((int)size.y - 1);
        print(size);

        var lines = new List<BoardLine>();
        var boxes = new List<Box>(boxesCount);

        //TopHorizontal
        for (int box = 0; box < boxesCount; box++)
        {
            int row = ((int)size.y - 1) - (box / ((int)size.x - 1));
            int colum = (box % ((int)size.x - 1));
            print($"r: {row}, c: {colum}");

            //TopLeft
            Transform dotTL = dots[row, colum];
            Transform dotTR = dots[row, colum + 1];
            Transform dotBL = dots[row - 1, colum];
            Transform dotBR = dots[row - 1, colum + 1];

            BoardLine lineTH = lines.Find(l => l.HasSameDots(dotTL, dotTR));
            if (lineTH == null)
            {
                lineTH = Instantiate(linePrefab, dotTL.position, Quaternion.identity, boardHolder.transform);
                lineTH.SetDots(dotTL, dotTR);
                lines.Add(lineTH);
            }

            BoardLine lineRV = lines.Find(l => l.HasSameDots(dotTR, dotBR));
            if (lineRV == null)
            {
                lineRV = Instantiate(linePrefab, dotTR.position, Quaternion.identity, boardHolder.transform);
                lineRV.SetDots(dotTR, dotBR);
                lines.Add(lineRV);
            }

            BoardLine lineBH = lines.Find(l => l.HasSameDots(dotBR, dotBL));
            if (lineBH == null)
            {
                lineBH = Instantiate(linePrefab, dotBR.position, Quaternion.identity, boardHolder.transform);
                lineBH.SetDots(dotBR, dotBL);
                lines.Add(lineBH);
            }

            BoardLine lineLV = lines.Find(l => l.HasSameDots(dotBL, dotTL));
            if (lineLV == null)
            {
                lineLV = Instantiate(linePrefab, dotBL.position, Quaternion.identity, boardHolder.transform);
                lineLV.SetDots(dotBL, dotTL);
                lines.Add(lineLV);
            }

            Box b = new Box(lineTH, lineRV, lineBH, lineLV);

            boxes.Add(b);
        }

        return boxes.AsReadOnly();
    }

    //public IEnumerator IGenerateBoard(Vector2 size)
    //{
    //    if (size.x <= 1 && size.y <= 1)
    //        throw new Exception("La dimension del tablero no puede ser inferior o igual a 1");

    //    boardHolder = new GameObject("Board");
    //    for (int i = (int)size.y - 1; i >= 0; i--)
    //    {
    //        for (int j = 0; j < size.x; j++)
    //        {
    //            Transform dot = Instantiate(dotPrefab, Vector2.up * i * DOTS_DISTANCE + Vector2.right * j * DOTS_DISTANCE, Quaternion.identity, boardHolder.transform);
    //            yield return new WaitForSeconds(0.25f);
    //        }
    //    }
    //}
}
