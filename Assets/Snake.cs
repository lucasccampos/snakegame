using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction;
    private Vector2 _nextDirection;
    private List<Transform> _segments = new List<Transform>();

    public Transform segmentPrefab;

    public int initialSize = 2;



    // Start is called before the first frame update
    void Start()
    {
        ResetState();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            _nextDirection = new Vector2(horizontal, 0f);
        }
        else
        {
            float vertical = Input.GetAxisRaw("Vertical");
            if (vertical != 0)
            {
                _nextDirection = new Vector2(0f, vertical);
            }
        }

        if (_direction != _nextDirection && _nextDirection != (_direction * -1))
        {
            _direction = _nextDirection;
        }
    }

    void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0f
        );
    }

    void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    void ResetState()
    {
        _direction = Vector2.right;
        _nextDirection = Vector2.right;

        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        for (int i = 1; i < initialSize; i++)
        {
            Grow();
        }

        this.transform.position = Vector3.zero;
    }

    void Shrink()
    {
        Transform segment = _segments[_segments.Count - 1].transform;
        _segments.Remove(segment);
        Destroy(segment.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
        }
        else if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
        }
    }
}
