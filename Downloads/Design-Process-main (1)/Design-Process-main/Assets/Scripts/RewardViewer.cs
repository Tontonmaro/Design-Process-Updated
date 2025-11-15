using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardViewer : MonoBehaviour
{
    public GameObject viewerPanel;
    public GameObject bgPanel;
    public RawImage modelDisplay;
    public TextMeshProUGUI rewardNameText;
    public Button closeButton;
    public Camera modelCamera;

    private GameObject currentModel;
    private Vector3 lastMousePos;

    void Start()
    {
        viewerPanel.SetActive(false);
        viewerPanel.GetComponent<CanvasGroup>().alpha = 0f;
        bgPanel.SetActive(false);
        bgPanel.GetComponent<CanvasGroup>().alpha = 0f;
        closeButton.onClick.AddListener(CloseViewer);
    }

    public void ShowReward(BlindBoxData.BlindBoxReward reward)
    {
        viewerPanel.SetActive(true);
        viewerPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
        bgPanel.SetActive(true);
        bgPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
        modelDisplay.DOFade(1f, 0.2f);
        rewardNameText.text = reward.rewardName;

        // Destroy any previous model
        if (currentModel != null)
            Destroy(currentModel);

        // Spawn model in front of the modelCamera
        if (reward.rewardPrefab != null)
        {
            currentModel = Instantiate(reward.rewardPrefab, modelCamera.transform);
            currentModel.transform.localPosition = new Vector3(0, 0, 4f); // adjust distance
            currentModel.transform.localRotation = Quaternion.identity;
        }
    }

    public void CloseViewer()
    {
        viewerPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.2f)
            .OnComplete(() => viewerPanel.SetActive(false));
        bgPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.2f)
            .OnComplete(() => bgPanel.SetActive(false));
        modelDisplay.DOFade(0f, 0.2f)
            .OnComplete(() =>
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
        }
    });
    }

    void Update()
    {
        if (!viewerPanel.activeSelf || currentModel == null)
            return;

        // Rotate model with mouse drag
        if (Input.GetMouseButtonDown(0))
            lastMousePos = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            currentModel.transform.Rotate(Vector3.up, -delta.x * 0.5f, Space.World);
            currentModel.transform.Rotate(Vector3.right, delta.y * 0.5f, Space.World);
        }

        // Zoom with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
            currentModel.transform.Translate(Vector3.forward * scroll * 2f);
    }
}
