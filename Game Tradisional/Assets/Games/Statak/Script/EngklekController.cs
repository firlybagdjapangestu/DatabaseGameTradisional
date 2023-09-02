using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngklekController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject engklek;
    [SerializeField] private EngklekManager engklekMenager;

    [HideInInspector] public float dir = 1f;
    [HideInInspector] public Transform spawnPos;
    private void Awake()
    {
        engklekMenager = FindObjectOfType<EngklekManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        speed = engklekMenager.speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * dir * Time.deltaTime);

        if (Vector2.Distance(engklekMenager.transform.position, this.transform.position) > 9f && this.transform.position.y < 0 && dir > 0)
        {
            ChangePlayer();
            Destroy(gameObject);
        }

        if (Vector2.Distance(engklekMenager.transform.position, this.transform.position) > 9f && this.transform.position.y > 0 && dir < 0)
        {
            ChangePlayer();
            Destroy(gameObject);
        }

    }
    public void TouchEngklek()
    {
        GameManager.instance.StartSfx(GameManager.instance.allSfx[1]);
        engklekMenager.scorePlayers[engklekMenager.playersTurn]++;
        engklekMenager.scoreText.text = 
            "Skor\n" + "P1 : " + engklekMenager.scorePlayers[0].ToString() + "\nP2 : " + engklekMenager.scorePlayers[1].ToString();
    }

    private void ChangePlayer()
    {
        engklekMenager.PlayerTurns();
    }
}
