using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    GasingMenager Gm;

    GameObject[] gasingObject;
    Rigidbody2D[] gasingRb = new Rigidbody2D[2];
    GasingController[] gasingControll = new GasingController[2];

    [SerializeField] float gravityField = 1f;

    void Start()
    {
        Gm = GetComponent<GasingMenager>();
        gasingObject = Gm.gasing;

        gasingRb[0] = gasingObject[0].GetComponent<Rigidbody2D>();
        gasingRb[1] = gasingObject[1].GetComponent<Rigidbody2D>();

        gasingControll[0] = gasingObject[0].GetComponent<GasingController>();
        gasingControll[1] = gasingObject[1].GetComponent<GasingController>();
    }

    // Update is called once per frame

    bool p1SetGravity = false;
    bool p2SetGravity = false;

    void FixedUpdate()
    {
        if (!Gm.gameStarted)
            return;

        if (Gm.gameEnd)
            return;

        Player1Gravity();
        Player2Gravity();
        
    }

    void Player1Gravity()
    {

        if (gasingObject[0] == null)
            return;

        //P1
        Vector2 dir1 = (this.transform.position - gasingObject[0].transform.position).normalized;
        if (!gasingControll[0].analogUse && !gasingControll[0].gasingDie)
        {
            if (gasingRb[0].velocity.x < 2f && gasingRb[0].velocity.y < 2f)
                gasingRb[0].velocity += dir1 * gravityField * Time.deltaTime;
        }

    }

    void Player2Gravity()
    {
        if (gasingObject[1] == null)
            return;

        //P2
        Vector2 dir2 = (this.transform.position - gasingObject[1].transform.position).normalized;
        if (!gasingControll[1].analogUse && !gasingControll[1].gasingDie)
        {
            if (gasingRb[1].velocity.x < 2f && gasingRb[1].velocity.y < 2f)
                gasingRb[1].velocity += dir2 * gravityField * Time.deltaTime;
        }
        
    }
}
