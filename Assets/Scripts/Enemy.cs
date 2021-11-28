using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    //属性值
    public float moveSpeed = 3;
    private Vector3 bulletEulerAngles;
    private float v = -1;
    private float h;
    private int num;
    private Vector3 prePosition;



    //引用
    private SpriteRenderer sr;
    public Sprite[] tankeSprite;//上右下左
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    //计时器
    private float timeVal = 0;
    private float timevalChangeDirection = 0;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        
        //攻击的时间间隔
        if (timeVal >= 2f)
        {
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }
    private void FixedUpdate()//这里面执行可以避免碰墙抖动
    {
        Move();
        
        Vector3 curPosition = transform.position;
        if(prePosition == curPosition)
        {
            timevalChangeDirection = 4.0f;
        }
        prePosition = curPosition;
    }

    //坦克的攻击方法
    private void Attack()
    {
            //子弹旋转的角度应该是当前坦克的角度，加上子弹应该旋转的角度
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));//欧拉角转四元数
        timeVal = 0;
    }
    private void Move()
    {
        if(timevalChangeDirection >= 3)
        {
            Debug.Log("我改变了一下方向");
            num = Random.Range(0, 4);
            if(num == 3)
            {
                v = -1;
                h = 0;
            }
            else if(num == 0)
            {
                v = 1; 
                h = 0;
            }
            else if (num == 1)
            {
                v = 0;
                h = -1;
            }
            else if (num == 2)
            {
                v = 0;
                h = 1;
            }
            timevalChangeDirection = 0;
        }
        else
        {
            timevalChangeDirection += Time.fixedDeltaTime;
        }
        
        
        transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);
        if (h < 0)
        {
            sr.sprite = tankeSprite[3];
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            sr.sprite = tankeSprite[1];
            bulletEulerAngles = new Vector3(0, 0, -90);
        }
        if (h != 0) return;//

       
        transform.Translate(Vector3.up * v * moveSpeed * Time.deltaTime, Space.World);

        if (v < 0)
        {
            sr.sprite = tankeSprite[2];
            bulletEulerAngles = new Vector3(0, 0, -180);
        }
        else if (v > 0)
        {
            sr.sprite = tankeSprite[0];
            bulletEulerAngles = new Vector3(0, 0, 0);
        }
        
    }
    //坦克的死亡方法
    private void Die()
    {
        PlayerManager.Instance.playerScore++;
        //产生爆炸效果
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        //死亡
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Barrier" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "River")
        if (collision.gameObject.tag == "Enemy")
        {
            timevalChangeDirection = 4.0f;
            //Debug.Log("我碰到了" + collision.gameObject.tag);
/*
            Debug.Log("我在碰撞里改变了一下方向");
            int randnum = Random.Range(0, 4);
            while (randnum == num)
            {
                randnum = Random.Range(0, 4);
            }
            num = randnum;
            if (num == 3)
            {
                v = -1;
                h = 0;
            }
            else if (num == 0)
            {
                v = 1;
                h = 0;
            }
            else if (num == 1)
            {
                v = 0;
                h = -1;
            }
            else if (num == 2)
            {
                v = 0;
                h = 1;
            }
            timevalChangeDirection = 0;
            */
        }
    }
}

