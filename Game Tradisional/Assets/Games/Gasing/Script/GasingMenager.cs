using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasingMenager : MonoBehaviour
{
    [SerializeField] private GameObject[] gasing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WinOrLose();
    }

    void WinOrLose()
    {
        if(gasing[0] == null)
        {
            Debug.Log("Player 2 Win");
        }
        else if(gasing[1] == null)
        {
            Debug.Log("Player 1 Win");
        }
    }
}
