using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_TimeAndSets : MonoBehaviour
{
    public static Controller_TimeAndSets Instance { get; private set; }


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


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }


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
            if(Controller_Points.Instance.numScore1 > Controller_Points.Instance.numScore2)
            {
                Debug.Log("Player1 WINS");
                restartPlayerPos();

                gameTime = gameMaxTime;
                Controller_Points.Instance.restartPoints();
                SetWinnerActivateText.text = "Player1 Won the SET";
                ShowSetWinnerText = true;
            }

            else if (Controller_Points.Instance.numScore2 > Controller_Points.Instance.numScore1)
            {
                Debug.Log("Player2 WINS");
                restartPlayerPos();

                gameTime = gameMaxTime;
                Controller_Points.Instance.restartPoints();

                SetWinnerActivateText.text = "Player2 Won the SET";
                ShowSetWinnerText = true;
            }

            else if(Controller_Points.Instance.numScore1 == Controller_Points.Instance.numScore2)
            {
                Debug.Log("Draw");
                restartPlayerPos();
                gameTime = gameMaxTime;
                Controller_Points.Instance.restartPoints();

                ShowSetWinnerText = true;
            }
        }


        if(ShowSetWinnerText)
        {
            
            showWinnerFunc();
        }

        
    }

    void restartPlayerPos()
    {
        player1.transform.position = player1.respawnPosition;
        player2.transform.position = player2.respawnPosition;
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
