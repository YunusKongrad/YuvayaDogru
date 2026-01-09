using UnityEngine;

public class KaymaMekanigi : MonoBehaviour
{
    public float maxHiz = 6f;
    public float kaymaIvmelenme = 4f;
    public float kaymaYavaslama = 2f;
    public float normalYavaslama = 20f;

    CharacterController cc;
    Char_Controller charCtrl;

    Vector3 velocity;
    bool kaymaAlaninda;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        charCtrl = GetComponent<Char_Controller>();
    }

    void Update()
    {
        // Kamera yönlü input
        Vector3 hedefYon =
        (
            charCtrl.transform.forward * charCtrl._moveInput.y +
            charCtrl.transform.right * charCtrl._moveInput.x
        ).normalized;

        Vector3 hedefVelocity = hedefYon * maxHiz;

        if (kaymaAlaninda)
        {
            if (hedefYon.magnitude > 0.1f)
            {
                // Kaygan zeminde yavaþ hýzlan
                velocity = Vector3.MoveTowards(
                    velocity,
                    hedefVelocity,
                    kaymaIvmelenme * Time.deltaTime
                );
            }
            else
            {
                // Tuþu býrakýnca kayarak dur
                velocity = Vector3.MoveTowards(
                    velocity,
                    Vector3.zero,
                    kaymaYavaslama * Time.deltaTime
                );
            }
        }
        else
        {
            // Normal zeminde anýnda durma
            velocity = Vector3.MoveTowards(
                velocity,
                hedefVelocity,
                normalYavaslama * Time.deltaTime
            );
        }

        cc.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("kaymaAlani"))
            kaymaAlaninda = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("kaymaAlani"))
            kaymaAlaninda = false;
    }
}
