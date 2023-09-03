using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    [SerializeField, Range(0.1f, 100f)] private float m_speed = 4f;
    [SerializeField] private float m_speedIncrease = .5f;
    [SerializeField] private bool m_moveRight = true;
    [SerializeField] private SpriteRenderer m_background;
    [SerializeField] private Transform m_resetingTransform;

    [SerializeField] private bool lerpSpeed;
    [SerializeField] private float lerpTarget;
    float lerpTime = 0;

    private Transform cameraTransform;
    private float textureSizeX;

    public static Scrolling Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        
        Instance = this;
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        Sprite sprite = m_background.sprite;
        Texture2D texture = sprite.texture;
        textureSizeX = sprite.texture.width / sprite.pixelsPerUnit * 10;
    }

    private void Update()
    {
        if (lerpSpeed) 
        {
            lerpTime += Time.deltaTime;

            m_speed = Mathf.Lerp(m_speed,lerpTarget,lerpTime);

            if (lerpTime > 1) 
            {
                lerpSpeed = false;
                lerpTime = 0;
                lerpTarget = 0;
            }
        }
    }

    private void LateUpdate()
    {
        transform.position += new Vector3(Time.deltaTime * (m_moveRight ? -1 : 1) * m_speed, 0, 0);

        if (Mathf.Abs(transform.position.x) >= textureSizeX)
        {
            float offsetPositionX = (m_resetingTransform.position.x) % textureSizeX;
            m_resetingTransform.position = new Vector3(cameraTransform.position.x + offsetPositionX, m_resetingTransform.position.y);
        }
    }

    public void SpeedUp()
    {
        m_speed += m_speedIncrease;
    }

    public void SetSpeed(float newSpeed) 
    {
        lerpSpeed = true;
        lerpTarget = newSpeed;
    }

    public float GetSpeed()
    {
        return m_speed;
    }
}
