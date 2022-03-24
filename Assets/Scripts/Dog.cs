using UnityEngine;
using System.Collections;

public class Dog : MonoBehaviour
{
    [SerializeField] private Sprite[] _dogFrames;
    [SerializeField] private Sprite[] _dogDirtFrames;
    [SerializeField] private Sprite[] _dogAngryFrames;
    [SerializeField] private float _normalSpeed = 2f;
    [SerializeField] private float _angrySpeed = 4f;
    [SerializeField] private GameObject _coinPref;
    [SerializeField] private GameObject _fireBonusPref;

    private float _moveSpeed;
    private Player _player;
    private FieldGrid _grid;
    private int _rX = 1;
    private int _rY = 0;
    private float _targetPos;
    private bool _isDead = false;
    private bool _isAngry = false;
    private LevelManager _lm;

    private const float _expireTime = 0.5f;

    private void Awake()
    {
        _moveSpeed = _normalSpeed;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _grid =   GameObject.Find("FieldGrid").GetComponent<FieldGrid>();
        _lm = GameObject.Find("GameManager").GetComponent<LevelManager>();
    }
    private void FixedUpdate()
    {
        _isAngry = CheckPlayer();

        if (!_isDead)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (_isAngry)
        {
            _moveSpeed = _angrySpeed;
        }

        else
        {
            _moveSpeed = _normalSpeed;
        }

        if (_targetPos > 0)
        {
            float x = _rX * _moveSpeed * Time.fixedDeltaTime +
            _rY * (_moveSpeed / 450f); // For illusion of movement in "3d"

            float y = _rY * _moveSpeed * Time.fixedDeltaTime;

            transform.Translate(new Vector2(x, y));

            // So that the Dog hides behind other objects and vice versa
            float z = (-3.5f + transform.position.y) / 7.2f;
            transform.position = new Vector3(transform.position.x, transform.position.y, z);

            _targetPos -= new Vector2(x, y).magnitude;

            MoveAnim(x, y);
        }

        else if (_isAngry)
        {
            float x = 2f * _rX + (_moveSpeed / 450f) * _rY * 2;
            float y = _rY * 1.8f;
            _targetPos = new Vector2(x, y).magnitude;
        }

        else
        {
            RandTurn(3);
        }
    }

    private void RandTurn(int steps = 1)
    {
        var rand = new System.Random();
        int direction = rand.Next(4);

        switch (direction)
        {
            // Left move
            case 0:
                _rX = -1; _rY = 0;
                _targetPos = new Vector2(steps * 2f, 0f).magnitude;
                break;

            // Right move
            case 1:
                _rX = 1; _rY = 0;
                _targetPos = new Vector2(steps * 2f, 0f).magnitude;
                break;

            // Down move
            case 2:
                _rX = 0; _rY = -1;
                _targetPos = new Vector2((_moveSpeed / 450f) * 2 * steps, 1.8f * steps).magnitude;
                break;

            // Up move
            case 3:
                _rX = 0; _rY = 1;
                _targetPos = new Vector2((_moveSpeed / 450f) * 2 * steps, 1.8f * steps).magnitude;
                break;
        }
    }

    private void MoveAnim(float x, float y)
    {
        Sprite[] frames = _dogFrames;

        if (_isAngry)
        {
            frames = _dogAngryFrames;
        }

        // Right
        if (x > 0)
        {
            GetComponent<SpriteRenderer>().sprite = frames[0];
        }

        // Left
        if (x < 0)
        {
            GetComponent<SpriteRenderer>().sprite = frames[1];
        }

        // Up
        if (y < 0)
        {
            GetComponent<SpriteRenderer>().sprite = frames[2];
        }

        // Down
        if (y > 0)
        {
            GetComponent<SpriteRenderer>().sprite = frames[3];
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Level")
        {
            RandTurn(3);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fire")
        {
            StartCoroutine(DeathTimer());
        }
    }

    IEnumerator DeathTimer()
    {
        _isDead = true;

        switch (_rX, _rY)
        {
            case (1, 0):
                GetComponent<SpriteRenderer>().sprite = _dogDirtFrames[0];
                break;

            case (-1, 0):
                GetComponent<SpriteRenderer>().sprite = _dogDirtFrames[1];
                break;

            case (0, -1):
                GetComponent<SpriteRenderer>().sprite = _dogDirtFrames[2];
                break;

            case (0, 1):
                GetComponent<SpriteRenderer>().sprite = _dogDirtFrames[3];
                break;
        }

        yield return new WaitForSeconds(_expireTime);

        int chance = new System.Random().Next(100);

        if (chance < 10)
        {
            Instantiate(_fireBonusPref, transform.position, Quaternion.identity, _lm._items);
        }

        else
        {
            Instantiate(_coinPref, transform.position, Quaternion.identity, _lm._items);
        }

        Destroy(gameObject);
    }

    private bool CheckPlayer()
    {
        Vector2 farmerCell = _grid.GetNearestCell(transform.position.x,
            transform.position.y, "Numbers");

        Vector2 playerCell = _grid.GetNearestCell(_player.transform.position.x,
            _player.transform.position.y, "Numbers");

        if (farmerCell.y == playerCell.y)
        {
            if ((_rX == 1) && (playerCell.x > farmerCell.x) || // Right vision
                (_rX == -1) && (playerCell.x < farmerCell.x))  // Left vision
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        else if (farmerCell.x == playerCell.x)
        {
            if ((_rY == 1) && (playerCell.y < farmerCell.y) ||  // Up vision
                (_rY == -1) && (playerCell.y > farmerCell.y))   // Down vision
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }
}
