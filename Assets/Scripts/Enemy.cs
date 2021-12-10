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

    public int lifeValue;

    private bool award;



    //引用
    //private SpriteRenderer sr;
    //public Sprite[] tankeSprite;//上右下左
    private Animator a_tor;
    public RuntimeAnimatorController[] tankAnimator;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public AudioClip hitAudio;
    public GameObject Prop;
    //计时器
    private float timeVal = 0;
    private float timevalChangeDirection = 0;
    void Start()
    {
        PropFlag.boomFlag = false;
        //sr = GetComponent<SpriteRenderer>();
        a_tor = GetComponent<Animator>();
        num = Random.Range(0, 2);
        if (num == 1 || num == 0)
        {
            award = true;
            ++lifeValue;
        }
       
    }

        // Update is called once per frame
        void Update()
    {

        
        
    }
    private void FixedUpdate()//这里面执行可以避免碰墙抖动
    {
        if (PropFlag.stopFlag == true)
        {
            //Debug.Log("the time" + stopTime);
            pubArg.stopTime -= Time.deltaTime;
            if (pubArg.stopTime <= 0)
            {
                pubArg.stopTime = 50;
                PropFlag.stopFlag = false;
            }     
        }
        Move();
        
        Vector3 curPosition = transform.position;
        if(prePosition == curPosition)
        {
            timevalChangeDirection = 4.0f;
        }
        prePosition = curPosition;

        //攻击的时间间隔
        if (PropFlag.stopFlag == false)
        {
            if (timeVal >= 2f)
            {
                Attack();
            }
            else
            {
                timeVal += Time.deltaTime;
            }
        }  
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
            //Debug.Log("我改变了一下方向");
            if (PropFlag.stopFlag == false)
            {
                num = Random.Range(0, 4);
            }
            if (num == 3)
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

        if (PropFlag.stopFlag == false)
        {
            transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);
        }
            
        if (h < 0)
        {
            //sr.sprite = tankeSprite[3];
            
            if(award == false)
            {
                a_tor.runtimeAnimatorController = tankAnimator[3];
            }
            else
            {
                a_tor.runtimeAnimatorController = tankAnimator[7];
            }
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            //sr.sprite = tankeSprite[1];
            if (award == false)
            {
                a_tor.runtimeAnimatorController = tankAnimator[1];
            }
            else
            {
                a_tor.runtimeAnimatorController = tankAnimator[5];
            }
            bulletEulerAngles = new Vector3(0, 0, -90);
        }
        if (h != 0) return;//

        if (PropFlag.stopFlag == false)
        {
            transform.Translate(Vector3.up * v * moveSpeed * Time.deltaTime, Space.World);
        }

        

        if (v < 0)
        {
            //sr.sprite = tankeSprite[2];
            if (award == false)
            {
                a_tor.runtimeAnimatorController = tankAnimator[2];
            }
            else
            {
                a_tor.runtimeAnimatorController = tankAnimator[6];
            }
            bulletEulerAngles = new Vector3(0, 0, -180);
        }
        else if (v > 0)
        {
            //sr.sprite = tankeSprite[0];
            if (award == false)
            {
                a_tor.runtimeAnimatorController = tankAnimator[0];
            }
            else
            {
                a_tor.runtimeAnimatorController = tankAnimator[4];
            }
            bulletEulerAngles = new Vector3(0, 0, 0);
        }
        
    }
    //
    private void Blend()
    {
        lifeValue--;
        if(award == true)
        {
            award = false;
            float xLocate = Random.Range(-10f, 11f);
            float yLocate = Random.Range(-8, 7);
            Vector3 RandomPosition = new Vector3(xLocate, yLocate, 0);

            Instantiate(Prop, RandomPosition, Quaternion.identity);
        }
        
        if(lifeValue <= 0)
        {
            Die();
        }
        else
        {
            PlayAudio();
        }
    }
    //坦克的死亡方法
    private void Die()
    {
        PlayerManager.Instance.playerScore++;
        //产生爆炸效果
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        //死亡
        if(gameObject.tag == "Enemy1")
        {
            ObjectPool.Instance.Add(ObjectType.Enemy1, gameObject);
            Enemy_List.enemy_List.Remove(gameObject);
        }
        else if (gameObject.tag == "Enemy2")
        {
            ObjectPool.Instance.Add(ObjectType.Enemy2, gameObject);
            Enemy_List.enemy_List.Remove(gameObject);
        }
        else if (gameObject.tag == "Enemy3")
        {
            ObjectPool.Instance.Add(ObjectType.Enemy3, gameObject);
            Enemy_List.enemy_List.Remove(gameObject);
        }
        pubArg.enemyNum--;
        //Debug.Log("the length of enemy_List = " + Enemy_List.enemy_List.Count);
        //Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Barrier" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "River")
        if (collision.gameObject.tag == "Enemy")
        {
            timevalChangeDirection = 4.0f;
        }
    }
    public void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(hitAudio, transform.position);
    }
}

