using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaretInputControll : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] BijiKaret PlayerKaret;
    [SerializeField] Image fillImage;

    [SerializeField] float maxScore = 100f;
    [SerializeField] float scorePerClick = 2f;
    float scoreNow;

    KaretGameManager Gm;

    private void Start()
    {
        Gm = GetComponent<KaretGameManager>();

        Gm.maxScore = maxScore;
       
        scoreNow = maxScore / 2;
        fillImage.fillAmount = scoreNow / maxScore;

        Gm.scoreNow = scoreNow;
    }

    void Update()
    {
        fillImage.fillAmount = scoreNow / maxScore;
        Gm.scoreNow = scoreNow;

        if (!Gm.playable)
            return;

        //InputControll();
    }

    void InputControll()
    {
       
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.y < Screen.height / 2)
            {
                AddPoint(1);
            }
                
            if (touch.position.y > Screen.height / 2)
            {
                AddPoint(-1);
            }
        }
    }


    public void AddPoint(int _player)
    {
        if (!Gm.playable)
            return;

        if (_player > 0)
        {
            //Debug.Log("Point Player 1 !!");
            scoreNow += scorePerClick;
        }
        else
        {
            //Debug.Log("Point Player 2 !!");
            scoreNow -= scorePerClick;
        }

        PlayerKaret.SpawnEffectHit();
    }
}
