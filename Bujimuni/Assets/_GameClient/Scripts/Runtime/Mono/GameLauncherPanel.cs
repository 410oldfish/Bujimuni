using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLauncherPanel : MonoBehaviour
{
    public GameObject objProgress;
    public Slider downloadSlider;
    public TextMeshProUGUI downloadText;
    
    public GameObject objMessageBox;
    public TextMeshProUGUI messageBoxTitle;
    public TextMeshProUGUI messageBoxContent;
    public Button messageBoxConfirmButton;
    public Button messageBoxCancelButton;

    private void Awake()
    {
        objMessageBox.SetActive(false);
        objProgress.SetActive(false);
    }

    public void SetDownloadProgress(float progress)
    {
        objProgress.SetActive(true);
        progress = Mathf.Clamp01(progress);
        downloadSlider.value = progress;
        downloadText.text = $"downloading... {(int)(progress * 100)}%";
    }
    
    public async UniTask<bool> ShowMessageBox(string title, string content)
    {
        messageBoxTitle.text = title;
        messageBoxContent.text = content;
        var completed = false;
        var confirmed = false;
        messageBoxConfirmButton.onClick.RemoveAllListeners();
        messageBoxConfirmButton.onClick.AddListener(() =>
        {
            completed = true;
            confirmed = true;
        });
        messageBoxCancelButton.onClick.RemoveAllListeners();
        messageBoxCancelButton.onClick.AddListener(() =>
        {
            completed = true;
        });
        objMessageBox.SetActive(true);
        while (!completed)
        {
            await UniTask.DelayFrame(1);
        }
        objMessageBox.SetActive(false);
        return confirmed;
    }
}
