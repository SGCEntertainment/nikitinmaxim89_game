using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Mole : MonoBehaviour
{
    BoxCollider2D myColl;

    [Space(10)]
    [SerializeField] UnityEvent OnShowEvent;
    [SerializeField] UnityEvent OnIdleEvent;
    [SerializeField] UnityEvent OnHideEvent;
    [SerializeField] UnityEvent OnHitEvent;
    [SerializeField] UnityEvent OnNoDamageEvent;

    private void Start()
    {
        myColl = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        OnHitEvent.Invoke();
        myColl.enabled = false;
        GameManager.Instance.UpdatePlayerScore(transform.position);
    }

    public IEnumerator Show()
    {
        OnShowEvent.Invoke();
        myColl.enabled = true;

        float et = 0.0f;
        float duration = GameManager.Instance.currentAppearanceTime;

        Vector3 initSize = Vector3.zero;
        Vector2 targetSize = Vector3.one;

        while (et < duration)
        {
            transform.localScale = Vector3.Lerp(initSize, targetSize, et / duration);

            et += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetSize;

        et = 0.0f;
        float delayTime = GameManager.Instance.currentDelayTime;
        while(et < delayTime)
        {
            OnIdleEvent.Invoke();
            yield return null;
        }

        et = 0.0f;
        duration = GameManager.Instance.currentDisappearanceTime;

        initSize = Vector3.one;
        targetSize = Vector3.zero;
        OnNoDamageEvent.Invoke();

        while (et < duration)
        {
            transform.localScale = Vector3.Lerp(initSize, targetSize, et / duration);

            et += Time.deltaTime;
            yield return null;
        }

        OnHideEvent.Invoke();
        transform.localScale = targetSize;
    }
}
