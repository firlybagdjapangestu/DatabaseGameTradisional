using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngklekController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject engklek;
    [SerializeField] private EngklekManager engklekMenager;
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
        engklek.transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (engklek.transform.position.y <= 70)
        {
            ChangePlayer();
            Destroy(gameObject);  
        }
    }
    public void TouchEngklek()
    {
        GameManager.instance.StartSfx(GameManager.instance.allSfx[1]);
        engklekMenager.players[engklekMenager.playersTurn]++;
        engklekMenager.scoreText.text = 
            "Skor\n" + engklekMenager.players[0].ToString() + " vs " + engklekMenager.players[1].ToString();
    }

    private void ChangePlayer()
    {
        engklekMenager.PlayerTurns();
    }
}
