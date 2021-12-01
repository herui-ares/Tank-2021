using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pubArg
{
    public static float stopTime = 50;
    public static int enemyNum = 0;
    public static float protectTime = 10;
}
public class MapCtration : MonoBehaviour
{
    //装饰初始化地图的数组
    //0老家 1墙 2 障碍 3 出生效果 4 河流 5 草 6 空气墙
    public GameObject[] item;

    //已经有东西的位置列表
    private List<Vector3> itemPositionList = new List<Vector3>();

    GameObject ite1;
    GameObject ite2;
    GameObject ite3;
    GameObject ite4;
    GameObject ite5;
    bool isIns = false;
    void Update()
    {
        //生成保护的家
        if (PropFlag.protect == true && isIns == false)
        {
            ite1 =  Instantiate(item[2], new Vector3(-1, -8, 0), Quaternion.identity);
            ite2 =  Instantiate(item[2], new Vector3(1, -8, 0), Quaternion.identity);
            ite3 =  Instantiate(item[2], new Vector3(-1, -7, 0), Quaternion.identity);
            ite4 =  Instantiate(item[2], new Vector3(0, -7, 0), Quaternion.identity);
            ite5 =  Instantiate(item[2], new Vector3(1, -7, 0), Quaternion.identity);
            isIns = true;
            pubArg.protectTime = 10;
        }
        if(PropFlag.protect == true)
        {
            pubArg.protectTime -= Time.deltaTime;
        }
        
        if(pubArg.protectTime <= 0 && PropFlag.protect == true)
        {
            Destroy(ite1);
            Destroy(ite2);
            Destroy(ite3);
            Destroy(ite4);
            Destroy(ite5);
            Instantiate(item[1], new Vector3(-1, -8, 0), Quaternion.identity);
            Instantiate(item[1], new Vector3(1, -8, 0), Quaternion.identity);
            Instantiate(item[1], new Vector3(-1, -7, 0), Quaternion.identity);
            Instantiate(item[1], new Vector3(0, -7, 0), Quaternion.identity);
            Instantiate(item[1], new Vector3(1, -7, 0), Quaternion.identity);
            pubArg.protectTime = 10;
            isIns = false;
            PropFlag.protect = false;
        }

    }
    private void Awake()
    {
        InitMap();
    }
    private void InitMap()
    {
        //实例化老家
        Instantiate(item[0], new Vector3(0, -8, 0), Quaternion.identity);
        //CreateItem(item[0], new Vector3(0, -8, 0), Quaternion.identity);
        itemPositionList.Add(new Vector3(0, -8, 0));
        //用墙把老家围起来
        //
        CreateItem(item[1], new Vector3(-1, -8, 0), Quaternion.identity);
        itemPositionList.Add(new Vector3(-1, -8, 0));
        CreateItem(item[1], new Vector3(1, -8, 0), Quaternion.identity);
        itemPositionList.Add(new Vector3(1, -8, 0));
        for (int i = -1; i < 2; i++)
        {
            CreateItem(item[1], new Vector3(i, -7, 0), Quaternion.identity);
            itemPositionList.Add(new Vector3(i, -7, 0));
        }
        //实例化外围墙  空气墙
        for (int i = -11; i < 12; i++)
        {
            CreateItem(item[6], new Vector3(i, 9, 0), Quaternion.identity);
        }
        for (int i = -11; i < 12; i++)
        {
            CreateItem(item[6], new Vector3(i, -9, 0), Quaternion.identity);
        }
        for (int i = -8; i < 9; i++)
        {
            CreateItem(item[6], new Vector3(-11, i, 0), Quaternion.identity);

        }
        for (int i = -8; i < 9; i++)
        {
            CreateItem(item[6], new Vector3(11, i, 0), Quaternion.identity);
        }
        //初始化玩家，出生特效
        GameObject go = Instantiate(item[3], new Vector3(-2, -8, 0), Quaternion.identity);
        go.GetComponent<Born>().cratePlayer = true;

        //产生敌人
        CreateItem(item[3], new Vector3(-10, 8, 0), Quaternion.identity);
        CreateItem(item[3], new Vector3(0, 8, 0), Quaternion.identity);
        CreateItem(item[3], new Vector3(8, 8, 0), Quaternion.identity);
        InvokeRepeating("CreateEnemy", 4, 5);
        
        //实例化地图
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPosition = CreateRandomPosition();
            itemPositionList.Add(randomPosition);
            CreateItem(item[1], randomPosition, Quaternion.identity);

            randomPosition = CreateRandomPosition();
            itemPositionList.Add(randomPosition);
            CreateItem(item[1], randomPosition, Quaternion.identity);

            randomPosition = CreateRandomPosition();
            itemPositionList.Add(randomPosition);
            CreateItem(item[1], randomPosition, Quaternion.identity);

            randomPosition = CreateRandomPosition();
            itemPositionList.Add(randomPosition);
            CreateItem(item[2], randomPosition, Quaternion.identity);

            randomPosition = CreateRandomPosition();
            itemPositionList.Add(randomPosition);
            CreateItem(item[4], randomPosition, Quaternion.identity);

            randomPosition = CreateRandomPosition();
            itemPositionList.Add(randomPosition);
            CreateItem(item[5], randomPosition, Quaternion.identity);
        }
    }
    private void CreateItem(GameObject createGameObject, Vector3 createPosition, Quaternion createRotation)
    {
        GameObject itemGo = Instantiate(createGameObject, createPosition, createRotation);
        itemGo.transform.SetParent(gameObject.transform);
    }

    //产生随机位置的方法
    private Vector3 CreateRandomPosition()
    {
        //不生成x=-10,10,的两列，y=-8,8正两行的位置
        while(true)
        {
            Vector3 createPosition = new Vector3(Random.Range(-10, 11), Random.Range(-7, 8), 0);
            if(!HasThePosition(createPosition))
            {
                return createPosition;
            }
        }
        //
    }

    //用来判断位置列表中是否有这个位置
    private bool HasThePosition(Vector3 createPos)
    {
        for(int i = 0; i < itemPositionList.Count;i++)
        {
            if(createPos == itemPositionList[i])
            {
                return true;
            }
        }
        return false;
    }
    private void CreateEnemy()
    {
        if (pubArg.enemyNum <= 8)
        {
            Vector3 EnemyPos = new Vector3();
            int num = Random.Range(0, 3);
            if (num == 0)
            {
                EnemyPos = new Vector3(-10, 8, 0);
            }
            if (num == 1)
            {
                EnemyPos = new Vector3(0, 8, 0);
            }
            if (num == 2)
            {
                EnemyPos = new Vector3(10, 8, 0);
            }
            CreateItem(item[3], EnemyPos, Quaternion.identity);
            pubArg.enemyNum++;
        }
    }
    
}
