using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float shakeDuration = 0.25f;
    [SerializeField] float shakeStrength = 0.4f;

    public void ShakeCamera()
    {
        StartCoroutine(CameraShakeRoutine());
    }

  IEnumerator CameraShakeRoutine()
    {
        Vector3 originalCamPos = transform.localPosition;

        float timePast = 0;

        while(timePast < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeStrength;
            float y = Random.Range(-1f, 1f) * shakeStrength;

            transform.localPosition = new Vector3(x, y, originalCamPos.z);
            timePast += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalCamPos;
    }

}
