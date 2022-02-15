using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3;
    private Vector3 _bulletEulerAngles;
    private float _defenedTimeVal = 3;
    private float _timeVal;
    private bool _isDefended = true;
    
    private SpriteRenderer _spriteRenderer;

    public Sprite[] tankSprite;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject defenedEffectPrefab;

    public AudioSource moveAudio;
    public AudioClip[] tankAudio;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //保护罩
        Defened();
        
        // Attack();
        if (PlayManager.Instance.isDefeat)
        {
            return;
        }
        
        //攻击的CD
        if (_timeVal >= 0.4f)
        {
            Attack();
        }
        else
        {
            _timeVal += Time.deltaTime;
        }
        
    }

    private void FixedUpdate()
    {
        // Attack();

        if (PlayManager.Instance.isDefeat)
        {
            return;
        }

        Move();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + _bulletEulerAngles));
            _timeVal = 0;
        }
    }

    /**
     * 坦克移动
     */
    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);

        if (h < 0)
        {
            _spriteRenderer.sprite = tankSprite[3];
            _bulletEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            _spriteRenderer.sprite = tankSprite[1];
            _bulletEulerAngles = new Vector3(0, 0, -90);
        }

        if (Mathf.Abs(h) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        
        if (h != 0)
        {
            return;
        }

        float v = Input.GetAxisRaw("Vertical");
        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);

        if (v < 0)
        {
            _spriteRenderer.sprite = tankSprite[2];
            _bulletEulerAngles = new Vector3(0, 0, 180);
        }
        else if (v > 0)
        {
            _spriteRenderer.sprite = tankSprite[0];
            _bulletEulerAngles = new Vector3(0, 0, 0);
        }
        

        if (Mathf.Abs(v) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
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

    /**
     * 坦克的死亡
     */
    private void Die()
    {
        if (_isDefended)
        {
            return;
        }

        //产生爆炸特效 
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        //死亡
        Destroy(gameObject);
        
        //减少生命并重生
        PlayManager.Instance.isDead = true;
    }

    public void Defened()
    {
        if (_isDefended)
        {
            defenedEffectPrefab.SetActive(true);
            _defenedTimeVal -= Time.deltaTime;

            if (_defenedTimeVal <= 0)
            {
                _isDefended = false;
                defenedEffectPrefab.SetActive(false);
            }
        }
    }
}