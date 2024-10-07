using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;

    // Durasi animasi
    public float duration = 0.2f;

    // Nilai penambahan width saat hover
    public float widthIncrease = 60f;

    // Menyimpan ukuran asli
    private float originalWidth;

    // Menyimpan child pertama
    private GameObject firstChild;

    private bool parentIsDisabled = false;

    private void Start()
    {
        // Mendapatkan komponen RectTransform dari UI
        rectTransform = GetComponent<RectTransform>();
        originalWidth = rectTransform.sizeDelta.x;  // Menyimpan lebar asli

        // Menyimpan child pertama (index 0)
        if (transform.childCount > 0)
        {
            firstChild = transform.GetChild(0).gameObject;
            firstChild.SetActive(false);  // Memastikan child pertama mulai dalam keadaan mati
        }
    }

    // Ketika kursor mulai hover di UI (event pointer enter)
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Menambah width dengan nilai yang ditentukan
        rectTransform.DOSizeDelta(new Vector2(originalWidth + widthIncrease, rectTransform.sizeDelta.y), duration).SetEase(Ease.OutQuad);

        // Mengaktifkan child pertama
        if (firstChild != null)
        {
            firstChild.SetActive(true);
        }
    }

    // Ketika kursor keluar dari UI (event pointer exit)
    public void OnPointerExit(PointerEventData eventData)
    {
        // Mengembalikan width ke ukuran asli
        rectTransform.DOSizeDelta(new Vector2(originalWidth, rectTransform.sizeDelta.y), duration).SetEase(Ease.OutQuad);

        // Menonaktifkan child pertama
        if (firstChild != null)
        {
            firstChild.SetActive(false);
        }
    }

    // Dipanggil secara otomatis ketika game object ini di-disable
    private void OnDisable()
    {
        ResetUIState();
    }

    // Fungsi untuk mengembalikan UI ke state normal
    private void ResetUIState()
    {
        // Mengembalikan ukuran asli tanpa animasi
        rectTransform.sizeDelta = new Vector2(originalWidth, rectTransform.sizeDelta.y);

        // Menonaktifkan child pertama jika masih aktif
        if (firstChild != null)
        {
            firstChild.SetActive(false);
        }
    }

    // Fungsi yang berjalan tiap frame untuk memeriksa parent
    private void Update()
    {
        if (parentIsDisabled && gameObject.activeInHierarchy)
        {
            parentIsDisabled = false; // Reset flag
        }
        else if (!gameObject.activeInHierarchy && !parentIsDisabled)
        {
            // Jika parent-nya di-disable, jalankan reset
            ResetUIState();
            parentIsDisabled = true; // Set flag agar tidak terus menerus memanggil Reset
        }
    }
}
