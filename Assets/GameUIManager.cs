using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [SerializeField]
    private GameObject startPanel = null;
    [SerializeField]
    private Dropdown sizeXDropdown = null;
    [SerializeField]
    private Dropdown sizeYDropdown = null;

    [SerializeField]
    private GameObject player1InGameInfo = null;
    [SerializeField]
    private Image player1IconImage = null;
    [SerializeField]
    private Text player1NameText = null;
    [SerializeField]
    private Text player1ScoreText = null;

    [SerializeField]
    private GameObject player2InGameInfo = null;
    [SerializeField]
    private Image player2IconImage = null;
    [SerializeField]
    private Text player2NameText = null;
    [SerializeField]
    private Text player2ScoreText = null;

    [SerializeField]
    private GameObject playerInTurnIndicator = null;
    [SerializeField]
    private Text playerInTurnText = null;
    [SerializeField]
    private Image playerInTurnIconImage = null;

    [SerializeField]
    private GameObject gameOverPanel = null;
    [SerializeField]
    private Text gameOverText = null;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }


    private void Start()
    {
        player1InGameInfo.SetActive(false);
        player2InGameInfo.SetActive(false);
        playerInTurnIndicator.SetActive(false);
        gameOverPanel.SetActive(false);
        startPanel.SetActive(true);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Restart();

    }


    public void SetPlayers(Player player1, Player player2)
    {
        player1IconImage.sprite = player1.Icon;
        player1NameText.text = player1.Name;
        player1ScoreText.text = "Score: 0";

        player2IconImage.sprite = player2.Icon;
        player2NameText.text = player2.Name;
        player2ScoreText.text = "Score: 0";
    }

    public void UpdatePlayerInTurnPlayerScore()
    {
        if(GameManager.Instance.PlayerInTurn == GameManager.Instance.Player1)
        {
            player1ScoreText.text = $"Score: {GameManager.Instance.PlayerInTurn.Score}";
        }
        else
        {
            player2ScoreText.text = $"Score: {GameManager.Instance.PlayerInTurn.Score}";
        }
    }

    public void SetPlayerInTurn(Player playerInTurn)
    {
        playerInTurnText.text = $"Turn for {playerInTurn.Name}";
        playerInTurnIconImage.sprite = playerInTurn.Icon;
    }

    public void ShowGameOver(Player winner)
    {
        if (winner == null)
        {
            //Draw
            gameOverText.text = $"It is a Draw";
        }
        else
        {
            gameOverText.text = $"Winner is {winner.Name} with a score of {winner.Score}";
        }
        gameOverPanel.SetActive(true);
    }

    public void ShowInGamePlayersInfo(bool show)
    {

    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnStartGame()
    {
        int sizeX = int.Parse(sizeXDropdown.options[sizeXDropdown.value].text);
        int sizeY = int.Parse(sizeYDropdown.options[sizeYDropdown.value].text);

        startPanel.SetActive(false);
        
        GameManager.Instance.StartGame(sizeX, sizeY);
        
        player1InGameInfo.SetActive(true);
        player2InGameInfo.SetActive(true);
        playerInTurnIndicator.SetActive(true);
    }
}
