using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KaretController : MonoBehaviour
{
    public Image fillImage;
    [SerializeField] private Image targetPowerImage;
    [SerializeField] private GameObject characterWinnerImage;
    [SerializeField] private Text characterWinnerText;
    [SerializeField] private Text scoreText;
    public float decreaseSpeed = 0.3f;
    public float increaseSpeed = 2f;

    public GameObject buttonPrefab;
    public Canvas canvas;

    [SerializeField] private Text timerText;
    public float gameTime = 60f;
    private float timer;
    [SerializeField] private float targetPower;
    [SerializeField] private float delayAfterPlayerTurn = 3f;
    [SerializeField] private Text delayAfterPlayerTurnText;


    private bool isPressed;
    private bool isButtonActive;

    [SerializeField] private int[] players;
    [SerializeField] private int playersTurn;
    [SerializeField] private GameObject playerTurnText;
    private bool isPlayerLost = false;

    private void Start()
    {
        Time.timeScale = 1;
        timer = 0;
        timerText.text = "Tunggu";
        isPlayerLost = true;
        StartCoroutine(DelayPlayerTurn());
    }


    private void Update()
    {
        // Pengurangan Fill Amount secara otomatis
        if (fillImage.fillAmount > 0)
        {
            fillImage.fillAmount -= decreaseSpeed * Time.deltaTime;
        }

        // Penambahan Fill Amount saat layar ditekan
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
        }
        else
        {
            isPressed = false;
        }

        // Cek jika waktu permainan belum habis
        if (timer > 0)
        {
            if (isPressed && fillImage.fillAmount < 1)
            {
                GameManager.instance.Shake();
                fillImage.fillAmount += increaseSpeed * Time.deltaTime;
            }

            // Cek apakah Fill Amount lebih besar dari 80% dan tombol tidak aktif
            if (fillImage.fillAmount > targetPower)
            {
                SpawnButton();
            }
            else
            {
                isButtonActive = false;
                buttonPrefab.SetActive(false);
            }

            // Update timer permainan
            timer -= Time.deltaTime;
            timerText.text = Mathf.RoundToInt(timer).ToString();
        }
        else
        {
            if (!isPlayerLost) // Tambahkan kondisi untuk memeriksa apakah pemain sudah kalah sebelum memanggil fungsi PlayerLost()
            {
                isPlayerLost = true; // Set status pemain kalah menjadi true
                PlayerLost();
            }
        }
    }

    // Fungsi untuk munculkan tombol random
    private void SpawnButton()
    {
        if (isButtonActive)
        {
            return; // Jika tombol sudah aktif, keluar dari fungsi
        }

        // Mengatur posisi tombol menjadi posisi acak pada layar Canvas
        buttonPrefab.transform.position = new Vector2(Random.Range(0, canvas.pixelRect.width), Random.Range(0, canvas.pixelRect.height));

        // Aktifkan tombol dan atur isButtonActive menjadi true
        buttonPrefab.SetActive(true);
        isButtonActive = true;

    }


    // Fungsi untuk mengatur pemain kalah
    private void PlayerLost()
    {
        timer = 0;
        fillImage.fillAmount = 0f;
        PlayerTurns();
    }

    // Fungsi untuk mengatur pemain Menang
    public void PlayerWins()
    {
        isPlayerLost = true;
        players[playersTurn]++;
        scoreText.text = "Skor\n" + players[0].ToString() + " vs " + players[1].ToString();
        fillImage.fillAmount = 0f;
        timer = 0;        
        PlayerTurns();
    }
    
    private void PlayerTurns()
    {
        playersTurn++;
        timerText.text = "Tunggu";
        StartCoroutine(DelayPlayerTurn());

        //Mencek apakah kedua player telah memainkan game
        if (playersTurn == players.Length)
        {
            playersTurn = 0;
            //Naik level
            if (players[0] == players[1])
            {
                StartCoroutine(DelayLevelTurn()); // Memulai coroutine delay
            }
            else if (players[0] != players[1])
            {
                GameOver();
            }
        }
    }
    private IEnumerator DelayPlayerTurn()
    {
        playerTurnText.GetComponent<Text>().text = "Pemain " + (playersTurn) + " \n Bersiap!!!";
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
        timer = gameTime;
        isPlayerLost = false;
    }


    private IEnumerator DelayLevelTurn()
    {
        yield return new WaitForSeconds(delayAfterPlayerTurn); // Menunggu selama delay yang ditentukan
        // Kode yang ingin dieksekusi setelah delay
        increaseSpeed = increaseSpeed - 0.1f;
        gameTime = gameTime - 5;
        timer = gameTime;
        targetPower = targetPower + 0.01f;
        targetPowerImage.fillAmount = targetPower;
        if (increaseSpeed < 1f && gameTime < 5 && targetPower > 0.9f)
        {
            targetPower = 0.9f;
            increaseSpeed = 1f;
            gameTime = 5;
        }
    }

    // Fungsi yang dipanggil ketika waktu permainan habis
    private void GameOver()
    {
        if (players[0] > players[1])
        {
            characterWinnerImage.SetActive(true);
            characterWinnerText.text = "Selamat player 1 menang, apakah anda ingin memainkan game ini lagi / keluar ?";
            timerText.text = "Game Berakhir";
            Debug.Log("Player 1 Menang");
            timer = 0;
        }
        else
        {
            characterWinnerImage.SetActive(true);
            characterWinnerText.text = "Selamat player 2 menang, apakah anda ingin memainkan game ini lagi / keluar ?";
            timerText.text = "Game Berakhir";
            Debug.Log("Player 2 Menang");
            timer = 0;
        }
        Time.timeScale = 0;
        // Lakukan tindakan game over di sini (misalnya, menampilkan pesan game over, menghentikan permainan, dll.)
    }
}
