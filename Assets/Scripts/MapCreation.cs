using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreation : MonoBehaviour
{

    //用来装饰初始化地图所需物体的数组
    //0.老家 1.墙 2.障碍 3.出生效果 4.水 5.草 6.空气墙
    public GameObject[] item;
    
    private void Awake()
    {
        //实例化老家
        CreateItem(item[0], new Vector3(0, -8, 0), Quaternion.identity);
        //用墙把老家围起来
        CreateItem(item[1], new Vector3(-1, -8, 0), Quaternion.identity);
        CreateItem(item[1], new Vector3(1, -8, 0), Quaternion.identity);
        for (int i = -1; i < 2; i++)
        {
            CreateItem(item[1], new Vector3(i, -7, 0), Quaternion.identity);
        }
        
    }

    private void CreateItem(GameObject createGameObject, Vector3 createPosition, Quaternion createRotaion)
    {
        GameObject itemGo = Instantiate(createGameObject, createPosition, createRotaion);
        itemGo.transform.SetParent(gameObject.transform);
    }
    
}
