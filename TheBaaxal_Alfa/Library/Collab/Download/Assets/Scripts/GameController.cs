using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private float gameTime;
    public float gameMaxTime;
    private float gameTimeRounded;
    private int SetsNumInt;
    public Disc_Movement disc_Controller;
    public PlayerController_p1 player1;
    public PlayerController_p2 player2;

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


        if(gameTimeRounded <= 0)
        {
            if(disc_Controller.numScore1 > disc_Controller.numScore2)
            {
                Debug.Log("Player1 WINS");
                player1.transform.position = player1.respawnPosition;
                player2.transform.position = player2.respawnPosition;
                gameTime = gameMaxTime;
                disc_Controller.numScore1 = 0;
                disc_Controller.numScore2 = 0;
                SetWinnerActivateText.text = "Player1 Won the SET";
                ShowSetWinnerText = true;
            }

            else if (disc_Controller.numScore2 > disc_Controller.numScore1)
            {
                Debug.Log("Player2 WINS");
                player1.transform.position = player1.respawnPosition;
                player2.transform.position = player2.respawnPosition;
                gameTime = gameMaxTime;
                disc_Controller.numScore1 = 0;
                disc_Controller.numScore2 = 0;
                SetWinnerActivateText.text = "Player2 Won the SET";
                ShowSetWinnerText = true;
            }

            else if(disc_Controller.numScore1 == disc_Controller.numScore2)
            {
                Debug.Log("Draw");
                player1.transform.position = player1.respawnPosition;
                player2.transform.position = player2.respawnPosition;
                gameTime = gameMaxTime;
                disc_Controller.numScore1 = 0;
                disc_Controller.numScore2 = 0;
                ShowSetWinnerText = true;
            }
        }


        if(ShowSetWinnerText)
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

        if(timer >= 3f)
        {
            SetWinnerActivateText.enabled = false;
            ShowSetWinnerText = false;
            timer = 0;
        }
    }
}
