using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Controller_Points : MonoBehaviour
{
    // Start is called before the first frame update
    public static Controller_Points Instance { get; private set; }

    public Text Score1Text;
    public Text Score2Text;
    public int numScore1=0;
    public int numScore2=0;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void pointScored(int player_number)
    {
        //restart disc
        addScore(player_number);
    }
    public void addScore(int player_number)
    {
        if(player_number==1)
        {
            numScore1++;
        }
        else
        {
            numScore2++;

        }

        updateText();
    }
    public void restartPoints()
    {
        numScore1 = 0;
        numScore2 = 0;
        updateText();
    }


    private void updateText()
    {
        Score1Text.text = "" + numScore1;
        Score2Text.text = "" + numScore2;
    }
    public bool initialThrow()
    {
        if (numScore1 == 0 && numScore2 == 0)
            return true;
        else
            return false;
    }
}
