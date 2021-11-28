using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    //属性值
    public int lifeValue = 3;
    public int playerScore = 0;
    public bool isDead = false;
    public bool isDefeat = false;
    //引用
    public GameObject born;
    public Text PlayerScoreText;
    public Text PlayerLifeValueText;
    public GameObject isDefeatUI;

    //单例
    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDefeat == true)
        {
            isDefeatUI.SetActive(true);
            Invoke("ReturnToTheMainMenu", 3);
            return;
        }
        if(isDead)
        {
            Recover();
            
        }
            
        PlayerScoreText.text = playerScore.ToString();
        PlayerLifeValueText.text = lifeValue.ToString();
    }
    private void Recover()
    {
        if(lifeValue <= 0)
        {
            //游戏失败
            isDefeat = true;
            
        }
        else
        {
            lifeValue--;
            GameObject go = Instantiate(born, new Vector3(-2, -8, 0), Quaternion.identity);
            go.GetComponent<Born>().cratePlayer = true;
            isDead = false;
        }
    }
    private void ReturnToTheMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
