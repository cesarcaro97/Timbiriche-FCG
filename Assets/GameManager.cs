using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Sprite defaultPlayer1Icon = null;
    [SerializeField]
    private Sprite defaultPlayer2Icon = null;
    [SerializeField]
    private Vector2 boardSize;
    [SerializeField]
    private BoardManager boardManager = null;

    public static GameManager Instance { get; set; }

    private ReadOnlyCollection<Box> Boxes { get; set; }
    public Player PlayerInTurn { get; private set; }

    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }

    public bool CanPlay { get; private set; }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        //StartGame(4, 4, new Player("Player 1", defaultPlayer1Icon, Color.black), new Player("Player 2", defaultPlayer2Icon, Color.black));
    }

    public void StartGame(int sizeX, int sizeY)
    {
        boardSize = new Vector2(sizeX, sizeY);

        Player1 = new Player("Player 1", defaultPlayer1Icon, Color.black);
        Player2 = new Player("Player 2", defaultPlayer2Icon, Color.black);
        GameUIManager.Instance.SetPlayers(Player1, Player2);

        PlayerInTurn = Random.Range(0, 2) == 0 ? Player1 : Player2;
        GameUIManager.Instance.SetPlayerInTurn(PlayerInTurn);

        GenerateBoard();
        Camera.main.GetComponent<CameraController>().SetReferencePosition(sizeX, sizeY);
        CanPlay = true;
    }


    private void GenerateBoard()
    {
        Boxes = boardManager.GenerateBoard(boardSize);
        //StartCoroutine(boardManager.IGenerateBoard(boardSize));
    }


    public void OnPlayerPlay(BoardLine selectedLine)
    {
        if (!CanPlay) return;

        if (selectedLine.WasClicked) return;

        selectedLine.WasClicked = true;

        selectedLine.LineRenderer.material.color = GameManager.Instance.PlayerInTurn.Color;

        var boxes = Boxes.Where(b => b.Lines.Contains(selectedLine)).ToArray();
        print(boxes.Length);

        var boxesCompleted = Boxes.Where(b => b.Lines.Contains(selectedLine)).Where(b => b.Lines.All(l => l.WasClicked)).ToArray();

        if(boxesCompleted.Length > 0)
        {
            print($"{PlayerInTurn.Name} completo {boxesCompleted.Length} boxes");
            PlayerInTurn.Score += boxesCompleted.Length;
            GameUIManager.Instance.UpdatePlayerInTurnPlayerScore();

            foreach (var box in boxesCompleted)
            {
                box.Completed = true;
                int boxRow = ((int)boardSize.y - 1) - (Boxes.IndexOf(box) / ((int)boardSize.x - 1));
                int boxColum = (Boxes.IndexOf(box) % ((int)boardSize.x - 1));

                Vector3 pos = new Vector3(boxColum * BoardManager.DOTS_DISTANCE + ((float)BoardManager.DOTS_DISTANCE / 2), boxRow * BoardManager.DOTS_DISTANCE - ((float)BoardManager.DOTS_DISTANCE / 2), 2);
                Vector3 scale = new Vector3(BoardManager.DOTS_DISTANCE, BoardManager.DOTS_DISTANCE, 1);

                var icGo = new GameObject("Box Icon").AddComponent<SpriteRenderer>();
                icGo.sprite = PlayerInTurn.Icon;
                icGo.transform.position = pos;
                icGo.transform.localScale = scale;
            }

            CheckForGameOver();
        }
        else
        {
            ChangePlayerTurn();
        }

    }

    private void CheckForGameOver()
    {
        Player winner = null;
        int boxesLeft = Boxes.Where(b => !b.Completed).Count();

        if(boxesLeft > 0)
        {
            if (Player1.Score > Player2.Score + boxesLeft)
            {
                winner = Player1;
            }
            else if(Player2.Score > Player1.Score + boxesLeft)
            {
                winner = Player2;
            }
        }
        else
        {
            if(Player1.Score > Player2.Score)
            {
                winner = Player1;
            }
            else if(Player2.Score > Player1.Score)
            {
                winner = Player2;
            }
        }

        if (winner != null || (boxesLeft == 0 && winner == null))
        {
            CanPlay = false;
            GameUIManager.Instance.ShowGameOver(winner);
        }
    }

    private void ChangePlayerTurn()
    {
        PlayerInTurn = PlayerInTurn == Player1 ? Player2 : Player1;
        GameUIManager.Instance.SetPlayerInTurn(PlayerInTurn);
    }
}
