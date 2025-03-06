using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class OpenDoorController : MonoBehaviour
{
    private List<GameObject> _doors = new List<GameObject>();

    private GameObject _leftDoor;
    private GameObject _rightDoor;
    private LayerMask _doorsLayer = 1 << 7;

    private Coroutine _openDoorCoroutine;

    [Inject] private LazyInject<ButtonOpenDoor> _lazyButtonOpenDoor;
    private ButtonOpenDoor _buttonOpenDoor;

    private readonly float _openingSpeed = 4f;
    private readonly float _normalizerTimeForOpenDoors = 1f;
    private readonly float _baseElapsedTime = 0f;
    private readonly float _yPositionForDoor = 0f;
    private readonly float _basePositionDoor = 0f;

    private readonly string _leftDoorName = "LeftDoorHolder";
    private readonly string _rightDoorName = "RightDoorHolder";

    public event Action ShowButton;
    public event Action HideButton;
    public event Action DisableButton;

    private void Start()
    {
        _buttonOpenDoor = _lazyButtonOpenDoor.Value;

        _buttonOpenDoor.OpenDoor += OpenDoors;
    }

    private void OnDestroy()
    {
        _buttonOpenDoor.OpenDoor -= OpenDoors;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if ((1 << collider.gameObject.layer) == _doorsLayer)
        {
            _doors.Add(collider.gameObject);

            foreach (GameObject item in _doors)
            {
                if (item.gameObject.name == _rightDoorName)
                {
                    _rightDoor = item;
                }
                else if (item.gameObject.name == _leftDoorName)
                {
                    _leftDoor = item;
                }
            }

            if (_leftDoor != null && _rightDoor != null)
                ShowButton?.Invoke();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if ((1 << collider.gameObject.layer) == _doorsLayer)
        {
            if (_doors.Count > 0)
            {
                if (_doors.Contains(_leftDoor))
                    _doors.Remove(_leftDoor);
                else if (_doors.Contains(_rightDoor))
                    _doors.Remove(_rightDoor);
            }

            _doors.Clear();
            HideButton?.Invoke();
        }
    }

    private void OpenDoors()
    {
        _openDoorCoroutine = StartCoroutine(OpenDoorsJob());
    }

    private IEnumerator OpenDoorsJob()
    {
        _leftDoor.GetComponent<Collider>().isTrigger = true;
        _rightDoor.GetComponent<Collider>().isTrigger = true;

        Quaternion startRotationLeftDoor = _leftDoor.transform.rotation;
        Quaternion startRotationRightDoor = _rightDoor.transform.rotation;

        Quaternion targetRotationForDoors = Quaternion.Euler(_basePositionDoor, _yPositionForDoor, _basePositionDoor);

        float elapsedTime = _baseElapsedTime;
        float duration = _normalizerTimeForOpenDoors / _openingSpeed;

        while (elapsedTime < duration)
        {
            float time = elapsedTime / duration;
            _leftDoor.transform.rotation = Quaternion.Slerp(startRotationLeftDoor, targetRotationForDoors, time);

            _rightDoor.transform.rotation = Quaternion.Slerp(startRotationRightDoor, targetRotationForDoors, time);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _leftDoor.transform.rotation = targetRotationForDoors;
        _rightDoor.transform.rotation = targetRotationForDoors;

        _leftDoor.GetComponent<Collider>().isTrigger = false;
        _rightDoor.GetComponent<Collider>().isTrigger = false;

        DisableButton?.Invoke();

        _openDoorCoroutine = null;
    }

}
