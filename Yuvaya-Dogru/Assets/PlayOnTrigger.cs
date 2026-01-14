using UnityEngine;

public class PlayOnTrigger : MonoBehaviour
{
    [SerializeField] private ParabolicMove target;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool oneShot = true;

    private bool played;

    private void OnTriggerEnter(Collider other)
    {
        if (oneShot && played) return;
        if (!other.CompareTag(playerTag)) return;

        played = true;
        target.Play();
    }
}