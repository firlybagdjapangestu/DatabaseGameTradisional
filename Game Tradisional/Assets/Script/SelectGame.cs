using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectGame : MonoBehaviour
{
    public SceneData[] sceneData; // Referensi ke ScriptableObject SceneData

    private int currentIndex; // Indeks scene saat ini

    [SerializeField] private RectTransform swipeContainer;
    [SerializeField] private Vector2 initialContainer;
    [SerializeField] private float swipeDistance = 500f; // Jarak geser swipe dalam satuan piksel
    [SerializeField] private float swipeDuration = 0.5f; // Durasi animasi swipe dalam detik

    [SerializeField] private Text titleGame;
    [SerializeField] private Text descriptionGame;
    [SerializeField] private Image imageGame;
    [SerializeField] private Button playButton;

    private Coroutine swipeCoroutine;

    private void Start()
    {
        initialContainer = swipeContainer.anchoredPosition;
        currentIndex = 0; // Menetapkan indeks awal ke 0 saat memulai permainan
        ChangeScene();
    }

    public void NextScene()
    {
        currentIndex++;
        if (currentIndex >= sceneData.Length)
        {
            currentIndex = 0; // Kembali ke indeks awal jika mencapai akhir daftar scene
        }
        ChangeScene();
        AnimateSwipe(Vector2.left);
    }

    public void PreviousScene()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = sceneData.Length - 1; // Kembali ke indeks akhir jika mencapai awal daftar scene
        }
        ChangeScene();
        AnimateSwipe(Vector2.right);
    }

    private void ChangeScene()
    {
        titleGame.text = sceneData[currentIndex].sceneTitle;
        descriptionGame.text = sceneData[currentIndex].sceneDescription;
        imageGame.sprite = sceneData[currentIndex].sceneImage;
        playButton.onClick.AddListener(LoadScene);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneData[currentIndex].sceneTitle);
    }

    private void AnimateSwipe(Vector2 direction)
    {
        if (swipeCoroutine != null)
        {
            StopCoroutine(swipeCoroutine);
        }
        swipeCoroutine = StartCoroutine(DoSwipeAnimation(direction));
    }

    private IEnumerator DoSwipeAnimation(Vector2 direction)
    {
        Vector2 startPosition = swipeContainer.anchoredPosition;
        Vector2 endPosition = startPosition + direction * swipeDistance;

        float elapsedTime = 0f;
        while (elapsedTime < swipeDuration)
        {
            float t = elapsedTime / swipeDuration;
            swipeContainer.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        swipeContainer.anchoredPosition = endPosition;

        // Kembalikan posisi swipeContainer ke posisi awal setelah animasi selesai
        yield return new WaitForSeconds(0.5f); // Waktu tunda sebelum mengembalikan posisi
        swipeContainer.anchoredPosition = initialContainer;
        swipeCoroutine = null;
    }
}
