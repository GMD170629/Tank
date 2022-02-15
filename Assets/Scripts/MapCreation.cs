using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreation : MonoBehaviour
{
    //用来装饰初始化地图所需物体的数组
    //0.老家 1.墙 2.障碍 3.出生效果 4.水 5.草 6.空气墙
    public GameObject[] item;

    //已经有东西的位置列表
    private List<Vector3> _itemPosition = new List<Vector3>();

    private void Awake()
    {
        //实例化老家
        CreateItem(item[0], new Vector3(0, -7, 0), Quaternion.identity);
        //用墙把老家围起来
        CreateItem(item[1], new Vector3(-1, -7, 0), Quaternion.identity);
        CreateItem(item[1], new Vector3(1, -7, 0), Quaternion.identity);
        for (int i = -1; i < 2; i++)
        {
            CreateItem(item[1], new Vector3(i, -6, 0), Quaternion.identity);
        }

        //实例化一下周围的空气墙
        for (int i = -14; i < 15; i++)
        {
            CreateItem(item[6], new Vector3(i, 8, 0), Quaternion.identity);
            CreateItem(item[6], new Vector3(i, -8, 0), Quaternion.identity);
        }

        for (int i = -7; i < 8; i++)
        {
            CreateItem(item[6], new Vector3(14, i, 0), Quaternion.identity);
            CreateItem(item[6], new Vector3(-14, i, 0), Quaternion.identity);
        }

        //玩家的出生
        GameObject go = Instantiate(item[3], new Vector3(-2, -7, 0), Quaternion.identity);
        go.GetComponent<Born>().createPlayer = true;
        //敌人的出生
        CreateItem(item[3], new Vector3(13, 7, 0), Quaternion.identity);
        CreateItem(item[3], new Vector3(0, 7, 0), Quaternion.identity);
        CreateItem(item[3], new Vector3(-13, 7, 0), Quaternion.identity);
        
        //每3s随机产生一个敌人
        InvokeRepeating("createEnemy", 4, 5);
        
        
        //产生随机地图
        for (int i = 0; i < 40; i++)
        {
            CreateItem(item[1], CreateRandomPostion(), Quaternion.identity);
        }

        for (int i = 0; i < 20; i++)
        {
            CreateItem(item[2], CreateRandomPostion(), Quaternion.identity);
        }

        for (int i = 0; i < 20; i++)
        {
            CreateItem(item[4], CreateRandomPostion(), Quaternion.identity);
        }

        for (int i = 0; i < 40; i++)
        {
            CreateItem(item[5], CreateRandomPostion(), Quaternion.identity);
        }
    }

    private void CreateItem(GameObject createGameObject, Vector3 createPosition, Quaternion createRotaion)
    {
        GameObject itemGo = Instantiate(createGameObject, createPosition, createRotaion);
        itemGo.transform.SetParent(gameObject.transform);
        _itemPosition.Add(createPosition);
    }

    /**
     * 产生随机位置的方法
     */
    private Vector3 CreateRandomPostion()
    {
        //不生成 x = 13 -13 y= 8 -8的物体

        while (true)
        {
            Vector3 createPosition = new Vector3(Random.Range(-12, 13), Random.Range(-6, 7), 0);

            if (!HasThePosition(createPosition))
            {
                return createPosition;
            }
        }
    }

    private void createEnemy()
    {
        int num = Random.Range(0, 3);
        Vector3 enemyPos = new Vector3();
        if (num == 0)
        {
            enemyPos = new Vector3(13, 7, 0);
        }
        else if (num == 1)
        {
            enemyPos = new Vector3(0, 7, 0);
        }
        else if (num == 2)
        {
            enemyPos = new Vector3(-13, 7, 0);
        }
        
        CreateItem(item[3], enemyPos, Quaternion.identity);
    }

    private bool HasThePosition(Vector3 createPos)
    {
        for (int i = 0; i < _itemPosition.Count; i++)
        {
            if (createPos == _itemPosition[i])
            {
                return true;
            }
        }

        return false;
    }
}