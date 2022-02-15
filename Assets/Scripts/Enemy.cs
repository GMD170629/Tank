using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3;
    private Vector3 _bulletEulerAngles;
    // private float _defenedTimeVal = 3;
    private float _timeVal;
    private float _timeValChangeDeriction;

    private float _v = -1;
    private float _h;
    
    // private bool _isDefended = true;

    private SpriteRenderer _spriteRenderer;

    public Sprite[] tankSprite;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    // public GameObject defenedEffectPrefab;

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
        // Defened();

        //攻击的时间间隔
        if (_timeVal >= 3)
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
        Move();
    }

    private void Attack()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + _bulletEulerAngles));
        _timeVal = 0;
    }

    /**
     * 坦克移动
     */
    private void Move()
    {

        if (_timeValChangeDeriction >= 4)
        {
            int num = Random.Range(0, 8);

            if (num >= 5)
            {
                _v = -1;
                _h = 0;
            }else if (num == 0)
            {
                _v = 1;
                _h = 0;
            }else if (num >= 1 && num <= 2) {
                _v = 0;
                _h = 1;
            }else{
                _v = 0;
                _h = -1;
            }

            _timeValChangeDeriction = 0;
        }
        else
        {
            _timeValChangeDeriction += Time.deltaTime;
        }

        transform.Translate(Vector3.right * _h * moveSpeed * Time.deltaTime, Space.World);

        if (_h < 0)
        {
            _spriteRenderer.sprite = tankSprite[3];
            _bulletEulerAngles = new Vector3(0, 0, 90);
        }
        else if (_h > 0)
        {
            _spriteRenderer.sprite = tankSprite[1];
            _bulletEulerAngles = new Vector3(0, 0, -90);
        }

        if (_h != 0)
        {
            return;
        }

        transform.Translate(Vector3.up * _v * moveSpeed * Time.deltaTime, Space.World);

        if (_v < 0)
        {
            _spriteRenderer.sprite = tankSprite[2];
            _bulletEulerAngles = new Vector3(0, 0, 180);
        }
        else if (_v > 0)
        {
            _spriteRenderer.sprite = tankSprite[0];
            _bulletEulerAngles = new Vector3(0, 0, 0);
        }
    }

    /**
     * 坦克的死亡
     */
    private void Die()
    {
        // if (_isDefended)
        // {
        //     return;
        // }

        PlayManager.Instance.playerScore++;
        
        //产生爆炸特效 
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        //死亡
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Barrier") || col.gameObject.CompareTag("Water"))
        {
            _timeValChangeDeriction = 4;
        }
    }
    // public void Defened()
    // {
    //     if (_isDefended)
    //     {
    //         defenedEffectPrefab.SetActive(true);
    //         _defenedTimeVal -= Time.deltaTime;
    //
    //         if (_defenedTimeVal <= 0)
    //         {
    //             _isDefended = false;
    //             defenedEffectPrefab.SetActive(false);
    //         }
    //     }
    // }
    
}
