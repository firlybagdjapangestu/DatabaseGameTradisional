using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngklekManager : MonoBehaviour
{
    public Text scoreText;
    [SerializeField] private GameObject[] engklekPrefabs;
    [SerializeField] private Transform panelTransform;


    public int[] players;
    public float speed;
    public int playersTurn;
    [SerializeField] private float delayAfterPlayerTurn;

    public int point;
    [SerializeField] private GameObject playerTurnText;
    [SerializeField] private Text delayAfterPlayerTurnText;
    [SerializeField] private GameObject characterWinnerImage;
    [SerializeField] private Text characterWinnerText;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayPlayerTurn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerTurns()
    {
        playersTurn++;
        StartCoroutine(DelayPlayerTurn());
        if (playersTurn == players.Length)
        {
            playersTurn = 0;            
            //Naik level
            if (players[0] == players[1])
            {
                LevelUp();
            }
            else if (players[0] != players[1])
            {
                GameOver();
            }
        }
    }
    private IEnumerator DelayPlayerTurn()
    {
        playerTurnText.GetComponent<Text>().text = "Pemain " + (playersTurn + 1) + " \n Bersiap!!!";
        playerTurnText.SetActive(true);

        float startTime = Time.realtimeSinceStartup;
        float countdown = delayAfterPlayerTurn;
        delayAfterPlayerTurnText.text = Mathf.CeilToInt(countdown).ToString();

        while (countdown > 0)
        {
            float elapsedTime = Time.realtimeSinceStartup - startTime;
            countdown = delayAfterPlayerTurn - elapsedTime;
            delayAfterPlayerTurnText.text = Mathf.CeilToInt(countdown).ToString();
            yield return null;
        }

        playerTurnText.SetActive(false);
        delayAfterPlayerTurnText.text = ""; // Menghapus teks hitungan setelah selesai


        yield return new WaitForSeconds(delayAfterPlayerTurn);
        playerTurnText.SetActive(false);
        Instantiate(engklekPrefabs[Random.Range(0,engklekPrefabs.Length)], panelTransform);
    }
    private void LevelUp()
    {
        speed = speed + 100;
    }
    private void GameOver()
    {
        if (players[0] > players[1])
        {
            characterWinnerImage.SetActive(true);
            characterWinnerText.text = "Selamat player 1 menang, apakah anda ingin memainkan game ini lagi / keluar ?";
            Debug.Log("Player 1 Menang");
        }
        else
        {
            characterWinnerImage.SetActive(true);
            characterWinnerText.text = "Selamat player 2 menang, apakah anda ingin memainkan game ini lagi / keluar ?";
            Debug.Log("Player 2 Menang");
        }
        Time.timeScale = 0;
    }
}
