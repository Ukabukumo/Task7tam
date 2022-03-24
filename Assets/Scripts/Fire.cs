using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    private const float _expireTime = 0.5f;

    private void Start()
    {
        StartCoroutine(ExpireTimer());
    }

    IEnumerator ExpireTimer()
    {
        yield return new WaitForSeconds(_expireTime);

        Destroy(gameObject);
    }
}
