using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Born : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerPrefab;
    public GameObject[] enemyPrefabList;
    public bool cratePlayer;
    void Start()
    {
        Invoke("BornTank", 0.8f);
        Destroy(gameObject, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void BornTank()
    {
        if(cratePlayer)
        {
            Instantiate(playerPrefab, transform.position, transform.rotation);//这里教程用Quaternion.indentity
        }
        else
        {
            
            int num = Random.Range(0, 3);
            //Instantiate(enemyPrefabList[num], transform.position, transform.rotation);
            //ObjectPool.Instance.Get(ObjectType.Enemy1, transform.position, transform.rotation);
            if (num == 0)
            {
                GameObject enemy = ObjectPool.Instance.Get(ObjectType.Enemy1, transform.position, transform.rotation);
                Enemy_List.enemy_List.Add(enemy);
            }
            else if(num == 1)
            {
                GameObject enemy = ObjectPool.Instance.Get(ObjectType.Enemy2, transform.position, transform.rotation);
                Enemy_List.enemy_List.Add(enemy);
            }
            else if (num == 2)
            {
                GameObject enemy = ObjectPool.Instance.Get(ObjectType.Enemy3, transform.position, transform.rotation);
                Enemy_List.enemy_List.Add(enemy);
            }
            //Debug.Log("the length of enemy_List = " + Enemy_List.enemy_List.Count);
        }
        
    }
}
