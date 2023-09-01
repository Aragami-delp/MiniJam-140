using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField, Range(0.1f, 5f)] private float m_speed = .5f;

    private Transform cameraTransform;
    private float textureSizeX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        Sprite sprite =  GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureSizeX = texture.width / sprite.pixelsPerUnit * 10;
    }

    private void LateUpdate()
    {
        transform.position += new Vector3(Time.deltaTime * - m_speed, 0, 0);

        if (Mathf.Abs(transform.position.x) >= textureSizeX)
        {
            float offsetPositionX = (transform.position.x) % textureSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}
