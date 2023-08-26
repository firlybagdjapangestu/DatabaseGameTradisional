using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GasingMenager : MonoBehaviour
{
    [Header("Reference")]
    public GameObject[] gasing;

    [Header("Guide Panel")]
    [SerializeField] GameObject guidePanel;
    [SerializeField] GameObject playerStageHelper;

    [Header("Endgame Reference")]
    [SerializeField] AudioClip clipWin;
    [SerializeField] GameObject panelEndGame;
    [SerializeField] TextMeshProUGUI winConditionText;

    [HideInInspector] public bool gameEnd = false;
    [HideInInspector] public bool gameStarted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        gameEnd = false;
        gameStarted = false;

        panelEndGame.SetActive(false);

        guidePanel.SetActive(true);
        playerStageHelper.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnd)
            return;

        WinOrLose();
    }

    #region EndGame
    void WinOrLose()
    {
        if (gasing[0] == null)
        {
            Debug.Log("Player 2 Win");
            gameEnd = true;
            StartCoroutine(EndGameState(1));
        }
        else if (gasing[1] == null)
        {
            Debug.Log("Player 1 Win");
            gameEnd = true;
            StartCoroutine(EndGameState(0));
        }
    }

    IEnumerator EndGameState(int _playerWin)
    {
        //play music win
        //set to play music Sfx

        if (_playerWin == 0)
            winConditionText.SetText("Selamat Player 1 memenangkan pertandingan !!!, Pertandingan yang hebat..");

        if (_playerWin == 1)
            winConditionText.SetText("Selamat Player 2 memenangkan pertandingan !!!, Pertandingan yang hebat..");

        //AudioManager.Singleton.SetClipFromList(0, 0, false, false);

        yield return new WaitForSeconds(2f);

        panelEndGame.SetActive(true);
        AudioManager.Singleton.SetClipForce(clipWin);

        GameManager.instance.SetTimeScale(0f);

    }

    #endregion


    #region StartGameState

    [Header("Start Attribut")]
    [SerializeField] float inBattleForce;
    [SerializeField] Transform arenaCenter;
    float randomnes = 0.5f;

    bool player1Ready = false;
    bool player2Ready = false;

    public void StartInBattle(int _playerId)
    {

        Vector2 centerArenaRandom = arenaCenter.position;
        centerArenaRandom.x = Random.Range(arenaCenter.position.x - randomnes, arenaCenter.position.x + randomnes);

        Vector2 dir = (centerArenaRandom - (Vector2) gasing[_playerId].transform.position).normalized;

        Rigidbody2D rb = gasing[_playerId].GetComponent<Rigidbody2D>();

        rb.AddForce(dir * inBattleForce, ForceMode2D.Impulse);

        gasing[_playerId].GetComponent<GasingController>().startSpin = true;

        if (_playerId == 0)
            player1Ready = true;

        if (_playerId == 1)
            player2Ready = true;

        if (player1Ready && player2Ready && !gameStarted)
            StartCoroutine(DelayStart());

        guidePanel.SetActive(false);
        playerStageHelper.SetActive(false);
    }

    IEnumerator DelayStart()
    {
        
        yield return new WaitForSeconds(1.5f);
        //AudioManager.Singleton.SetClipFromList(0, 0, true, true);
        gameStarted = true;
    }


    #endregion
}
