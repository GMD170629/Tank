using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{

    //生命值
    public int lifeValue = 3;
    //是否死亡
    public bool isDead;
    public bool isDefeat;

    public Text playerScoreText;
    public Text playerLifevalueText;
    public GameObject isDefeatUI;
    
    //得分
    public int playerScore;

    public GameObject born;

    private static PlayManager _instance;

    public static PlayManager Instance
    {
        get
        {
            return _instance;
        }
        
        set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isDefeat)
        {
            isDefeatUI.SetActive(true);
            Invoke(nameof(ReturnToTheMainMene), 3);
            return;
        }
        
        if (isDead)
        {
            Recover();
        }


        playerScoreText.text = playerScore.ToString();
        playerLifevalueText.text = lifeValue.ToString();
    }
    
    
    
    private void Recover()
    {
        if (lifeValue <= 0)
        {
            //游戏失败
            isDefeat = true; 
            Invoke(nameof(ReturnToTheMainMene), 3);
        }
        else
        {
            lifeValue--;
            //玩家重生
            GameObject go = Instantiate(born, new Vector3(-4, -7, 0), Quaternion.identity);
            go.GetComponent<Born>().createPlayer = true;
            isDead = false;
        }
    }

    private void ReturnToTheMainMene()
    {
        SceneManager.LoadScene(0);
    }
}
