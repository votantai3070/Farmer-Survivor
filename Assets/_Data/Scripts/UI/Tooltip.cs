using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private RectTransform rectTransform;

    [Header("Position Settings")]
    [SerializeField] private Vector2 offset = new Vector2(20f, 20f); // Offset từ chuột
    [SerializeField] private float screenEdgePadding = 10f;

    private int characterWrapLimit = 80;

    private void Awake()
    {
        if (layoutElement == null)
        {
            layoutElement = GetComponent<LayoutElement>();
        }
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;
            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
        }

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        // Force update để có kích thước chính xác
        Canvas.ForceUpdateCanvases();

        // Lấy kích thước tooltip
        Vector2 tooltipSize = rectTransform.sizeDelta;

        // Mặc định: tooltip ở phía trên bên phải chuột
        Vector2 tooltipPosition = mousePosition + offset;
        Vector2 pivot = new Vector2(0, 0); // Bottom-left pivot

        // Kiểm tra vượt cạnh phải màn hình
        if (tooltipPosition.x + tooltipSize.x > Screen.width - screenEdgePadding)
        {
            // Đổi sang bên trái chuột
            tooltipPosition.x = mousePosition.x - offset.x;
            pivot.x = 1; // Right pivot
        }

        // Kiểm tra vượt cạnh trên màn hình
        if (tooltipPosition.y + tooltipSize.y > Screen.height - screenEdgePadding)
        {
            // Đổi xuống dưới chuột
            tooltipPosition.y = mousePosition.y - offset.y;
            pivot.y = 1; // Top pivot
        }

        // Kiểm tra vượt cạnh trái màn hình
        if (tooltipPosition.x < screenEdgePadding)
        {
            // Đẩy sang phải
            tooltipPosition.x = mousePosition.x + offset.x;
            pivot.x = 0; // Left pivot
        }

        // Kiểm tra vượt cạnh dưới màn hình
        if (tooltipPosition.y < screenEdgePadding)
        {
            // Đẩy lên trên
            tooltipPosition.y = mousePosition.y + offset.y;
            pivot.y = 0; // Bottom pivot
        }

        // Đặt pivot và position
        rectTransform.pivot = pivot;
        transform.position = tooltipPosition;
    }

    public void SetText(string content, string header = "")
    {
        if (headerField == null || contentField == null)
        {
            Debug.LogError("HeaderField or ContentField is NULL!");
            return;
        }

        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        if (layoutElement != null)
        {
            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
        }

        // Force update layout
        Canvas.ForceUpdateCanvases();
    }
}
