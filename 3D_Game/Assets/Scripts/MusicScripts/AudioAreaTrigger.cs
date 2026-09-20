using UnityEngine;

public class AudioAreaTrigger : MonoBehaviour
{
    [Header("Area Audio")]
    [SerializeField] private AudioClip areaMusic;
    [SerializeField] private AudioClip areaAmbience;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (AreaAudioController.Instance != null)
        {
            AreaAudioController.Instance.ChangeAreaAudio(
                areaMusic,
                areaAmbience
            );
        }
    }
}
