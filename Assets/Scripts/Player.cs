using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3;
    private Vector3 _bulletEulerAngles;
    private float _defenedTimeVal = 3;
    private float _timeVal;
    private bool _isDefended = true;
    private bool _isMove;

    private int _hKeyFrame = 0;
    private int _vKeyFrame = 0;

    private Animator _animator;

    public AnimatorController[] tankAnimatorControllers;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject defenedEffectPrefab;

    public AudioSource moveAudio;
    public AudioClip[] tankAudio;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
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

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0)
        {
            _hKeyFrame++;
        }
        else
        {
            _hKeyFrame = 0;
        }

        if (v != 0)
        {
            _vKeyFrame++;
        }
        else
        {
            _vKeyFrame = 0;
        }

        int direction = 4;
        if (h > 0)
        {
            direction = 1;
        }
        else if (h < 0)
        {
            direction = 3;
        }

        if (v > 0)
        {
            if ((Mathf.Abs(h) > 0 && _vKeyFrame < _hKeyFrame) || h == 0)
            {
                direction = 0;
            }
        }
        else if (v < 0)
        {
            if ((Mathf.Abs(h) > 0 && _vKeyFrame < _hKeyFrame) || h == 0)
            {
                direction = 2;
            }
        }

        if (direction <= 3)
        {
            Move(direction);
        }
        else
        {
            _isMove = false;
        }

        MoveState();
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
    private void Move(int dirction)
    {
        _isMove = true;
        switch (dirction)
        {
            case 0: //向上移动
                MoveUp(1f);
                break;
            case 1:
                MoveRight(1f);
                break;
            case 2:
                MoveDown(1f);
                break;
            case 3:
                MoveLeft(1f);
                break;
        }
    }


    private void MoveUp(float distance)
    {
        //控制行走动画
        _animator.runtimeAnimatorController = tankAnimatorControllers[0];
        //控制子弹旋转
        _bulletEulerAngles = new Vector3(0, 0, 0);
        //移动
        transform.Translate(Vector3.up * distance * moveSpeed * Time.fixedDeltaTime, Space.World);
    }

    private void MoveRight(float distance)
    {
        //控制行走动画
        _animator.runtimeAnimatorController = tankAnimatorControllers[1];
        //控制子弹旋转
        _bulletEulerAngles = new Vector3(0, 0, -90);
        //移动
        transform.Translate(Vector3.right * distance * moveSpeed * Time.fixedDeltaTime, Space.World);
    }

    private void MoveDown(float distance)
    {
        //控制行走动画
        _animator.runtimeAnimatorController = tankAnimatorControllers[2];
        //控制子弹旋转
        _bulletEulerAngles = new Vector3(0, 0, -180);
        //移动
        transform.Translate(Vector3.up * -distance * moveSpeed * Time.fixedDeltaTime, Space.World);
    }

    private void MoveLeft(float distance)
    {
        //控制行走动画
        _animator.runtimeAnimatorController = tankAnimatorControllers[3];
        //控制子弹旋转
        _bulletEulerAngles = new Vector3(0, 0, 90);
        //移动
        transform.Translate(Vector3.right * -distance * moveSpeed * Time.fixedDeltaTime, Space.World);
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

    public void MoveState()
    {
        if (_isMove)
        {
            _animator.speed = 1;
        }
        else
        {
            _animator.speed = 0;
        }
    }
}