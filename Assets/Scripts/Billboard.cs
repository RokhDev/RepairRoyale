using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Sprite[] sprites;
    public float sizeVariance;
    public float sizeChangeSpeed;

    private Transform cameraTransform;
    private bool shrinking = false;
    private float minSize;
    private float maxSize;
    private SpriteRenderer sr;
    private Vector3 originalLocalScale;

    private void Awake()
    {
        originalLocalScale = transform.localScale;
        minSize = transform.localScale.x - sizeVariance;
        maxSize = transform.localScale.x + sizeVariance;
    }

    private void Start()
    {
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
    }

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        int spriteIdx = Random.Range(0, sprites.Length);
        sr.sprite = sprites[spriteIdx];
        transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        shrinking = false;
    }

    private void Update()
    {
        transform.LookAt(cameraTransform);

        if (shrinking)
        {
            transform.localScale = new Vector3(transform.localScale.x - sizeChangeSpeed * Time.deltaTime,
                                               transform.localScale.y - sizeChangeSpeed * Time.deltaTime,
                                               transform.localScale.z - sizeChangeSpeed * Time.deltaTime);
            if (transform.localScale.x < minSize) { shrinking = false; }
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x + sizeChangeSpeed * Time.deltaTime,
                                               transform.localScale.y + sizeChangeSpeed * Time.deltaTime,
                                               transform.localScale.z + sizeChangeSpeed * Time.deltaTime);
            if (transform.localScale.x > maxSize) { shrinking = true; }
        }
    }
}
