using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    //属性值
    public float moveSpeed = 3;
    private Vector3 bulletEulerAngles;
    private float timeVal = 0;
    private float defendTime = 3;
    private bool isDefended = true;


    //引用
    //private SpriteRenderer sr;
    //public Sprite[] tankeSprite;//上右下左
    private Animator a_tor;
    public RuntimeAnimatorController[] tankAnimator;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject defendEffectPrefab;
    public AudioSource moveAudio;
    public AudioClip[] tankAudio;
    
    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        a_tor = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //是否处于无敌状态
        if(isDefended)
        {
            defendEffectPrefab.SetActive(true);
            defendTime -= Time.deltaTime;
            if(defendTime <= 0)
            {
                isDefended = false;
                defendEffectPrefab.SetActive(false);
            }
        }
        //攻击的CD
        if (timeVal >= 0.4f)
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
        if(PlayerManager.Instance.isDefeat)
        {
            return;
        }

        
        Move();
    }

    //坦克的攻击方法
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //子弹旋转的角度应该是当前坦克的角度，加上子弹应该旋转的角度
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));//欧拉角转四元数
            timeVal = 0;
        }
    }
    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal"); //判断水平的按键，左右或AD  右 h为正，左h为负
        transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);
        if (h < 0)
        {
            //sr.sprite = tankeSprite[3];
            a_tor.runtimeAnimatorController = tankAnimator[3];
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            //sr.sprite = tankeSprite[1];
            a_tor.runtimeAnimatorController = tankAnimator[1];
            bulletEulerAngles = new Vector3(0, 0, -90);
        }
        if (h != 0)
        {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        if (h != 0) return;//
        

        float v = Input.GetAxisRaw("Vertical");//判断竖直的案件，上下或WS
        transform.Translate(Vector3.up * v * moveSpeed * Time.deltaTime, Space.World);

        if (v < 0)
        {
            //sr.sprite = tankeSprite[2];
            a_tor.runtimeAnimatorController = tankAnimator[2];
            bulletEulerAngles = new Vector3(0, 0, -180);
        }
        else if (v > 0)
        {
            //sr.sprite = tankeSprite[0];
            a_tor.runtimeAnimatorController = tankAnimator[0];
            bulletEulerAngles = new Vector3(0, 0, 0);
        }
        if(v != 0)
        {
            moveAudio.clip = tankAudio[1];
            if(!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.clip = tankAudio[0];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
    }
    //坦克的死亡方法
    private void Die()
    {
        if(isDefended)
        {
            return;
        }
        //
        PlayerManager.Instance.isDead = true;
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        //死亡
        Destroy(gameObject);
    }
}
