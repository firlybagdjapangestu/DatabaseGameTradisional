using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Sprite[] characterSprite;
    public Image characterPause;
    public bool pauseGame;
    public GameObject pauseImage;
    public static GameManager instance;
    public float startGameDelay = 3;
    public AudioSource audioSource;
    public AudioClip[] allSfx;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetTimeScale(1f);

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        initialPosition = cameraTransform.localPosition;
        audioSource = GetComponent<AudioSource>();

        return;

        if(pauseImage == null && characterPause == null)
        {
            return;
        }
        else
        {
            pauseImage = GameObject.Find("PausePanel");
            characterPause = GameObject.Find("PausePanel").GetComponent<Image>();
            pauseImage.SetActive(false);
        }
        
    }

    public void PauseGame()
    {
        if (!pauseImage.activeSelf)
        {
            pauseGame = true;
            pauseImage.SetActive(true);
            //characterPause.sprite = characterSprite[21];
            Time.timeScale = 0f;
        }
        else
        {
            pauseGame = false;
            pauseImage.SetActive(false);
            //characterPause.sprite = characterSprite[21];
            Time.timeScale = 1f;
        }
        
    }

    public void SetTimeScale(float  _scale)
    {
        Time.timeScale = _scale;
    }
    
    public void LoadScene(string sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    #region Camera Behaviour
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    private Vector3 initialPosition;
    private bool isShaking = false;

    public void Shake()
    {
        if (!isShaking)
        {
            isShaking = true;
            InvokeRepeating("DoShake", 0f, 0.01f);
            Invoke("StopShake", shakeDuration);
        }
    }

    private void DoShake()
    {
        float shakeX = Random.Range(-1f, 1f) * shakeMagnitude;
        float shakeY = Random.Range(-1f, 1f) * shakeMagnitude;
        Vector3 shakePosition = new Vector3(shakeX, shakeY, initialPosition.z);
        cameraTransform.localPosition = initialPosition + shakePosition;
    }

    private void StopShake()
    {
        CancelInvoke("DoShake");
        cameraTransform.localPosition = initialPosition;
        isShaking = false;
    }
    #endregion

    #region Game Effect
    public ParticleSystem particleSystemPrefab;

    public void StartParticleSystem(Transform position)
    {
        ParticleSystem newParticleSystem = Instantiate(particleSystemPrefab, position.position, Quaternion.identity);
        newParticleSystem.Play();
    }
    #endregion

    #region Sound Effect
    public void StartSfx(AudioClip sfx)
    {
       
        audioSource.PlayOneShot(sfx);
    }
    #endregion
}
