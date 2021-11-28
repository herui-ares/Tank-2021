using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 10;
    public bool isPlayerBullet;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World); //世界坐标和up(UP是Y轴，right是X轴)需要多了解一下

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Tank":
                if(!isPlayerBullet)
                {
                    collision.SendMessage("Die");
                    Destroy(gameObject);
                }
                
                break;
            case "Heart":
                collision.SendMessage("Die");//这里是碰撞体的方法，也就是和Tank的Die是两个不同的
                Destroy(gameObject);
                break;
            case "Enemy": 
                if(isPlayerBullet)
                {
                    collision.SendMessage("Blend");
                    Destroy(gameObject);
                }
                
                break;
            case "Wall":
                if (isPlayerBullet)
                {
                    collision.SendMessage("PlayAudio");
                }
                Destroy(collision.gameObject);
                Destroy(gameObject);
                break;
            case "Barrier":
                if(isPlayerBullet)
                {
                    collision.SendMessage("PlayAudio");
                }
                
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
