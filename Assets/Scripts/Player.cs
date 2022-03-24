using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private FixedJoystick   _joystick;
    [SerializeField] private Sprite[]        _pigFrames;
    [SerializeField] private Button          _bombBtn;
    [SerializeField] private GameObject      _bombPref;
    [SerializeField] private FieldGrid       _grid;
    [SerializeField] private float           _moveSpeed;
    [SerializeField] private int             _maxHealth = 3;
    [SerializeField] private int             _health = 3;
    [SerializeField] private Text            _helthIndicator;
    [SerializeField] private int             _score;
    [SerializeField] private Text            _scoreIndicator;
    [SerializeField] private GameObject      _mainMenuUI;
    [SerializeField] private GameObject      _gameUI;
    [SerializeField] private GameObject      _gameManager;
    
    private bool _isDamaged;
    private LevelManager _lm;

    private const float _invulnerabilityTime = 1f;
    private float _explosionArea = 1;

    private void Awake()
    {
        _lm = _gameManager.GetComponent<LevelManager>();
        _bombBtn.onClick.AddListener(SpawnBomb);
        Debug.Log(_explosionArea);
    }

    private void Update()
    {
        CheckHealth();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float x = _joystick.Horizontal * _moveSpeed * Time.fixedDeltaTime +
            _joystick.Vertical * (_moveSpeed / 500f); // For illusion of movement in "3d"

        float y = _joystick.Vertical * _moveSpeed * Time.fixedDeltaTime;

        transform.Translate(new Vector2(x, y));

        // So that the Pig hides behind other objects and vice versa
        float z = (-3.5f + transform.position.y) / 7.2f;
        transform.position = new Vector3(transform.position.x, transform.position.y, z);

        MoveAnim(x, y);
    }

    private void MoveAnim(float x, float y)
    {
        // Right
        if (x > 0)
        {
            GetComponent<SpriteRenderer>().sprite = _pigFrames[0];
        }

        // Left
        if (x < 0)
        {
            GetComponent<SpriteRenderer>().sprite = _pigFrames[1];
        }

        // Up
        if (y < 0)
        {
            GetComponent<SpriteRenderer>().sprite = _pigFrames[2];
        }

        // Down
        if (y > 0)
        {
            GetComponent<SpriteRenderer>().sprite = _pigFrames[3];
        }
    }

    private void SpawnBomb()
    {
        Vector2 nearestCell = _grid.GetNearestCell(transform.position.x, transform.position.y);
        GameObject bomb = Instantiate(_bombPref, 
            new Vector3(nearestCell.x, nearestCell.y, transform.position.z), Quaternion.identity);

        bomb.GetComponent<Bomb>().BombInit((int)_explosionArea);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Dog") || 
            (collision.gameObject.tag == "Farmer"))
        {
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), collision.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "FireBonus")
        {
            _explosionArea += 0.5f;

            if (other.gameObject != null)
            {
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.tag == "Coin")
        {
            _score += 5;
            _scoreIndicator.text = _score.ToString();

            if (other.gameObject != null)
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Dog") || 
            (collision.gameObject.tag == "Farmer") ||
            (collision.gameObject.tag == "Fire"))
        {
            if (!_isDamaged)
            {
                _isDamaged = true;
                TakeDamage();
                StartCoroutine(InvulnerabilityTimer());
            }
        }
    }

    private void TakeDamage()
    {
        _health--;
        _helthIndicator.text = _health.ToString();
    }

    IEnumerator InvulnerabilityTimer()
    {
        yield return new WaitForSeconds(_invulnerabilityTime);
        _isDamaged = false;
    }

    private void CheckHealth()
    {
        if (_health <= 0)
        {
            _health = _maxHealth;
            _helthIndicator.text = _health.ToString();
            _explosionArea = 1;
            _score = 0;
            _lm.LevelClear();

            _mainMenuUI.SetActive(true);
            _gameUI.SetActive(false);
        }
    }
}
