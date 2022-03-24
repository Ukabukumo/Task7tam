using UnityEngine;
using System;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject _firePref;
        
    private int   _explosionArea = 1;
    private float _explosionTime = 1.5f;

    /*private void Start()
    {
        StartCoroutine(ExplosionTimer());
    }*/

    public void BombInit(int explosionArea)
    {
        _explosionArea = explosionArea;

        //Debug.Log(_explosionArea + " " + explosionArea);
        StartCoroutine(ExplosionTimer());
    }

    IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(_explosionTime);
        Explosion();
    }

    private void Explosion()
    {
        Instantiate(_firePref, transform.position, Quaternion.identity);

        // Right area
        for (int i = 1; i <= _explosionArea; i++)
        {
            if (!CheckStone(transform.position.x + i, transform.position.y) &&
                !CheckBorder(transform.position.x + i, transform.position.y))
            {
                Vector3 firePos = transform.position + new Vector3(i, 0f, 0f);
                Instantiate(_firePref, firePos, Quaternion.identity);
            }

            else
            {
                break;
            }
        }

        // Left area
        for (int i = 1; i <= _explosionArea; i++)
        {
            if (!CheckStone(transform.position.x - i, transform.position.y) &&
                !CheckBorder(transform.position.x - i, transform.position.y))
            {
                Vector3 firePos = transform.position - new Vector3(i, 0f, 0f);
                Instantiate(_firePref, firePos, Quaternion.identity);
            }

            else
            {
                break;
            }
        }

        // Down area
        for (int i = 1; i <= _explosionArea; i++)
        {
            if (!CheckStone(transform.position.x, transform.position.y - 0.9f * i) &&
                !CheckBorder(transform.position.x, transform.position.y - 0.9f * i))
            {
                Vector3 firePos = transform.position - new Vector3(0.1f * i, 0.9f * i, 0f);
                Instantiate(_firePref, firePos, Quaternion.identity);
            }

            else
            {
                break;
            }
        }

        // Up area
        for (int i = 1; i <= _explosionArea; i++)
        {
            if (!CheckStone(transform.position.x, transform.position.y + 0.9f * i) &&
                !CheckBorder(transform.position.x, transform.position.y + 0.9f * i))
            {
                Vector3 firePos = transform.position + new Vector3(0.1f * i, 0.9f * i, 0f);
                Instantiate(_firePref, firePos, Quaternion.identity);
            }

            else
            {
                break;
            }
        }

        Destroy(gameObject);
    }

    private bool CheckStone(float x, float y)
    {
        // Search for nearest coords
        int nY = (int)Math.Round(Math.Abs(y - 3.6f) / 0.9f);
        int nX = (int)Math.Round(x + 7.9f + 0.1f * nY);

        // Bombs located on even cells (counting from zero)
        if ((nY % 2 == 1) && (nX % 2 == 1))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private bool CheckBorder(float x, float y)
    {
        // Search for nearest coords
        int nY = (int)Math.Round(Math.Abs(y - 3.5f) / 0.9f);
        int nX = (int)Math.Round(x + 7.9f + 0.1f * nY);

        //if ((nY < 0) || (nY > 8) || (nX < 0) || (nX > 16))
        if ((y > 3.5f) || (y < -3.7f) || (nX < 0) || (nX > 16))
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
