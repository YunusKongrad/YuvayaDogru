using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Controller : MonoBehaviour
{
    [SerializeReference] Button[] buttons;
    [SerializeReference] GameObject[] buttonsSelect;

    GameControls _controller;
    int _index;
    private Coroutine _navigationRoutine;

    private void Awake() => _controller = new GameControls();

    private void OnEnable()
    {
        _controller.Enable();
        _controller.Menu.Move.performed += ctx => StartNavigating(ctx.ReadValue<float>());
        _controller.Menu.Move.canceled += ctx => StopNavigating();
    }

    private void OnDisable() => _controller.Disable();

    private void StartNavigating(float value)
    {       
        if (Mathf.Abs(value) < 0.35f) return;
        int direction = value > 0 ? -1 : 1;
        if (_navigationRoutine == null)
        {
            _navigationRoutine = StartCoroutine(NavigationLoop(direction));
        }
    }

    private void StopNavigating()
    {
        if (_navigationRoutine != null)
        {
            StopCoroutine(_navigationRoutine);
            _navigationRoutine = null;
        }
    }

    private IEnumerator NavigationLoop(int direction)
    {
        ChangeIndex(direction);

        yield return new WaitForSeconds(0.6f);

        while (true)
        {
            ChangeIndex(direction);
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void ChangeIndex(int step)
    {
       
        buttonsSelect[_index].SetActive(false);

       
        _index = (_index + step + buttonsSelect.Length) % buttonsSelect.Length;

       
        buttonsSelect[_index].SetActive(true);

       
    }
}

