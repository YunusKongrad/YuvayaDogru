using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kapıacma : MonoBehaviour
{
    public float openAngle = 45f;
    public float speed = 2f;

    bool isOpen = false;
    bool isMoving = false;

    Quaternion closedRot;
    Quaternion openedRot;

    void Start()
    {
        closedRot = transform.localRotation;
        openedRot = closedRot * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        if (!isMoving) return;

        Quaternion target = isOpen ? openedRot : closedRot;

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            target,
            Time.deltaTime * speed
        );

        if (Quaternion.Angle(transform.localRotation, target) < 0.2f)
        {
            transform.localRotation = target;
            isMoving = false;
        }
    }

    public void PressButton()
    {
        isOpen = !isOpen;
        isMoving = true;
    }
}
