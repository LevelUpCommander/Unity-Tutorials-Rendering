using UnityEngine;

/// <summary>
/// 旋转变换类，继承自Transformation基类，用于在三维空间中对点进行旋转操作。
/// </summary>
public class RotationTransformation : Transformation
{
    /// <summary>
    /// 旋转轴的旋转角度，以度为单位。
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// 应用旋转变换到给定的点上。
    /// </summary>
    /// <param name="point">待变换的点。</param>
    /// <returns>变换后的点。</returns>
    public override Vector3 Apply(Vector3 point)
    {
        // 将旋转角度从度转换为弧度，以便进行三角函数计算。
        float radX = rotation.x * Mathf.Deg2Rad;
        float radY = rotation.y * Mathf.Deg2Rad;
        float radZ = rotation.z * Mathf.Deg2Rad;

        // 计算旋转轴的正弦和余弦值，用于后续的旋转矩阵计算。
        float sinX = Mathf.Sin(radX);
        float cosX = Mathf.Cos(radX);
        float sinY = Mathf.Sin(radY);
        float cosY = Mathf.Cos(radY);
        float sinZ = Mathf.Sin(radZ);
        float cosZ = Mathf.Cos(radZ);

        // 构建旋转矩阵的X轴、Y轴和Z轴分量。
        // 这些向量定义了旋转后的坐标系轴，相对于原始坐标系的表示。
        Vector3 xAxis = new Vector3(cosY * cosZ,
            cosX * sinZ + sinX * sinY * cosZ,
            sinX * sinZ - cosX * sinY * cosZ);
        Vector3 yAxis = new Vector3(-cosY * sinZ,
            cosX * cosZ - sinX * sinY * sinZ,
            sinX * cosZ + cosX * sinY * sinZ);
        Vector3 zAxis = new Vector3(sinY,
            -sinX * cosY,
            cosX * cosY);

        // 应用旋转矩阵到给定的点上，通过向量的线性组合实现旋转。
        return xAxis * point.x + yAxis * point.y + zAxis * point.z;

        // 以下为简化版的二维旋转实现，仅考虑Z轴旋转。
        // return new Vector3(
        //     point.x * cosZ - point.y * sinZ,
        //     point.x * sinZ + point.y * cosZ,
        //     point.z);
    }

    public override Matrix4x4 Matrix
    {
        get
        {
            // 将旋转角度从度转换为弧度，以便进行三角函数计算。
            float radX = rotation.x * Mathf.Deg2Rad;
            float radY = rotation.y * Mathf.Deg2Rad;
            float radZ = rotation.z * Mathf.Deg2Rad;

            // 计算旋转轴的正弦和余弦值，用于后续的旋转矩阵计算。
            float sinX = Mathf.Sin(radX);
            float cosX = Mathf.Cos(radX);
            float sinY = Mathf.Sin(radY);
            float cosY = Mathf.Cos(radY);
            float sinZ = Mathf.Sin(radZ);
            float cosZ = Mathf.Cos(radZ);

            Matrix4x4 matrix4X4 = new Matrix4x4();
            matrix4X4.SetColumn(0, new Vector4(
                cosY * cosZ,
                cosX * sinZ + sinX * sinY * cosZ,
                sinX * sinZ - cosX * sinY * cosZ,
                0));
            matrix4X4.SetColumn(1, new Vector4(
                -cosY * sinZ,
                cosX * cosZ - sinX * sinY * sinZ,
                sinX * cosZ + cosX * sinY * sinZ,
                0
            ));
            matrix4X4.SetColumn(2, new Vector4(
                sinY,
                -sinX * cosY,
                cosX * cosY,
                0
            ));
            matrix4X4.SetColumn(3, new Vector4(0, 0, 0, 1));
            return matrix4X4;
        }
    }
}