using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BijiKaret : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] KaretGameManager GM;
    [SerializeField] GameObject effectHit;
    [SerializeField] GameObject[] player;
    [SerializeField] GameObject deadEffect;
    [HideInInspector] public Animator anim;

    [Header("Sound")]
    [SerializeField] AudioClip[] clipAudio;
    
    [Header("Another")]
    [SerializeField] float hitEffectDelayTime = 0.1f;

    bool playerDead = false;

    bool clicked = false;
    float cdEffect = 1.6f;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CountdownEffect();

        CheckWhoLost();
    }

    #region EndGame
    void CheckWhoLost()
    {
        if (GM.playable)
            return;

        if (GM.PlayerWin <= 0)
            return;

        if (playerDead)
            return;

        StartCoroutine(DelayDeadEffect());

    }

    IEnumerator DelayDeadEffect()
    {
        playerDead = true;

        int playerLost = 0;
        if (GM.PlayerWin == 1) playerLost = 2;
        else if (GM.PlayerWin == 2) playerLost = 1; 


        if (playerLost > 0)
        {
            Vector2 posSpawnEffect = player[playerLost - 1].transform.position;
            player[playerLost - 1].SetActive(false);
            GameManager.instance.StartSfx(clipAudio[1]);
            Instantiate(deadEffect, posSpawnEffect, Quaternion.identity);
        }

        if (playerLost <= 0)
        {
            Vector2 posSpawnEffect = player[0].transform.position;
            player[0].SetActive(false);
            
            GameManager.instance.StartSfx(clipAudio[1]);
            Instantiate(deadEffect, posSpawnEffect, Quaternion.identity);
            
            posSpawnEffect = player[1].transform.position;
            player[1].SetActive(false);
            Instantiate(deadEffect, posSpawnEffect, Quaternion.identity);
        }

        yield return null;

        anim.SetBool("Adu", false);
        
    }

    #endregion

    #region Effect

    void CountdownEffect()
    {
        if (cdEffect > 0)
        {
            cdEffect -= Time.deltaTime;
        }

        if (cdEffect <= 0)
        {
            anim.SetBool("Adu", false);
        }
    }

    public void SpawnEffectHit()
    {
        cdEffect = 0.5f;
        anim.SetBool("Adu", true);
        

        if (spawned)
            return;

        if (!GM.playable)
            return;

        StartCoroutine(HitEffetDelay());
    }

    bool spawned = false;
    IEnumerator HitEffetDelay()
    {
        spawned = true;
        GameManager.instance.StartSfx(clipAudio[0]);
        Instantiate(effectHit, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(hitEffectDelayTime);

        spawned = false;
    }
    #endregion




}
