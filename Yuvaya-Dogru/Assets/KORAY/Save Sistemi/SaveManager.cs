using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [Header("Transforms")]
    public Transform player;           // playerin transformu
    public Transform charObj;          // "char" adlý obje
    public Transform cameraTransform;  // kamera
    public Transform toastLid;         // tost makinesi kapaðý
    public Transform stovePan;         // ocaðýn üstündeki havada duran tava
    public Transform floatingObj;      // havada duran diðer obje (tavan / baþka obje)
    public Transform washingDoor;      // çamaþýr makinesi kapaðý

    [Header("Trigger Durumlarý")]
    public bool toasterTriggered;      // ekmek kýzartma makinesi trigger
    public bool stovePanTriggered;     // tavaya ait trigger

    public TostMakinasi tostScript;
    public FallingObstacleLink tavaScript;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 0 = normal giriþ, 1 = kaldýðýn yerden devam
        int shouldLoad = PlayerPrefs.GetInt("load_from_save", 0);

        if (shouldLoad == 1)
        {
            LoadFromPrefs();

            // Bir kere iþini yaptýktan sonra sýfýrla ki
            // sahne yeniden yüklendiðinde otomatik load olmasýn
            PlayerPrefs.SetInt("load_from_save", 0);
        }
        toasterTriggered = tostScript.tostTrigger;
        stovePanTriggered = tavaScript.tavaTrigger;
    }

    // ================== DIÞARIDAN ÇAÐIRACAÐIN FONKSÝYON ==================
    public void SaveCheckpoint()
    {
        // Transformlar
        SaveTransform("player", player);
        SaveTransform("char", charObj);
        SaveTransform("cam", cameraTransform);
        SaveTransform("toastLid", toastLid);
        SaveTransform("stovePan", stovePan);
        SaveTransform("floatingObj", floatingObj);
        SaveTransform("washingDoor", washingDoor);

        // Triggerlar
        PlayerPrefs.SetInt("toaster_trigger", toasterTriggered ? 1 : 0);
        PlayerPrefs.SetInt("stovePan_trigger", stovePanTriggered ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Checkpoint kaydedildi (PlayerPrefs).");
    }

    // ================== OYUN AÇILDIÐINDA YA DA SAHNEYE DÖNÜNCE ==================
    public void LoadFromPrefs()
    {
        // Player kaydý yoksa hiç bulaþma
        if (!PlayerPrefs.HasKey("player_px"))
        {
            Debug.Log("Kayýt bulunamadý, muhtemelen ilk giriþ.");
            return;
        }

        LoadTransform("player", player);
        LoadTransform("char", charObj);
        LoadTransform("cam", cameraTransform);
        LoadTransform("toastLid", toastLid);
        LoadTransform("stovePan", stovePan);
        LoadTransform("floatingObj", floatingObj);
        LoadTransform("washingDoor", washingDoor);

        toasterTriggered = PlayerPrefs.GetInt("toaster_trigger", 0) == 1;
        stovePanTriggered = PlayerPrefs.GetInt("stovePan_trigger", 0) == 1;

        Debug.Log("Checkpoint yüklendi (PlayerPrefs).");
    }

    // ================== YARDIMCI FONKSÝYONLAR ==================
    void SaveTransform(string key, Transform t)
    {
        if (t == null) return;

        PlayerPrefs.SetFloat(key + "_px", t.position.x);
        PlayerPrefs.SetFloat(key + "_py", t.position.y);
        PlayerPrefs.SetFloat(key + "_pz", t.position.z);

        PlayerPrefs.SetFloat(key + "_rx", t.rotation.x);
        PlayerPrefs.SetFloat(key + "_ry", t.rotation.y);
        PlayerPrefs.SetFloat(key + "_rz", t.rotation.z);
        PlayerPrefs.SetFloat(key + "_rw", t.rotation.w);
    }

    void LoadTransform(string key, Transform t)
    {
        if (t == null) return;
        if (!PlayerPrefs.HasKey(key + "_px")) return;

        Vector3 pos = new Vector3(
            PlayerPrefs.GetFloat(key + "_px"),
            PlayerPrefs.GetFloat(key + "_py"),
            PlayerPrefs.GetFloat(key + "_pz")
        );

        Quaternion rot = new Quaternion(
            PlayerPrefs.GetFloat(key + "_rx"),
            PlayerPrefs.GetFloat(key + "_ry"),
            PlayerPrefs.GetFloat(key + "_rz"),
            PlayerPrefs.GetFloat(key + "_rw")
        );

        t.position = pos;
        t.rotation = rot;
    }
}
