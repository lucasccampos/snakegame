using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction;
    private Vector2 _nextDirection;
    private List<Transform> _segments = new List<Transform>();

    [SerializeField] private Transform segmentPrefab;

    [SerializeField] private int initialSize = 2;

    private float moveTimer;
    [SerializeField] private float minMoveTimer = 0.05f;
    [SerializeField] private float defaultMaxMoveTimer = 0.1f;
    public float moveTimerMax = 0.1f;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.instance;
        GameManager.OnGameStart += ResetState;
    }

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if (_gameManager.isGameOver)
        {
            return;
        }

        HandleInput();
        HandleMoviment();
    }

    private void HandleMoviment()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer < moveTimerMax)
        {
            return;
        }

        moveTimer -= moveTimerMax;

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

    private void HandleInput()
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

    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    public void SetSpeed(float newSpeed)
    {
        this.moveTimerMax = Mathf.Min(newSpeed, minMoveTimer);
    }

    private void ResetState()
    {
        this.moveTimerMax = defaultMaxMoveTimer;
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

    public void Shrink()
    {
        if(_segments.Count == 1){
            _gameManager.EndGame();
            return;
        }

        Transform segment = _segments[_segments.Count - 1].transform;
        _segments.Remove(segment);
        Destroy(segment.gameObject);
    }

    public void Reverse()
    {
        int segmentCount = _segments.Count;

        print(segmentCount);
        this.transform.position = _segments[segmentCount - 1].position + (Vector3)_direction;

        _direction *= -1;
        _segments.Reverse(1, segmentCount - 1);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            _gameManager.EndGame();
        }
        else if (other.CompareTag("Item")){
            other.GetComponent<Item>().ApplyEffect(this);
        }
    }
}
