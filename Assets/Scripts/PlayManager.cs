using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{

    //生命值
    public int lifeValue = 3;
    //是否死亡
    public bool isDead;
    public bool isDefeat;
    
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
        if (isDead)
        {
            Recover();
        }
    }
    
    
    
    private void Recover()
    {
        if (lifeValue <= 0)
        {
            //游戏失败
            isDefeat = true;
        }
        else
        {
            lifeValue--;
            //玩家重生
            GameObject go = Instantiate(born, new Vector3(-2, -7, 0), Quaternion.identity);
            go.GetComponent<Born>().createPlayer = true;
            isDead = false;
        }
    }
}
