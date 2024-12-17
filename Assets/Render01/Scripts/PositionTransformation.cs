using UnityEngine;

/// <summary>
/// PositionTransformation 类继承自 Transformation，用于在三维空间中应用位置变换。
/// </summary>
public class PositionTransformation : Transformation
{
    /// <summary>
    /// 位置向量，表示在三维空间中的平移量。
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 应用位置变换到给定的点。
    /// </summary>
    /// <param name="point">需要进行变换的三维点。</param>
    /// <returns>返回变换后的位置。</returns>
    public override Vector3 Apply(Vector3 point)
    {
        return point + position;
    }

    /// <summary>
    /// 获取代表位置变换的4x4矩阵。
    /// </summary>
    /// <returns>返回一个Matrix4x4，表示位置变换的矩阵。</returns>
    public override Matrix4x4 Matrix
    {
        get
        {
            Matrix4x4 matrix4X4 = new Matrix4x4();
            matrix4X4.SetRow(0, new Vector4(1, 0, 0, position.x));
            matrix4X4.SetRow(1, new Vector4(0, 1, 0, position.y));
            matrix4X4.SetRow(2, new Vector4(0, 0, 1, position.z));
            matrix4X4.SetRow(3, new Vector4(0, 0, 0, 1));
            return matrix4X4;
        }
    }
}