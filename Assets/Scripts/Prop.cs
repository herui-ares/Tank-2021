using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PropType 
{ 
    AddLife,
    FixTime,
    Protect,
    Bomb,
    FirePower,
    Cover
}
[System.Serializable]//序列化的标签,去学习一下
public class PropType_Sprite
{
    public PropType type;
    public Sprite sp;
}
public class PropFlag 
{
    public static bool boomFlag = false;
    public static bool stopFlag = false;
    public static bool protect = false;
}
public class Prop : MonoBehaviour
{

    public GameObject barrierPrefab;
    public PropType_Sprite[] propType_Sprites;
    private SpriteRenderer spriteRenderer;
    private PropType propType;
    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        int index = Random.Range(0, propType_Sprites.Length);
        spriteRenderer.sprite = propType_Sprites[index].sp;
        propType = propType_Sprites[index].type;
       // Debug.Log("this is" + propType);
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tank")
        {
            
            switch (propType)
            {
                case PropType.AddLife:
                    AddLife();
                    Debug.Log("this is " + propType);
                    Destroy(gameObject);
                    break;
                case PropType.FixTime:
                    PropFlag.stopFlag = true;
                    
                    Debug.Log("this is " + propType);
                    Debug.Log("the stopFlag is " + PropFlag.stopFlag);
                    Destroy(gameObject);
                    break;
                case PropType.Protect:
                    PropFlag.protect = true;
                    Debug.Log("this is " + propType);
                    Destroy(gameObject);
                    break;
                case PropType.Bomb:
                    PropFlag.boomFlag = true;
                    Debug.Log("this is " + propType);
                    Destroy(gameObject);
                    break;
                case PropType.FirePower:
                    collision.SendMessage("FirePower");
                    Debug.Log("this is " + propType);
                    Destroy(gameObject);
                    break;
                case PropType.Cover:
                    collision.SendMessage("Cover");
                    Debug.Log("this is" + propType);
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }

    }
    private void AddLife()
    {
        //这里是前面的单例模式，多了解一下
        PlayerManager.Instance.isLive = true;
    }
    private void FixTime()
    {

    }
    
    private void CreateItem(GameObject createGameObject, Vector3 createPosition, Quaternion createRotation)
    {
        GameObject itemGo = Instantiate(createGameObject, createPosition, createRotation);
    }
    private void Bomb()
    {

    }
    private void FirePower()
    {

    }
    private void Cover()
    {

    }


}
