using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    AirBarrier,
    Barrier,
    Grass,
    River,
    Wall,
    Prop,
    Born
}

[System.Serializable] //加这个标签，类对应的列表才能看到
public class Type_Prefab
{
    public ObjectType type;
    public GameObject prefab;
}
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    //public Dictionary<ObjectType, GameObject> keyValuePairs = new Dictionary<ObjectType, GameObject>();//字典不能序列化出来，unity看不到
    public List<Type_Prefab> typePrefabs = new List<Type_Prefab>();  //通过类和链表实现
    private Dictionary<ObjectType, List<GameObject>> dic = new Dictionary<ObjectType, List<GameObject>>();

    //可以先在这里面创建一些对象 
    private void Awake()
    {
        Instance = this;
    }
    private GameObject GetPreByType(ObjectType type)
    {
        foreach (var item in typePrefabs)
        {
            if (item.type == type)
            {
                return item.prefab;
            }

        }
        return null;
    }
    //通过物体类型，从相应的对象池取
    public GameObject Get(ObjectType type, Vector3 pos, Quaternion rot)
    {
        GameObject temp = null;
        //判断字典中有没有与该类型匹配的对象池，没有则创建
        if (dic.ContainsKey(type) == false)
        {
            dic.Add(type, new List<GameObject>());
        }
        //判断该类型对象池中有没有物体
        if (dic[type].Count > 0)
        {
            int index = dic[type].Count - 1;
            temp = dic[type][index];
            dic[type].RemoveAt(index);
        }
        else
        {
            GameObject pre = GetPreByType(type);
            if (pre != null)
            {
                temp = Instantiate(pre, pos, rot);
            }

        }
        temp.transform.position = pos;
        temp.transform.rotation = rot;
        return temp;
    }

    public void Add(ObjectType type, GameObject go)
    {
        //判断该类型是否有对应的对象池以及对象池中不存在该物体
        if (dic.ContainsKey(type) && dic[type].Contains(go) == false)
        {
            dic[type].Add(go);
        }
        go.SetActive(false);//隐藏
    }
}
