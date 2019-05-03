using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_IA : MonoBehaviour
{

    private float gameTime;
    public float gameMaxTime;
    private float gameTimeRounded;
    private int SetsNumInt;
    public DiscMovementIA disc_Controller;
    public PlayerController_p1 player1;
    public IA IA;

    public Text textTime;
    public Text textSetNum;
    public Text SetWinnerActivateText;

    private bool ShowSetWinnerText;
    private float timer;





    // Start is called before the first frame update
    void Start()
    {
        gameTime = gameMaxTime;
        SetsNumInt = 1;
        SetWinnerActivateText.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        updateText();

        gameTime -= Time.deltaTime;
        gameTimeRounded = Mathf.RoundToInt(gameTime);


        if (gameTimeRounded <= 0)
        {
            if (Controller_Points.Instance.numScore1 > Controller_Points.Instance.numScore2)
            {
                Debug.Log("Player1 WINS");
                player1.transform.position = player1.respawnPosition;
                IA.transform.position = IA.respawnPosition;
                gameTime = gameMaxTime;
                Controller_Points.Instance.numScore1 = 0;
                Controller_Points.Instance.numScore2 = 0;
                SetWinnerActivateText.text = "Player1 Won the SET";
                ShowSetWinnerText = true;
            }

            else if (Controller_Points.Instance.numScore2 > Controller_Points.Instance.numScore1)
            {
                Debug.Log("Player2 WINS");
                player1.transform.position = player1.respawnPosition;
                IA.transform.position = IA.respawnPosition;
                gameTime = gameMaxTime;
                Controller_Points.Instance.numScore1 = 0;
                Controller_Points.Instance.numScore2 = 0;
                SetWinnerActivateText.text = "Player2 Won the SET";
                ShowSetWinnerText = true;
            }

            else if (Controller_Points.Instance.numScore1 == Controller_Points.Instance.numScore2)
            {
                Debug.Log("Draw");
                player1.transform.position = player1.respawnPosition;
                IA.transform.position = IA.respawnPosition;
                gameTime = gameMaxTime;
                Controller_Points.Instance.numScore1 = 0;
                Controller_Points.Instance.numScore2 = 0;
                ShowSetWinnerText = true;
            }
        }


        if (ShowSetWinnerText)
        {

            showWinnerFunc();
        }


    }



    void updateText()
    {

        textTime.text = "" + gameTimeRounded;
        textSetNum.text = "" + SetsNumInt;
    }

    void showWinnerFunc()
    {
        timer += Time.deltaTime;
        SetWinnerActivateText.enabled = true;

        if (timer >= 3f)
        {
            SetWinnerActivateText.enabled = false;
            ShowSetWinnerText = false;
            timer = 0;
        }
    }
}
