using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solgae.FindSolgae
{
    public class PlayerCamera : MonoBehaviour
    {

        public Transform target;

        Vector3 position;

        Quaternion rotation;
        //카메라와의 거리

        public float dist = 30f;

        //카메라 회전 속도

        public float xSpeed = 220.0f;
        public float ySpeed = 100.0f;

        //카메라 초기 위치

        private float x = 0.0f;
        private float y = 0.0f;

        //y값 제한 (위 아래 제한)

        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;

        //앵글의 최소,최대 제한

        float ClampAngle(float angle, float min, float max)

        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

        void Start()

        {
            Vector3 angles = transform.eulerAngles;

            x = angles.y;
            y = angles.x;

        }

        void Update()

        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.015f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.015f;

                //앵글값 정하기(y값 제한)
                y = ClampAngle(y, yMinLimit, yMaxLimit);

                //카메라 위치 변화 계산
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    dist += 10;
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    dist -= 10;
                }
            }
            rotation = Quaternion.Euler(y, x, 0);
            position = rotation * new Vector3(0, 10f, -dist) + target.position + new Vector3(0.0f, 0, 0.0f);

            transform.rotation = rotation;
            transform.position = position;
        }


}

}
