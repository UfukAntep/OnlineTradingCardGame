using System.Collections;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    public float animationSpeed = 5f;
    private Transform _originalTransform;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _originalScale;
    private bool _isSelected;
    private GameObject _parentObject;

    private void Start()
    {
        _isSelected = false;
        _originalTransform = transform;
        _originalPosition = _originalTransform.position;
        _originalRotation = _originalTransform.rotation;
    }

    public void StartAnimation()
    {
        if (_isSelected)
        {
            _isSelected = false;
            SetDefault();
        }
        else
        {
            _isSelected = true;
            _parentObject = transform.parent.gameObject;

            foreach (Transform child in _parentObject.transform)
            {
                CardAnimation cardAnimation = child.GetComponent<CardAnimation>();
                if (cardAnimation != null)
                {
                    cardAnimation.SetDefault();
                }
            }

            StartCoroutine(MoveToCenter());
        }
    }

    private void SetDefault()
    {
        StartCoroutine(MoveToDefault());
    }

    private IEnumerator MoveToCenter()
    {
        Vector3 targetPosition = new Vector3(0f, 0f, 0f);
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * animationSpeed;

            transform.position = Vector3.Lerp(_originalPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(_originalRotation, targetRotation, t);
            yield return null;
        }
    }

    private IEnumerator MoveToDefault()
    {
        Transform targetTransform = transform;
        Vector3 targetPosition = targetTransform.position;
        Quaternion targetRotation = targetTransform.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * animationSpeed;

            transform.position = Vector3.Lerp(targetPosition, _originalPosition, t);
            transform.rotation = Quaternion.Slerp(targetRotation, _originalRotation, t);
            yield return null;
        }
    }
}