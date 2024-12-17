using UnityEngine;

/// <summary>
/// ScaleTransformation 类继承自 Transformation，用于实现对象的缩放变换。
/// </summary>
public class ScaleTransformation : Transformation
{
    /// <summary>
    /// 定义缩放因子的三维向量，默认值为 Vector3.one，即无缩放。
    /// </summary>
    public Vector3 scale = Vector3.one;

    /// <summary>
    /// 应用缩放变换到给定的三维点。
    /// </summary>
    /// <param name="point">待变换的三维点。</param>
    /// <returns>返回缩放后的三维点。</returns>
    public override Vector3 Apply(Vector3 point)
    {
        // 对点的每个坐标轴应用缩放因子
        point.x *= scale.x;
        point.y *= scale.y;
        point.z *= scale.z;
        return point;
    }

    /// <summary>
    /// 获取表示缩放变换的四行四列矩阵。
    /// </summary>
    /// <returns>返回缩放变换的 Matrix4x4 矩阵。</returns>
    public override Matrix4x4 Matrix
    {
        get
        {
            // 构建缩放变换矩阵
            Matrix4x4 matrix4X4 = new Matrix4x4();
            matrix4X4.SetRow(0, new Vector4(scale.x, 0, 0, 0));
            matrix4X4.SetRow(1, new Vector4(0, scale.y, 0, 0));
            matrix4X4.SetRow(2, new Vector4(0, 0, scale.z, 0));
            matrix4X4.SetRow(3, new Vector4(0, 0, 0, 1));
            return matrix4X4;
        }
    }
}