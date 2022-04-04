﻿using System;
using UnityEngine;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
    public class PositionInfo
    {
        private string _x;
        public string x
        {
            get => _x;
            set => _x = value;
        }

        private string _y;
        public string y
        {
            get => _y;
            set => _y = value;
        }

        private string _z;
        public string z
        {
            get => _z;
            set => _z = value;
        }

        public PositionInfo() : this(Vector3.zero)
        {

        }

        public PositionInfo(Vector3 position)
        {
            x = position.x.ToString("F8");
            y = position.y.ToString("F8");
            z = position.z.ToString("F8");
        }

        public PositionInfo(PositionInfo positionInfo)
        {
            x = positionInfo.x;
            y = positionInfo.y;
            z = positionInfo.z;
        }

        public override string ToString()
        {
            return $"({x:F8}, {y:F8}, {z:F8})";
        }

        public void Update(Vector3 position)
        {
            x = position.x.ToString("F8");
            y = position.y.ToString("F8");
            z = position.z.ToString("F8");
        }

        public Vector3 ConvertToVector3()
        {
            var xSuccess = float.TryParse(x, out var newX);
            var ySuccess = float.TryParse(y, out var newY);
            var zSuccess = float.TryParse(z, out var newZ);

            return new Vector3(xSuccess ? newX : 0, ySuccess ? newY : 0, zSuccess ? newZ : 0);
        }
    }
}
