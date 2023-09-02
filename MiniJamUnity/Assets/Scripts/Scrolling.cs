using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    [SerializeField, Range(0.1f, 100f)] private float m_speed = .5f;
    [SerializeField] private bool m_moveRight = true;
    [SerializeField] private SpriteRenderer m_background;

    private Transform cameraTransform;
    private float textureSizeX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        Sprite sprite =  m_background.sprite;
        Texture2D texture = sprite.texture;
        textureSizeX = sprite.texture.width / sprite.pixelsPerUnit * 10;
    }

    private void LateUpdate()
    {
        transform.position += new Vector3(Time.deltaTime * (m_moveRight ? -1 : 1) * m_speed, 0, 0);

        if (Mathf.Abs(transform.position.x) >= textureSizeX)
        {
            float offsetPositionX = (transform.position.x) % textureSizeX;
            m_background.transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}
