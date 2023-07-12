using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaterialParticle : MonoBehaviour
{
    public MeshFilter meshFilter;
    public bool playing;

    private Transform targetTransform;
    private Vector3 targetPos;

    public void Initialize(Vector3 startPos, Mesh mesh, Vector3 target)
    {
        targetTransform = null;
        targetPos = target;
        transform.position = startPos;
        gameObject.SetActive(true);
        playing = true;
        meshFilter.mesh = mesh;
        Vector3 startTargetPosition = startPos;
        startTargetPosition.y += Random.Range(2.5f, 3.5f);
        startTargetPosition.z += Random.Range(-1f, 1f);
        startTargetPosition.x += Random.Range(-1f, 1f);

        Vector3 startRotationPosition = new Vector3(0, 0, 0);
        float maxRotationOffset = 150;
        startRotationPosition.x += Random.Range(-maxRotationOffset, maxRotationOffset);
        startRotationPosition.y += Random.Range(-maxRotationOffset, maxRotationOffset);
        startRotationPosition.z += Random.Range(-maxRotationOffset, maxRotationOffset);

        LeanTween.rotate(gameObject, startRotationPosition, 0.5f);
        LeanTween.move(gameObject, startTargetPosition, 0.25f).setEaseOutQuad().setOnComplete(() =>
        {
            StartCoroutine(MoveToPlayer(gameObject));
        });
    }

    public void Initialize(Vector3 startPos, Mesh mesh, Transform target = null)
    {
        targetTransform = target;
        transform.position = startPos;
        gameObject.SetActive(true);
        playing = true;
        meshFilter.mesh = mesh;
        Vector3 startTargetPosition = startPos;
        startTargetPosition.y += Random.Range(2f, 3f);
        startTargetPosition.z += Random.Range(-2f, 2f);
        startTargetPosition.x += Random.Range(-2f, 2f);

        Vector3 startRotationPosition = new Vector3(0, 0, 0);
        float maxRotationOffset = 150;
        startRotationPosition.x += Random.Range(-maxRotationOffset, maxRotationOffset);
        startRotationPosition.y += Random.Range(-maxRotationOffset, maxRotationOffset);
        startRotationPosition.z += Random.Range(-maxRotationOffset, maxRotationOffset);

        LeanTween.rotate(gameObject, startRotationPosition, 0.5f);
        LeanTween.move(gameObject, startTargetPosition, 0.25f).setEaseOutQuad().setOnComplete(() =>
        {
            StartCoroutine(MoveToPlayer(gameObject));
        });
    }

    IEnumerator MoveToPlayer(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);
        Transform objTransform = obj.transform;
        Vector3 startPos = objTransform.position;
        var elapsedTime = 0f;
        var duration = 0.3f;
        while (elapsedTime < duration)
        {
            var t = Mathf.Lerp(0, 1, (elapsedTime / duration));
            t = EasingFunctions.EaseInQuad(0, 1, t);
            if (targetTransform == null)
            {
                var newPosition = Vector3.Lerp(startPos, targetPos, t);
                objTransform.position = newPosition;
            }
            else
            {
                var newPosition = Vector3.Lerp(startPos, targetTransform.position, t);
                objTransform.position = newPosition;
            }
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        playing = false;
        obj.SetActive(false);
    }
}
