using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _damageFlashColor = Color.red;
    [SerializeField] private Color _lowHealthFlashColor = Color.black;
    [SerializeField] private float _flashTime = 0.25f;
    [SerializeField] private float _lowHealthFlashInterval = 0.5f;
    [SerializeField] public float lowHealthThreshold = 30f;

    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private Coroutine _damageFlashCoroutine;
    private Coroutine _lowHealthFlashCoroutine;

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

    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFLasher());
    }

    public void StartLowHealthFlash()
    {
        if (_lowHealthFlashCoroutine == null)
        {
            _lowHealthFlashCoroutine = StartCoroutine(LowHealthFlasher());
        }
    }

    public void StopLowHealthFlash()
    {
        if (_lowHealthFlashCoroutine != null)
        {
            StopCoroutine(_lowHealthFlashCoroutine);
            _lowHealthFlashCoroutine = null;
            //_material.SetColor("_FlashColor", Color.white); // Reset to default color
        }
    }


    private IEnumerator DamageFLasher()
    {
        // set color
        _material.SetColor("_FlashColor", _damageFlashColor);


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
            _material.SetColor("_FlashColor", _damageFlashColor);


            yield return null;
        }
    }

    private IEnumerator LowHealthFlasher()
    {
        while (true)
        {
            float currentFlashAmount = 0f;
            float elapsedTime = 0f;

            _material.SetColor("_FlashColor", _lowHealthFlashColor);

            while (elapsedTime < _flashTime)
            {
                elapsedTime += Time.deltaTime;
                currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
                _material.SetFloat("_FlashAmount", currentFlashAmount);
                yield return null;
            }

            yield return new WaitForSeconds(_lowHealthFlashInterval);
        }
    }
}
