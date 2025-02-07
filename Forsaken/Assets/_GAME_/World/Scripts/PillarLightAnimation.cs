using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PillarLightAnimation : MonoBehaviour
{
    public Light2D targetLight;
    public float fadeDuration = 1.0f;
    public float radiusFadeDuration = 1.0f;

    public void FlashWhite()
    {
        targetLight.color = Color.white;
        StartCoroutine(FadeLightIntensity(0f, 4f, fadeDuration));
        StartCoroutine(FadeLightRadius(10f, 4f, radiusFadeDuration));
    }

    public void FlashBlue()
    {
        targetLight.color = new Color(173f / 255f, 249f / 255f, 255f / 255f); // RGB(173, 249, 255)
        // StartCoroutine(FadeLightIntensity(4f, 2f, fadeDuration));
    }

    public void LowerBlue()
    {
        targetLight.color = new Color(173f / 255f, 249f / 255f, 255f / 255f); // RGB(173, 249, 255)
        StartCoroutine(FadeLightIntensity(4f, 0.3f, fadeDuration));
    }

    IEnumerator FadeLightIntensity(float startIntensity, float endIntensity, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            targetLight.intensity = Mathf.Lerp(startIntensity, endIntensity, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        targetLight.intensity = endIntensity; // Ensure exact final value
    }
    IEnumerator FadeLightRadius(float startRadius, float endRadius, float duration)
    {
        float elapsed = 0f;

        // Gradually change the radius over time
        while (elapsed < duration)
        {
            targetLight.pointLightOuterRadius = Mathf.Lerp(startRadius, endRadius, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait until next frame
        }

        targetLight.pointLightOuterRadius = endRadius;  // Ensure the final radius value
    }

}
