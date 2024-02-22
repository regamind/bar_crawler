using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;

    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private Coroutine _damageFlashCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Init();
    }
    // Start is called before the first frame update
    void Init()
    {
        _material = _spriteRenderer.material;
    }

    public void CallDamagefLash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFLasher());
    }

    private IEnumerator DamageFLasher()
    {
        // set color
        _material.SetColor("_FlashColor", _flashColor);


        // lerp flash amount
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while(elapsedTime < _flashTime)
        {
            //iterate elapsedTime
            elapsedTime += Time.deltaTime;

            // lerp flash amount
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
            _material.SetFloat("_FlashAmount", currentFlashAmount);


            yield return null;
        }
    }
}
