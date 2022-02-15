using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    
    private SpriteRenderer _spriteRenderer;
    
    public GameObject explosionPrefab;
    public Sprite brokenSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die()
    {
        _spriteRenderer.sprite = brokenSprite;
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        PlayManager.Instance.isDefeat = true;
    }
}
