using UnityEngine;
using DG.Tweening;

public class CameraFloatingEffect : MonoBehaviour
{
    // Konfigurasi gerakan floating
    public float floatDistance = 1f;   // Jarak gerakan ke atas dan ke bawah
    public float floatDuration = 2f;   // Durasi gerakan (naik atau turun)
    
    private Vector3 originalPosition;  // Posisi awal kamera

    private void Start()
    {
        // Simpan posisi awal kamera
        originalPosition = transform.position;

        // Mulai efek mengambang
        StartFloating();
    }

    private void StartFloating()
    {
        // Tween untuk menggerakkan kamera ke atas lalu ke bawah terus menerus
        transform.DOMoveY(originalPosition.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)  // Pola gerakan halus
            .SetLoops(-1, LoopType.Yoyo);  // Ulangi terus menerus dengan pola Yoyo (naik turun)
    }

    private void OnDisable()
    {
        // Hentikan animasi ketika kamera dinonaktifkan atau game objectnya disable
        transform.DOKill();
    }

    private void OnEnable()
    {
        // Mulai ulang efek ketika game object diaktifkan kembali
        StartFloating();
    }
}
