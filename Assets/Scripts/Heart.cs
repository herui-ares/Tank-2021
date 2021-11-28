using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer sr;
    public GameObject explosionPrefab;
    public Sprite BrokenSprite;
    public AudioClip dieAudio;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Die()
    {
        PlayerManager.Instance.isDefeat = true;
        sr.sprite = BrokenSprite;
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(dieAudio, transform.position);
    }
}
