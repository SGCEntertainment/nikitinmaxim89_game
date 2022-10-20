using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Mole : MonoBehaviour
{
    BoxCollider2D myColl;

    [Space(10)]
    [SerializeField] UnityEvent OnAppearanceEvent;
    [SerializeField] UnityEvent OnProcessEvent;
    [SerializeField] UnityEvent OnHitEvent;
    [SerializeField] UnityEvent OnHideEvent;
    [SerializeField] UnityEvent OnNoDamageEvent;

    private void Start()
    {
        myColl = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        myColl.enabled = false;

        GameManager.Instance.UpdatePlayerScore(transform.position);
    }

    public IEnumerator Show()
    {
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

        float delayTime = GameManager.Instance.currentDelayTime;
        yield return new WaitForSeconds(delayTime);

        et = 0.0f;
        duration = GameManager.Instance.currentDisappearanceTime;

        initSize = Vector3.one;
        targetSize = Vector3.zero;

        while (et < duration)
        {
            transform.localScale = Vector3.Lerp(initSize, targetSize, et / duration);

            et += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetSize;
    }
}
