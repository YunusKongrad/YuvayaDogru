using System.Security.Cryptography;
using UnityEngine;

public class CheckpointRespawn : MonoBehaviour
{
    [Header("Checkpoint yoksa buraya döner")]
    [SerializeField] private Transform startPoint;

    private Vector3 _lastPos;
    private Quaternion _lastRot;
    private bool _hasCheckpoint;
    

    private CharacterController _cc;

    public AudioSource audioSource;
    public AudioClip ses;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();

        // Başlangıç checkpoint’i
        if (startPoint != null)
            SetCheckpoint(startPoint.position, startPoint.rotation);
        else
            SetCheckpoint(transform.position, transform.rotation);
    }

    public void SetCheckpoint(Vector3 pos, Quaternion rot)
    {
        _lastPos = pos;
        _lastRot = rot;
        _hasCheckpoint = true;
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Respawn"))
        {
            RespawnToLastCheckpoint();
            audioSource.PlayOneShot(ses);
        }
    }

    // UI Button bu fonksiyonu çağırır
    public void RespawnToLastCheckpoint()
    {
        if (!_hasCheckpoint) return;

        // CC açıkken position set edersen çoğu zaman itme/titreme olur.
        _cc.enabled = false;

        transform.SetPositionAndRotation(_lastPos, _lastRot);

        _cc.enabled = true;
    }
}
