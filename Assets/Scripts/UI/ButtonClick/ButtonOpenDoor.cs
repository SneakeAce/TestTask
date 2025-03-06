using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ButtonOpenDoor : MonoBehaviour
{
    [SerializeField] private Button _button;

    private OpenDoorController _openDoorController;

    [Inject]
    private void Construct(OpenDoorController openDoorController)
    {
        _openDoorController = openDoorController;
        _button.gameObject.SetActive(false);

        _openDoorController.ShowButton += ShowButton;
        _openDoorController.HideButton += HideButton;
        _openDoorController.DisableButton += DisableButton;
    }

    public event Action OpenDoor;

    private void OnDestroy()
    {
        _openDoorController.ShowButton -= ShowButton;
        _openDoorController.HideButton -= HideButton;
        _openDoorController.DisableButton -= DisableButton;
    }

    public void OnClickButton()
    {
        OpenDoor?.Invoke();
        HideButton();
    }

    private void ShowButton()
    {
        _button.gameObject.SetActive(true);
    }

    private void HideButton()
    {
        _button.gameObject.SetActive(false);
    }

    private void DisableButton()
    {
        _openDoorController.ShowButton -= ShowButton;
        _openDoorController.HideButton -= HideButton;
        _openDoorController.DisableButton -= DisableButton;

        _button.enabled = false;
    }
}
