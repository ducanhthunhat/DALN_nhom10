using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // Sử dụng hệ thống Input System

public class Platform : MonoBehaviour
{
    public static event Action<Platform> OnPlatformClicked;
    [SerializeField] private LayerMask platformLayerMask;
    public static bool towerPanelOpen { get; set; } = false;
    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (towerPanelOpen || Time.timeScale == 0f) return;

        // 1. Khởi tạo biến lưu vị trí click/chạm và ID con trỏ
        Vector2 screenPosition = Vector2.zero;
        int pointerId = -1;
        bool inputDetected = false;

        // --- Xử lý Input bằng Input System ---

        // 2. Xử lý Input Mobile (Touch) - Dùng Input System
        Touchscreen touchscreen = Touchscreen.current;
        if (touchscreen != null && touchscreen.primaryTouch.press.wasPressedThisFrame)
        {
            // Lấy vị trí chạm
            screenPosition = touchscreen.primaryTouch.position.ReadValue();
            // Trong Input System, Primary Touch thường được coi là ID 0 cho EventSystem
            pointerId = 0;
            inputDetected = true;
        }
        // 3. Xử lý Input PC/Editor (Mouse) - Dùng Input System
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Lấy vị trí chuột
            screenPosition = Mouse.current.position.ReadValue();
            // ID cho chuột là -1
            pointerId = -1;
            inputDetected = true;
        }

        if (!inputDetected)
        {
            return;
        }

        // 4. KIỂM TRA UI TRƯỚC KHI RAYCAST GAMEPLAY
        if (EventSystem.current.IsPointerOverGameObject(pointerId))
        {
            // Input nằm trên UI, bỏ qua tương tác Gameplay
            return;
        }

        // 5. Thực hiện Raycast 2D để tương tác với Platform
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D raycastHit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, platformLayerMask);

        if (raycastHit.collider != null)
        {
            Platform platform = raycastHit.collider.GetComponent<Platform>();
            if (platform != null)
            {
                OnPlatformClicked?.Invoke(platform);
            }
        }
    }

    public void PlaceTower(TowerData data)
    {
        Instantiate(data.prefab, transform.position, Quaternion.identity, transform);

        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = false;
        }
    }
}