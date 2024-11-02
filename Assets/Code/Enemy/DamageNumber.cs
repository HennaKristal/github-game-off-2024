using UnityEngine;
using TMPro;
using System.Collections;


public class DamageNumber : MonoBehaviour
{
    private static Transform canvasParent;

    [SerializeField] private float moveDistance = 0.5f;
    [SerializeField] private float fadeDuration = 1f;

    private TextMeshProUGUI textMesh;
    private Vector3 targetDirection;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        // Find damage number canvas
        if (canvasParent == null)
        {
            GameObject canvasObject = GameObject.Find("Damage Numbers Canvas");

            if (canvasObject != null)
            {
                canvasParent = canvasObject.transform;
            }
        }

        // Make damage number a child of the damage number canvas
        if (canvasParent != null)
        {
            transform.SetParent(canvasParent, false);
        }
    }


    public void Initialize(int damage, bool isCritical, bool isWeakSpot)
    {
        textMesh.text = damage.ToString();

        if (isCritical)
        {
            if (isWeakSpot)
            {
                textMesh.color = Color.red;
                textMesh.fontSize = 0.35f;
            }
            else
            {
                textMesh.color = new Color(255, 255, 0);
                textMesh.fontSize = 0.3f;
            }
        }
        else
        {
            if (isWeakSpot)
            {
                textMesh.color = Color.red;
                textMesh.fontSize = 0.3f;
            }
            else
            {
                textMesh.color = Color.white;
                textMesh.fontSize = 0.2f;
            }
        }

        StartCoroutine(FadeOutAndMove());
    }


    private IEnumerator FadeOutAndMove()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        // Choose a random direction within 360 degrees
        float angle = Random.Range(0f, 360f);
        targetDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * moveDistance;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;

            // Move text
            float easedProgress = Mathf.Sqrt(progress);
            transform.position = Vector3.Lerp(startPosition, startPosition + targetDirection, easedProgress);

            // Fade out
            float alpha = Mathf.Lerp(1f, 0f, progress);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }
}
