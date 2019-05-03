using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private float gameTime;
    public float gameMaxTime;
    private float gameTimeRounded;

    public Text textTime;





    // Start is called before the first frame update
    void Start()
    {
        gameTime = 90f;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        updateText();

        gameTime -= Time.deltaTime;
        gameTimeRounded = Mathf.RoundToInt(gameTime);


                
    }



    void updateText()
    {
        
        textTime.text = "" + gameTimeRounded;
    }
}
