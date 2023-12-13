using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            player2HoleButton[i].interactable = false;
            player1HoleButton[i].interactable = true;
        }
    }

    public void Player1Go(int id)
    {
        if (!isMovingStones)
        {
            currentPlayer = 1;
            StartCoroutine(MoveStone(id, player1Hole, player2Hole));          
            Player2Turn();
        }
    }

    void Player2Turn()
    {
        for (int i = 0; i < player2HoleButton.Length; i++)
        {
            player1HoleButton[i].interactable = false;
            player2HoleButton[i].interactable = true;
        }
    }

    public void Player2Go(int id)
    {
        if (!isMovingStones)
        {
            currentPlayer = 2;
            StartCoroutine(MoveStone(id, player1Hole, player2Hole));
            Player1Turn();
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
            }
            else if(currentIndex >= combinedArray.Length && currentPlayer == 2)
            {
                haveHalfHole = true;
                currentIndex = 0;
                player2Score++;
                stonesToMove--;
            }
            else
            {
                combinedArray[currentIndex]++;
                currentIndex++;
                stonesToMove--;
            } 
            player1Hole = combinedArray.Take(halfHole).ToArray();
            player2Hole = combinedArray.Skip(halfHole).ToArray();
            UpdateUI();            
            yield return new WaitForSeconds(2f);
        }
        
        currentIndex--;
        if (combinedArray[currentIndex] == 1 && currentPlayer == 1)
        {
            player1Score += combinedArray[combinedArray.Length - currentIndex - 1];
            combinedArray[combinedArray.Length - currentIndex - 1] = 0;
            print(combinedArray.Length - currentIndex);
        }
        else if (combinedArray[currentIndex] == 1 && currentPlayer == 2)
        {
            player2Score += combinedArray[combinedArray.Length - currentIndex - 1];
            combinedArray[combinedArray.Length - currentIndex - 1] = 0;
            print(combinedArray.Length - currentIndex);
        }
        player1Hole = combinedArray.Take(halfHole).ToArray();
        player2Hole = combinedArray.Skip(halfHole).ToArray();
        UpdateUI();
        isMovingStones = false;
    }
   

    void CheckWinner()
    {
        
    }
}
