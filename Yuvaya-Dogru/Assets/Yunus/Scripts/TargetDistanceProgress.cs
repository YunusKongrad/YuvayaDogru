using UnityEngine;
using UnityEngine.UI;

public class TargetDistanceProgress : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;      // Char (Player)
    [SerializeField] private Transform target;      // Hedef nokta (Transform)
    [SerializeField] private Slider progressSlider; // UI Slider

    [Header("Behaviour")]
    [Tooltip("Slider değeri güncellensin mi?")]
    [SerializeField] private bool updateEveryFrame = true;

    [Tooltip("Başlangıç mesafesini otomatik al (hedef set edilince / Start'ta)")]
    [SerializeField] private bool autoCaptureMaxDistance = true;

    private float _maxDistance = -1f;

    private void Awake()
    {
        // Slider'ı güvenli başlangıçta boş yap
        if (progressSlider != null)
        {
            progressSlider.minValue = 0f;
            progressSlider.maxValue = 1f;
            progressSlider.value = 0f;
        }
    }

    private void Start()
    {
        // Player atanmadıysa tag ile bulmayı dene (senin tag Player)
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (autoCaptureMaxDistance)
            CaptureMaxDistance();
    }

    private void Update()
    {
        if (!updateEveryFrame) return;
        UpdateProgress();
    }

    /// <summary>
    /// Hedefi runtime'da değiştirirsen buradan set et.
    /// </summary>
    public void SetTarget(Transform newTarget, bool recaptureMaxDistance = true)
    {
        target = newTarget;

        if (progressSlider != null)
            progressSlider.value = 0f; // yeni hedefte tekrar boş başla

        if (recaptureMaxDistance)
            CaptureMaxDistance();
    }

    /// <summary>
    /// Başlangıç mesafesini "maxDistance" olarak kaydeder.
    /// Slider'ın "başta boş" olması için şart.
    /// </summary>
    public void CaptureMaxDistance()
    {
        if (player == null || target == null)
        {
            _maxDistance = -1f;
            return;
        }

        _maxDistance = Vector3.Distance(player.position, target.position);

        // Edge-case: oyuncu hedefin üstünde spawn olduysa maxDistance 0 olur.
        // Bölme hatası olmasın diye küçük bir eşik koy.
        if (_maxDistance < 0.001f)
            _maxDistance = 0.001f;

        // Başlangıçta boş
        if (progressSlider != null)
            progressSlider.value = 0f;
    }

    /// <summary>
    /// Slider'ı günceller: hedefe yaklaştıkça dolar.
    /// </summary>
    public void UpdateProgress()
    {
        if (player == null || target == null || progressSlider == null) return;
        if (_maxDistance <= 0f) return;

        float currentDistance = Vector3.Distance(player.position, target.position);
        float progress = 1f - (currentDistance / _maxDistance);

        progressSlider.value = Mathf.Clamp01(progress);
    }
}
