using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CongklakController : MonoBehaviour
{
    public int[] player1Hole;
    public int[] player2Hole;


    [SerializeField] private Text[] player1HoleText;
    [SerializeField] private Text[] player2HoleText;

    [SerializeField] private Text player1ScoreText;
    [SerializeField] private Text player2ScoreText;
    private int player1Score, player2Score;

    [SerializeField] private Button[] player1HoleButton;
    [SerializeField] private Button[] player2HoleButton;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] congklakSfx;

    private int currentPlayer; // 1 for player 1, 2 for player 2
    private bool isMovingStones = false;

    void Start()
    {
        currentPlayer = 1;
        UpdateUI();
        Player1Turn();
    }

    void UpdateUI()
    {
        for (int i = 0; i < player1Hole.Length; i++)
        {
            player1HoleText[i].text = player1Hole[i].ToString();
            player2HoleText[i].text = player2Hole[i].ToString();
        }
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    void Player1Turn()
    {
        for (int i = 0; i < player1HoleButton.Length; i++)
        {
            if(player1Hole[i] == 0)
            {
                player1HoleButton[i].interactable = false;
            }
            else
            {
                player1HoleButton[i].interactable = true;
            }
            player2HoleButton[i].interactable = false;
            
        }
    }

    public void Player1Go(int id)
    {
        if (!isMovingStones)
        {
            currentPlayer = 1;            
            StartCoroutine(MoveStone(id, player1Hole, player2Hole));
        }
    }

    void Player2Turn()
    {
        for (int i = 0; i < player2HoleButton.Length; i++)
        {
            if (player2Hole[i] == 0)
            {
                player2HoleButton[i].interactable = false;
            }
            else
            {
                player2HoleButton[i].interactable = true;
            }
            player1HoleButton[i].interactable = false;
        }
    }

    public void Player2Go(int id)
    {
        if (!isMovingStones)
        {
            currentPlayer = 2;
            StartCoroutine(MoveStone(id, player1Hole, player2Hole));
        }
    }

    IEnumerator MoveStone(int startIndex, int[] hole1Array, int[] hole2Array)
    {
        isMovingStones = true;
        bool haveHalfHole = true;
        int[] combinedArray = hole1Array.Concat(hole2Array).ToArray();
        int halfHole = combinedArray.Length / 2;
        int stonesToMove = combinedArray[startIndex];
        combinedArray[startIndex] = 0;

        int currentIndex = startIndex + 1; 
        while (stonesToMove > 0)
        {
            if(currentIndex == halfHole && haveHalfHole && currentPlayer == 1)
            {
                player1Score++;
                stonesToMove--;
                haveHalfHole = false;
                if(stonesToMove == 0)
                {
                    currentPlayer = 2;
                    Player1Turn();
                    audioSource.PlayOneShot(congklakSfx[2]);
                }
            }
            else if(currentIndex >= combinedArray.Length && currentPlayer == 2)
            {
                haveHalfHole = true;
                currentIndex = 0;
                player2Score++;
                stonesToMove--;
                if (stonesToMove == 0)
                {
                    currentPlayer = 1;
                    Player2Turn();
                    audioSource.PlayOneShot(congklakSfx[2]);
                }
            }
            else if (currentIndex >= combinedArray.Length)
            {
                currentIndex = 0;
                combinedArray[currentIndex]++;
                currentIndex++;
                stonesToMove--;
            }
            else
            {
                haveHalfHole = true;
                combinedArray[currentIndex]++;
                currentIndex++;
                stonesToMove--;
            } 

            player1Hole = combinedArray.Take(halfHole).ToArray();
            player2Hole = combinedArray.Skip(halfHole).ToArray();
            audioSource.PlayOneShot(congklakSfx[0]);
            UpdateUI();            
            yield return new WaitForSeconds(1f);
        }
        currentIndex--;
        currentIndex = currentIndex < 0 ? 0 : currentIndex;
        if(combinedArray[currentIndex] == 1)
        {
            if (currentPlayer == 1 && haveHalfHole)
            {
                player1Score += combinedArray[combinedArray.Length - currentIndex - 1];
                combinedArray[combinedArray.Length - currentIndex - 1] = 0;
                audioSource.PlayOneShot(congklakSfx[1]);
            }
            else if (currentPlayer == 2)
            {
                player2Score += combinedArray[combinedArray.Length - currentIndex - 1];
                combinedArray[combinedArray.Length - currentIndex - 1] = 0;
                audioSource.PlayOneShot(congklakSfx[1]);
            }
        }

        if(currentPlayer == 1)
        {
            Player2Turn();
        }
        else if(currentPlayer == 2)
        {
            Player1Turn();
        }
        player1Hole = combinedArray.Take(halfHole).ToArray();
        player2Hole = combinedArray.Skip(halfHole).ToArray();
        CheckWinner();
        UpdateUI();
        isMovingStones = false;
    }
   

    void CheckWinner()
    {
        if(player1Score >= 49)
        {
            Debug.Log("Player 1 Win");
            StartCoroutine(EndGameState(1));
        }
        if (player2Score >= 49)
        {
            Debug.Log("Player 2 Win");
            StartCoroutine(EndGameState(1));
        }
       
    }

    [SerializeField] GameObject panelEndGame;
    [SerializeField] TextMeshProUGUI winConditionText;
    IEnumerator EndGameState(int _playerWin)
    {
        //play music win
        //set to play music Sfx

        if (_playerWin == 1)
            winConditionText.SetText("Selamat Player 1 memenangkan pertandingan !!!, Pertandingan yang hebat..");

        if (_playerWin == 2)
            winConditionText.SetText("Selamat Player 2 memenangkan pertandingan !!!, Pertandingan yang hebat..");

        //AudioManager.Singleton.SetClipFromList(0, 0, false, false);

        yield return new WaitForSeconds(2f);

        panelEndGame.SetActive(true);
        audioSource.PlayOneShot(congklakSfx[3]);
        GameManager.instance.SetTimeScale(0f);
    }
}
