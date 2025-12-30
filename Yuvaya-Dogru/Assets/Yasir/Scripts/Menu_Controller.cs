using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Menu_Controller : MonoBehaviour
{
    [Serializable]
    public struct MenuButon
    {
        // 2. KURAL: Ýçerideki deðiþkenleri görünür yap
        [SerializeField] public GameObject[] buttonsSelect;
        [SerializeField] public Button[] buttons;
        public bool[] isSleder;
        public Slider[] slider;
        public MenuButon(GameObject[] g, Button[] b,bool[] i,Slider[] s)
        {
            buttonsSelect = g;
            buttons = b;
            isSleder = i;
            slider = s;
        }
    }
   
    [SerializeField] public MenuButon[] menuB;

    GameControls _controller;
    [SerializeField] int _index,_index2,butonCount;
    private Coroutine _navigationRoutine, _navigationRoutine2;

    private void Awake() => _controller = new GameControls();

    private void OnEnable()
    {
        _controller.Enable();
        
        _controller.Menu.Move.performed += ctx => StartNavigating2(ctx.ReadValue<float>());
        _controller.Menu.Move.canceled += ctx => StopNavigating2();

        _controller.Menu.Move2.performed += ctx2 => StartNavigating(ctx2.ReadValue<float>());
        _controller.Menu.Value.performed += ctx3 => StartValue(ctx3.ReadValue<float>());
        _controller.Menu.Move2.canceled += ctx2 => StopNavigating();
        _controller.Menu.Select.performed += Select_performed;
    }
    float vv = 0;
    private void StartValue(float value)
    {
        
        if (menuB[_index2].isSleder[_index])
        {
            Debug.Log(value);
            vv += value*0.1f;
            menuB[_index2].slider[_index].value = vv;
            
        }
    }

    private void Select_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       menuB[_index2].buttons[_index].onClick.Invoke();
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
       
        menuB[_index2].buttonsSelect[_index].SetActive(false);

        
        _index = (_index + step + menuB[_index2].buttons.Length) % menuB[_index2].buttons.Length;
        if (menuB[_index2].buttons.Length <= _index)
        {
            Debug.Log("11");
        }
        if (menuB.Length <= _index2)
        {
            Debug.Log("21");
        }

        menuB[_index2].buttonsSelect[_index].SetActive(true);

       
    }

    private void StartNavigating2(float value)
    {
        if (Mathf.Abs(value) < 0.35f) return;
        int direction = value > 0 ? -1 : 1;
        if (_navigationRoutine2 == null)
        {
            _navigationRoutine2 = StartCoroutine(NavigationLoop2(direction));
        }
    }

    private void StopNavigating2()
    {
        if (_navigationRoutine2 != null)
        {
            StopCoroutine(_navigationRoutine2);
            _navigationRoutine2 = null;
        }
    }

    private IEnumerator NavigationLoop2(int direction)
    {
        ChangeIndex2(direction);

        yield return new WaitForSeconds(0.6f);

        while (true)
        {
            ChangeIndex2(direction);
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void ChangeIndex2(int step)
    {

        menuB[_index2].buttonsSelect[_index].SetActive(false);


        _index2 = (_index2 + step + menuB.Length) % menuB.Length;
        if (menuB[_index2].buttons.Length <= _index)
        {
            _index=menuB[_index2].buttons.Length-1;
        }
        if (menuB.Length <= _index2)
        {
            Debug.Log("22");
        }

        menuB[_index2].buttonsSelect[_index].SetActive(true);


    }
}

