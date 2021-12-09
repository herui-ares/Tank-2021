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
    public GameObject Heart;
    //已经有东西的位置列表
    private List<Vector3> itemPositionList = new List<Vector3>();

    private Dictionary<ObjectType, List<GameObject>> PoolObjectDic = new Dictionary<ObjectType, List<GameObject>>();
    
    void Update()
    {
        //生成保护的家
        if (PropFlag.protect == true)
        {
            PropFlag.protect = false;
            Destroy(InsAndLoc(ObjectType.Barrier, new Vector3(-1, -8, 0)), 5);
            Destroy(InsAndLoc(ObjectType.Barrier, new Vector3(1, -8, 0)), 5);
            for (int i = -1; i < 2; i++)
            {
                
                Destroy(InsAndLoc(ObjectType.Barrier, new Vector3(i, -7, 0)), 5);
            }
            Invoke("CreatHome", 5);
        }
    }
    private void Awake()
    {
        InitMap();
    }
    private void InitMap()
    {
        //实例化老家
        CreateItem(Heart, new Vector3(0, -8, 0), Quaternion.identity);
        itemPositionList.Add(new Vector3(0, -8, 0));
        //用墙把老家围起来
       // CreatHome();
        //实例化外围墙  空气墙
        for (int i = -11; i < 12; i++)
        {
            InsAndLoc(ObjectType.AirBarrier, new Vector3(i, 9, 0));
        }
        for (int i = -11; i < 12; i++)
        {
            InsAndLoc(ObjectType.AirBarrier, new Vector3(i, -9, 0));
        }
        for (int i = -8; i < 9; i++)
        {
            InsAndLoc(ObjectType.AirBarrier, new Vector3(-11, i, 0));
        }
        for (int i = -8; i < 9; i++)
        {
            InsAndLoc(ObjectType.AirBarrier, new Vector3(11, i, 0));
        }
        //初始化玩家，出生特效
        
        GameObject go = InsAndLoc(ObjectType.Born, new Vector3(-2, -8, 0)); ;
        go.GetComponent<Born>().cratePlayer = true;

        //产生敌人
        InsAndLoc(ObjectType.Born, new Vector3(-10, 8, 0));
        InsAndLoc(ObjectType.Born, new Vector3(0, 8, 0));
        InsAndLoc(ObjectType.Born, new Vector3(8, 8, 0));
        InvokeRepeating("CreateEnemy", 4, 5);
        
        //实例化地图
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPosition = CreateRandomPosition();
            InsAndLoc(ObjectType.Wall, randomPosition);

            randomPosition = CreateRandomPosition();
            InsAndLoc(ObjectType.Wall, randomPosition);


            randomPosition = CreateRandomPosition();
            InsAndLoc(ObjectType.Wall, randomPosition);


            randomPosition = CreateRandomPosition();
            InsAndLoc(ObjectType.Barrier, randomPosition);


            randomPosition = CreateRandomPosition();
            InsAndLoc(ObjectType.River, randomPosition);


            randomPosition = CreateRandomPosition();
            InsAndLoc(ObjectType.Grass, randomPosition);

        }
    }
    private void CreateItem(GameObject createGameObject, Vector3 createPosition, Quaternion createRotation)
    {
        GameObject itemGo = Instantiate(createGameObject, createPosition, createRotation);
        itemGo.transform.SetParent(gameObject.transform);
    }

    //实例化并添加进位置列表
    private GameObject InsAndLoc(ObjectType type, Vector3 pos)
    {
        GameObject obj = ObjectPool.Instance.Get(type, pos, Quaternion.identity);
        obj.transform.SetParent(gameObject.transform);
        if (!itemPositionList.Contains(pos))
        {
            itemPositionList.Add(pos);
        }
        if (PoolObjectDic.ContainsKey(type) == false)
        {
            PoolObjectDic.Add(type, new List<GameObject>());
        }
        PoolObjectDic[type].Add(obj);
        return obj;
    }
    // 实例化老家的围墙
    private void CreatHome()
    {
        //用墙把老家围起来
        InsAndLoc(ObjectType.Wall, new Vector3(-1, -8, 0));
        InsAndLoc(ObjectType.Wall, new Vector3(1, -8, 0));
        for (int i = -1; i < 2; i++)
        {
            InsAndLoc(ObjectType.Wall, new Vector3(i, -7, 0));
        }
        
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
            InsAndLoc(ObjectType.Born, EnemyPos);
            pubArg.enemyNum++;
        }
    }
}
