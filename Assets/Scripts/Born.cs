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
            Instantiate(enemyPrefabList[num], transform.position, transform.rotation);
            //Debug.Log("我正在生成");
            //ObjectPool.Instance.Get(transform.position, transform.rotation);
           // ObjectPool.Instance.Get
            
        }
        
    }
}
