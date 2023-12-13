using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TheGirl : MonoBehaviour
{
    public Sprite spriteGame;
    public Image imageGame;
    public TextMeshProUGUI textQuestion;
    public string stringQuestion;
    public Button buttonLoadScene; // Pastikan tombol terkait terhubung di Inspector
    public GameObject lebelGame;
    public GameObject lebelAsking;
    public string nameScene; // Nama scene yang akan dimuat

    private void Start()
    {
        UpdatePanelQuestion();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boy")) // Menggunakan CompareTag lebih optimal daripada collision.tag
        {
            buttonLoadScene.onClick.AddListener(() => SceneLoader(nameScene));
            lebelGame.SetActive(false);
            lebelAsking.SetActive(true);
            UpdatePanelQuestion();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Boy")) // Menggunakan CompareTag lebih optimal daripada collision.tag
        {
            lebelGame.SetActive(true);
            lebelAsking.SetActive(false);
            UpdatePanelQuestion();
        }
    }

    public void SceneLoader(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void UpdatePanelQuestion()
    {
        imageGame.sprite = spriteGame;
        textQuestion.text = stringQuestion;
        // Fungsi untuk memperbarui panel pertanyaan
    }
}
