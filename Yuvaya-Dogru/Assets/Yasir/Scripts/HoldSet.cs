using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSet : MonoBehaviour
{
    public Char_Controller cc;
    Animator _animator;
    [SerializeField] Transform _Cam;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (_animator == null) return;
    
        // Sadece týrmanýrken elleri kilitle
        if (cc.isClimbing)
        {
            _animator.SetLookAtWeight(1f, 0.1f, 1f, 0f, 0.5f);

            // Karakterin baktýðý yönün 2 metre ilerisine bak (Duvar Yönü)
            Vector3 lookPos = _Cam.position + (_Cam.forward+(Vector3.forward*3)) * 2f;
            _animator.SetLookAtPosition(lookPos);
            // --- SAÐ EL ---
            if (cc.rightHandTarget != null)
            {
                // 1. Pozisyon Aðýrlýðý (Ne kadar ýsrarcýyýz?)
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, cc.ikWeight);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, cc.ikWeight);

                // 2. Pozisyonu ve Dönüþü Ayarla (Hedefe yapýþtýr)
                _animator.SetIKPosition(AvatarIKGoal.RightHand, cc.rightHandTarget.position);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, cc.rightHandTarget.rotation);
            }

            // --- SOL EL ---
            if (cc.leftHandTarget != null)
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, cc.ikWeight);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,    cc.ikWeight);

                _animator.SetIKPosition(AvatarIKGoal.LeftHand, cc.leftHandTarget.position);
                _animator.SetIKRotation(AvatarIKGoal.LeftHand, cc.leftHandTarget.rotation);
            }


            // Vucut hedefe bakýyor
            _animator.SetLookAtWeight(1);
            _animator.SetLookAtPosition(cc.rightHandTarget.position);
        }
        else
        {
            // Týrmanma bittiyse aðýrlýklarý sýfýrla ki normal animasyon oynasýn
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }
    }
}
