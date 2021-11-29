using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    //属性值
    public float moveSpeed = 3;
    private Vector3 bulletEulerAngles;
    private float v = -1;
    private float h;
    private int num;
    private Vector3 prePosition;

    public int flag; //0 代表普通  1代表黄色   2 代表绿色
    public int lifeValue = 1;
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
        //sr = GetComponent<SpriteRenderer>();
        a_tor = GetComponent<Animator>();
        flag = Random.Range(0, 3);
        lifeValue += flag;
        num = Random.Range(0, 2);
        if (num == 1)
        {
            award = true;
            ++lifeValue;
        }

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
        if (prePosition == curPosition)
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
        if (timevalChangeDirection >= 3)
        {
            Debug.Log("我改变了一下方向");
            num = Random.Range(0, 4);
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
        }
        else
        {
            timevalChangeDirection += Time.fixedDeltaTime;
        }


        transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);
        if (h < 0)
        {
            //sr.sprite = tankeSprite[3];

            if (award == false)
            {
                if(flag == 0)
                {
                    a_tor.runtimeAnimatorController = tankAnimator[3];
                }
                else if(flag == 1)
                {
                    a_tor.runtimeAnimatorController = tankAnimator[7];
                }
                else
                {
                    a_tor.runtimeAnimatorController = tankAnimator[11];
                }
                
            }
            else
            {
                a_tor.runtimeAnimatorController = tankAnimator[15];
            }
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            //sr.sprite = tankeSprite[1];
            if (award == false)
            {
                if (flag == 0)
                {
                    a_tor.runtimeAnimatorController = tankAnimator[1];
                }
                else if (flag == 1)
                {
                    a_tor.runtimeAnimatorController = tankAnimator[5];
                }
                else
                {
                    a_tor.runtimeAnimatorController = tankAnimator[9];
                }
            }
            else
            {
                a_tor.runtimeAnimatorController = tankAnimator[13];
            }
            bulletEulerAngles = new Vector3(0, 0, -90);
        }
        if (h != 0) return;//


        transform.Translate(Vector3.up * v * moveSpeed * Time.deltaTime, Space.World);

        if (v < 0)
        {
            //sr.sprite = tankeSprite[2];
            if (award == false)
            {
                if (flag == 0)
                {
                    a_tor.runtimeAnimatorController = tankAnimator[2];
                }
                else if (flag == 1)
                {
                    a_tor.runtimeAnimatorController = tankAnimator[6];
                }
                else
                {
                    a_tor.runtimeAnimatorController = tankAnimator[10];
                }
            }
            else
            {
                a_tor.runtimeAnimatorController = tankAnimator[14];
            }
            bulletEulerAngles = new Vector3(0, 0, -180);
        }
        else if (v > 0)
        {
            //sr.sprite = tankeSprite[0];
            if (award == false)
            {
                if (flag == 0)
                {
                    a_tor.runtimeAnimatorController = tankAnimator[0];
                }
                else if (flag == 1)
                {
                    a_tor.runtimeAnimatorController = tankAnimator[4];
                }
                else
                {
                    a_tor.runtimeAnimatorController = tankAnimator[8];
                }
            }
            else
            {
                a_tor.runtimeAnimatorController = tankAnimator[12];
            }
            bulletEulerAngles = new Vector3(0, 0, 0);
        }

    }
    //
    private void Blend()
    {
        lifeValue--;
        //flag--;
        if(award == true)
        {
           // while (true)
           // {
                Vector3 RandomPosition = new Vector3(Random.Range(-10, 11), Random.Range(-8, 8), 0);
            
               // if (RodomPosition != new Vector3(0, -8, 0))
               // {
                  //  break;
               // }
            //}
            Instantiate(Prop, RandomPosition, Quaternion.identity);
            award = false;
        }
        else
        {
            flag = Mathf.Max(--flag, 0);
        }
        
        Debug.Log("the flag = " + flag);
        if (lifeValue <= 0)
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
        Destroy(gameObject);
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

