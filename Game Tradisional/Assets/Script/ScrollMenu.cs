using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollMenu : MonoBehaviour
{
    public Color[] colors;
    public GameObject scrollbar, imageContent;
    float scroll_pos = 0;
    float[] pos;
    private bool runIt = false;
    private float time;
    private Button takeTheBtn;
    int btnNumber;

    int currentScrollNumber = 0;

    [Header("Level")]
    public SceneData[] sceneData;

    [Header("Panel Select Game")]
    [SerializeField] GameObject panelSelectGame;
    [SerializeField] GameObject panelMainMenu;

    private void Start()
    {
        FillAllGame();
    }

    Button btn;
    void FillAllGame()
    {
        for (int i = 0; i < sceneData.Length; i++)
        {
            transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = sceneData[i].sceneTitle;
            transform.GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite = sceneData[i].sceneImage;
            transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = sceneData[i].sceneDescription;

            btn = transform.GetChild(i).transform.GetChild(3).GetComponent<Button>();
            btn.onClick.AddListener(LoadScene);
            
            if (i != 0)
            {
                btn.interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panelMainMenu.SetActive(true);
            panelSelectGame.SetActive(false);
        }

        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        if (runIt)
        {
            GecisiDuzenle(distance, pos, takeTheBtn);
            time += Time.deltaTime;

            if (time > 1f)
            {
                time = 0;
                runIt = false;
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            if (!buttonClicked)
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                    {
                        currentScrollNumber = i;

                        //scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                    }
                }
            }

            if (buttonClicked && CheckDistance(scrollbar.GetComponent<Scrollbar>().value, pos[currentScrollNumber], 0.02f))
            {
                scroll_pos = pos[currentScrollNumber];
                buttonClicked = false;
            }


            scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[currentScrollNumber], 0.1f);
        }

        transform.GetChild(currentScrollNumber).localScale = Vector2.Lerp(transform.GetChild(currentScrollNumber).localScale, new Vector2(1f, 1f), 0.1f);
        transform.GetChild(currentScrollNumber).transform.GetChild(3).GetComponent<Button>().interactable = true;

        for (int i = 0; i < pos.Length; i++)
        {
            if (i != currentScrollNumber)
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                transform.GetChild(i).transform.GetChild(3).GetComponent<Button>().interactable = false;
            }
            
        }


    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneData[currentScrollNumber].sceneTitle);
    }

    private void GecisiDuzenle(float distance, float[] pos, Button btn)
    {
        // btnSayi = System.Int32.Parse(btn.transform.name);

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], 1f * Time.deltaTime);

            }
        }

        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            btn.transform.name = ".";
        }

    }


    private bool CheckDistance(float _from, float _to, float _distance)
    {
        bool temp = false;
        if (_from > _to && _from - _to <= _distance)
        {
            temp = true;
        }
        else if (_from <= _to && _to - _from <= _distance)
        {
            temp = true;
        }

        return temp;

    }

    bool buttonClicked = false;
    public void NextButton(int _AddCount)
    {
        
        if (currentScrollNumber + _AddCount >= pos.Length || currentScrollNumber + _AddCount < 0 || buttonClicked)
            return;

        buttonClicked = true;
        currentScrollNumber += _AddCount;
        //scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[currentScrollNumber], 0.1f);
    }

    public void WhichBtnClicked(Button btn)
    {
        btn.transform.name = "clicked";
        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            if (btn.transform.parent.transform.GetChild(i).transform.name == "clicked")
            {
                btnNumber = i;
                takeTheBtn = btn;
                time = 0;
                scroll_pos = (pos[btnNumber]);
                runIt = true;
            }
        }


    }
}
