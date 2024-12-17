using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransformation : Transformation
{
    public float focalLength = 2; // 焦距

    public override Vector3 Apply(Vector3 point)
    {
        return point;
    }

    public override Matrix4x4 Matrix
    {
        get
        {
            Matrix4x4 matrix4X4 = new Matrix4x4();

            // matrix4X4.SetRow(0, new Vector4(1,0,0,0));
            // matrix4X4.SetRow(1, new Vector4(0,1,0,0));
            // matrix4X4.SetRow(2, new Vector4(0,0,0,0));
            // matrix4X4.SetRow(3, new Vector4(0,0,1,0));

            matrix4X4.SetRow(0, new Vector4(focalLength, 0, 0, 0));
            matrix4X4.SetRow(1, new Vector4(0, focalLength, 0, 0));
            matrix4X4.SetRow(2, new Vector4(0, 0, 0, 0));
            matrix4X4.SetRow(3, new Vector4(0, 0, 1, 0));


            return matrix4X4;
        }
    }
}