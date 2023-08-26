using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KaretGameManager : MonoBehaviour
{
    [Header("Reference")]
    
    [SerializeField] Text countdouwnText;
    [SerializeField] GameObject panelEndGame;
    [SerializeField] TextMeshProUGUI textCondtion;

    [SerializeField] GameObject panelTextCoundownStart;

    [SerializeField] GameObject panelHelper;

    [Header("Effect")]
    [SerializeField] AudioClip endGameClip;

    [Header("Attribut")]
    [SerializeField] float gameplayDuration = 20f;

    float currentGameplayDuration;


    public float maxScore;
    [HideInInspector] public float scoreNow;

    public bool playable = false;
    bool onEndagameState = false;

    [HideInInspector] public int PlayerWin = 0;

    void Start()
    {
        GameManager.instance.SetTimeScale(1f);
        playable = false;

        currentGameplayDuration = gameplayDuration;
        countdouwnText.text = gameplayDuration.ToString("F0");

        StartCoroutine(DelayStartGame());
    }

    // Update is called once per frame
    void Update()
    {

        CountdownDuration();

        if (onEndagameState)
            return;

        if (currentGameplayDuration <= 0 && !onEndagameState)
        {
            // end game
            EndGame(0);
        }

        if (scoreNow <= 0 && !onEndagameState)
        {
            EndGame(2);
        }

        if (scoreNow >= maxScore && !onEndagameState)
        {
            EndGame(1);
        }

    }

    void CountdownDuration()
    {
        if(currentGameplayDuration > 0 && playable)
            currentGameplayDuration -= Time.deltaTime;

        countdouwnText.text = currentGameplayDuration.ToString("F0");
    }

    #region Startgame
        
    IEnumerator DelayStartGame()
    {
        panelHelper.SetActive(true);

        yield return new WaitForSeconds(2f);

        panelHelper.SetActive(false);

        panelTextCoundownStart.SetActive(true);

        TextMeshProUGUI textCount = panelTextCoundownStart.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        textCount.SetText("Bersiap");
        yield return new WaitForSeconds(1f);

        textCount.SetText("3");
        yield return new WaitForSeconds(1f);

        textCount.SetText("2");
        yield return new WaitForSeconds(1f);

        textCount.SetText("1");
        yield return new WaitForSeconds(1f);

        textCount.SetText("Mulai !!");

        yield return new WaitForSeconds(1f);
        playable = true;
        panelTextCoundownStart.SetActive(false);

    }

    #endregion

    #region Endgame
    void EndGame(int _playerWin)
    {
        playable = false;
        onEndagameState = true;
        countdouwnText.text = "0";

        //find who win
        int playerWin = _playerWin;
        if (_playerWin == 0)
        {
            if (scoreNow < maxScore / 2)
                playerWin = 2;
            else if (scoreNow > maxScore / 2)
                playerWin = 1;
            else { playerWin = 0; }
        }

        PlayerWin = playerWin;
        StartCoroutine(StateEndGame(playerWin));
    }

    IEnumerator StateEndGame(int _player)
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Singleton.SetClipForce(endGameClip);
        GameManager.instance.SetTimeScale(0);
        //GameManager.instance.StartSfx(endGameClip);

        if (_player == 2)
            textCondtion.SetText("Player 2 Memenangkan pertandingan !!!");
        else if (_player == 1)
            textCondtion.SetText("Player 1 Memenangkan pertandingan !!!");
        else
        {
            textCondtion.SetText("Wow Seri, Hebat !!!");
        }

        panelEndGame.SetActive(true);
    }
    #endregion

}
