using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelVideo;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private JoinChannelVideo _joinChannelVideo;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _joinChannelVideo = FindObjectOfType<JoinChannelVideo>();

        if (_joinChannelVideo == null)
        {
            Debug.LogError("JoinChannelVideo script not found in the scene.");
        }
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, moveY, 0));

        Vector2 direction = new Vector2(moveX, moveY).normalized;
        rb.velocity = direction * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            JoinChannel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            JoinChannel();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LeaveChannel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LeaveChannel();
        }
    }

    private void JoinChannel()
    {
        if (_joinChannelVideo != null)
        {
            _joinChannelVideo.JoinChannel();
        }
    }

    private void LeaveChannel()
    {
        if (_joinChannelVideo != null)
        {
            _joinChannelVideo.LeaveChannel();
        }
    }
}
