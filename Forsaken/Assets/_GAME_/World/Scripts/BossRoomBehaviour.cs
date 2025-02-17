using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomBehaviour : MonoBehaviour
{
    public List<Animator> targetAnimators;
    public GameObject BossHealthBar;
    public AudioClip bossMusic;
    public GameObject player;
    public GameObject boss;
    public CinemachineVirtualCamera virtualCamera;
    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!hasPlayed && collider.CompareTag("Player"))
        {
            foreach (var targetAnimator in targetAnimators)
            {
                targetAnimator.SetTrigger("Play");
            }

            BossHealthBar.SetActive(true);

            // Play Boss Music
            AudioManager.instance.StopAudio("Level_1Music");
            AudioManager.instance.PlayAudio(bossMusic, "BossMusic", 0.1f, true);

            if (virtualCamera != null)
            {
                StartCoroutine(SmoothCameraTransition(7, 1));
                StartCoroutine(SmoothCameraTransitionToBoss(player, boss, 0.5f));
            }

            StartCoroutine(ChangeTriggersToColliders());
            hasPlayed = true;
        }
    }

    private IEnumerator SmoothCameraTransition(float targetSize, float duration)
    {
        float startSize = virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetSize; // Ensure it reaches the exact target size
    }

    private IEnumerator SmoothCameraTransitionToBoss(GameObject player, GameObject target, float duration)
    {
        // Move the camera to follow the target
        virtualCamera.Follow = target.transform;

        Vector3 playerPos = player.transform.position;
        Vector3 targetPos = target.transform.position;
        float elapsedTime = 0f;

        // Transition from player to target position
        while (elapsedTime < duration)
        {
            virtualCamera.transform.position = Vector3.Lerp(playerPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.transform.position = targetPos;

        yield return new WaitForSeconds(duration);

        // Now transition back to the player's position
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            virtualCamera.transform.position = Vector3.Lerp(targetPos, playerPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera is exactly at the player's position at the end of the transition
        virtualCamera.transform.position = playerPos;

        // Set the camera to follow the player again
        virtualCamera.Follow = player.transform;
    }


    private IEnumerator ChangeTriggersToColliders()
    {
        yield return new WaitForSeconds(1); 
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BossRoomCollider"))
        {
            Collider2D collider = obj.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = false;
            }
        }
    }
}
