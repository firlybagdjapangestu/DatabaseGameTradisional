using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class EngklekManager : MonoBehaviour
{
    [Header("Level Attribut")]
    [SerializeField] int MaxLevel = 3;
    int currentLevel;

    [Header("Reference")]
    public Text scoreText;
    [SerializeField] private GameObject[] engklekPrefabs;
    [SerializeField] private Transform[] panelSpawnTransform;
    [SerializeField] private GameObject[] panelPlayerStage1;
    [SerializeField] private GameObject[] panelPlayerStage2;

    [Space(5f)]
    [SerializeField] GameObject panelHelper;
    int guideCount = 0;
    [SerializeField] GameObject[] playerGuidePanel;
    [SerializeField] AudioClip winSfx;

    [Header("Attribut")]
    public int[] scorePlayers = new int[2];
    public float speed;
    public int playersTurn;
    [SerializeField] private float delayAfterPlayerTurn;

   
    public int point;
    [SerializeField] private GameObject playerTurnText;
    [SerializeField] private Text delayAfterPlayerTurnText;
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private TextMeshProUGUI characterWinnerText;

    int indexObjectEngklek = 0 ;
    // Start is called before the first frame update
    void Start()
    {
        indexObjectEngklek = Random.Range(0, engklekPrefabs.Length);
        currentLevel = 1;
        StartCoroutine(DelayStartGame());
    }

    // Update is called once per frame
    void Update()
    {
        InputControll();
    }

    #region InputManager
    void InputControll()
    {
        if (Input.touchCount > 0)
        {
            // Ambil posisi sentuhan pertama (bisa disesuaikan dengan indeks sesuai kebutuhan)
            Touch touch = Input.GetTouch(0);

            Vector2 touchPosition = touch.position;
            Vector2 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldTouchPosition, Vector2.zero);
            
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("BoxEngklek"))
                {
                    AddScore(hit.collider);
                }
            }
        }

    }

    void AddScore(Collider2D collider)
    {
        Color oriColor = collider.GetComponent<SpriteRenderer>().color;


        collider.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.3f);
        collider.GetComponent<BoxCollider2D>().enabled = false;

        GameManager.instance.StartSfx(GameManager.instance.allSfx[1]);
        scorePlayers[playersTurn]++;
        scoreText.text =
            "Skor\n" + "P1 : " + scorePlayers[0].ToString() + "\nP2 : " + scorePlayers[1].ToString();

    }

    #endregion

    #region StartReg
    private IEnumerator DelayStartGame()
    {
        panelHelper.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(DelayPlayerTurn());
    }

    #endregion

    #region Transition
    public void PlayerTurns()
    {
        playersTurn++;

        foreach (GameObject item in panelPlayerStage1)
            item.SetActive(true);
        foreach (GameObject item in panelPlayerStage2)
            item.SetActive(true);

        playerGuidePanel[0].SetActive(false);
        playerGuidePanel[1].SetActive(false);

        if (playersTurn >= scorePlayers.Length)
        {
            playersTurn = 0;
            currentLevel++;
            //Naik level
            if (currentLevel > MaxLevel)
            {
                if (scorePlayers[0] == scorePlayers[1] && scorePlayers[0] != 0)
                {
                    MaxLevel++;
                }
                else
                {
                    GameOver();
                    return;
                }

            }

            LevelUp();
        }

        StartCoroutine(DelayPlayerTurn());

    }

    private IEnumerator DelayPlayerTurn()
    {

        if (guideCount < 2)
        {
            playerGuidePanel[playersTurn].SetActive(true);
        }

        guideCount++;

        playerTurnText.GetComponent<Text>().text = "Pemain " + (playersTurn + 1) + " \n Bersiap!!!";


        Vector3 tempRot = Vector3.zero;
        if (playersTurn == 1)
            tempRot = new Vector3(0, 0, 180f);

        playerTurnText.GetComponent<RectTransform>().eulerAngles = tempRot;
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


        yield return new WaitForSeconds(0.2f);

        playerTurnText.SetActive(false);
        GameObject tempObject = Instantiate(engklekPrefabs[indexObjectEngklek], panelSpawnTransform[playersTurn]);

        float dir = 1f;
        if (playersTurn == 1)
            dir = -1;


        //tempObject.GetComponent<RectTransform>().eulerAngles = tempRot;
        tempObject.transform.localScale = new Vector3(1f, dir, 0f);
        tempObject.GetComponent<EngklekController>().dir = dir;
        tempObject.GetComponent<EngklekController>().spawnPos = panelSpawnTransform[playersTurn];

        if (playersTurn == 0)
        {
            foreach (GameObject item in panelPlayerStage1)
                item.SetActive(false);
        }
        else if (playersTurn == 1)
        {
            foreach (GameObject item in panelPlayerStage2)
                item.SetActive(false);
        }
        
    }

    private void LevelUp()
    {
        indexObjectEngklek = Random.Range(0, engklekPrefabs.Length);
        speed = speed + 2;
    }
    #endregion

    #region EndGame
    private void GameOver()
    {
        winnerPanel.SetActive(true);

        if (scorePlayers[0] > scorePlayers[1])
        {
            
            characterWinnerText.SetText("Selamat player 1 menang, apakah anda ingin memainkan game ini lagi / keluar ?");
            Debug.Log("Player 1 Menang");
        }
        else if (scorePlayers[0] < scorePlayers[1])
        {
            
            characterWinnerText.SetText("Selamat player 2 menang, apakah anda ingin memainkan game ini lagi / keluar ?");
            Debug.Log("Player 2 Menang");
        }
        else
        {
            
            characterWinnerText.SetText("Wow Seri !!!, apakah anda ingin memainkan game ini lagi / keluar ?");
            Debug.Log("Seri");
        }

        AudioManager.Singleton.SetClipForce(winSfx);

        Time.timeScale = 0;
    }
    #endregion

}
