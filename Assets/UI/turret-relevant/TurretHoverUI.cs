using UnityEngine;
using UnityEngine.UI;

public class TurretStatsUI : MonoBehaviour
{
    public float displayDistance = 5f;
    public float checkInterval = 0.1f; // Interval to check player distance and visibility
    public Camera playerCamera;
    public float canvasHeightOffset = 2f; // Adjust this to control how high above the turret the canvas appears

    private Canvas turretCanvas;
    private RectTransform canvasRect;
    private Text statsText;
    private Transform playerTransform;
    private bool isPlayerNearby = false;
    private float nextCheckTime;

    void Start()
    {
        // Create and configure the Canvas
        GameObject canvasObj = new GameObject("TurretCanvas");
        turretCanvas = canvasObj.AddComponent<Canvas>();
        turretCanvas.renderMode = RenderMode.WorldSpace;
        turretCanvas.transform.SetParent(transform);
        canvasRect = turretCanvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(2, 1); // Size in world units

        // Create and configure the Panel
        GameObject panel = new GameObject("TurretStatsPanel");
        panel.transform.SetParent(turretCanvas.transform);
        panel.AddComponent<CanvasRenderer>();
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.5f); // Semi-transparent black background
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.sizeDelta = canvasRect.sizeDelta;
        panelRect.localPosition = Vector3.zero; // Center the panel on the canvas

        // Create and configure the Text
        GameObject textObj = new GameObject("StatsText");
        statsText = textObj.AddComponent<Text>();
        statsText.transform.SetParent(panel.transform);
        statsText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        statsText.alignment = TextAnchor.MiddleCenter;
        statsText.color = Color.white;
        RectTransform textRect = statsText.GetComponent<RectTransform>();
        textRect.sizeDelta = canvasRect.sizeDelta;
        textRect.localPosition = Vector3.zero; // Center the text on the panel

        turretCanvas.enabled = false;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;

            if (Vector3.Distance(playerTransform.position, transform.position) <= displayDistance && IsTurretInView())
            {
                if (!isPlayerNearby)
                {
                    isPlayerNearby = true;
                    UpdateStatsUI();
                    turretCanvas.enabled = true;
                }
                Vector3 canvasPosition = transform.position + Vector3.up * canvasHeightOffset;
                turretCanvas.transform.position = canvasPosition;
                turretCanvas.transform.LookAt(playerCamera.transform);
            }
            else
            {
                if (isPlayerNearby)
                {
                    isPlayerNearby = false;
                    turretCanvas.enabled = false;
                }
            }
        }
    }

    void UpdateStatsUI()
    {
        Turret turretComponent = GetComponent<Turret>();
        if (turretComponent != null)
        {
            statsText.text = turretComponent.GetStatsAsString();
        }
    }

    bool IsTurretInView()
    {
        Vector3 screenPoint = playerCamera.WorldToViewportPoint(transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
