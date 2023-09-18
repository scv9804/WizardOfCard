using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA.Graphics
{
    // ==================================================================================================== CameraHandler

    [RequireComponent(typeof(Camera))]
    public sealed class CameraHandler : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== Graphic

        // ================================================== Camera

        [FoldoutGroup("카메라")]
        public Camera Camera;

        // ================================================== Movement

        [ShowInInspector] [FoldoutGroup("화면 이동")]
        private float _movementSpeed = 0.05f;

        // ================================================== Shake

        private Vector3 _originalPos;

        private float _timeAtCurrentFrame;
        private float _timeAtLastFrame;
        private float _fakeDelta;

        // ================================================== Transparency Sort

        [ShowInInspector] [FoldoutGroup("좌표 정렬")]
        private bool _isAutoCustomAxis = true;

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Awake()
        {
            CheckCamera();

            if (_isAutoCustomAxis)
            {
                SetTransparencySortOption(TransparencySortMode.CustomAxis, new Vector3(0.0f, 1.0f, -0.26f));
            }
        }

        private void Update()
        {
            Move();
            SetCameraShakeFrame();
        }

        // =========================================================================== Graphic

        // ================================================== Camera

        private void CheckCamera()
        {
            if (Camera == null)
            {
                Camera = GetComponent<Camera>();
            }
        }

        // ================================================== Movement

        private void Move()
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                y += _movementSpeed;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                y -= _movementSpeed;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                x += _movementSpeed;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                x -= _movementSpeed;
            }

            transform.position = new Vector3(x, y, z);
        }

        // ================================================== Shake

        private void SetCameraShakeFrame()
        {
            _timeAtCurrentFrame = Time.realtimeSinceStartup;
            _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
            _timeAtLastFrame = _timeAtCurrentFrame;
        }

        [Button] [FoldoutGroup("화면 진동")]
        public void Shake(float duration, float amount)
        {
            _originalPos = gameObject.transform.localPosition;

            StopAllCoroutines();
            StartCoroutine(Main());

            #region IEnumerator Main();

            IEnumerator Main()
            {
                float endTime = Time.time + duration;

                while (duration > 0)
                {
                    transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

                    duration -= _fakeDelta;

                    yield return null;
                }

                transform.localPosition = _originalPos;
            } 

            #endregion
        }

        // ================================================== Transparency Sort

        [Button] [FoldoutGroup("좌표 정렬")]
        public void SetTransparencySortOption(TransparencySortMode mode, Vector3 axis)
        {
            CheckCamera();

            Camera.transparencySortMode = mode;
            Camera.transparencySortAxis = axis;
        }
    }
}
