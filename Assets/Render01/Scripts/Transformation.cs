using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 为所有的Transform组件创建一个可以继承的基类。
// 它是一个抽象类，这意味着它不能直接使用。
// 给它一个抽象的Apply方法，具体的转换组件将使用它来完成其工作。
public abstract class Transformation : MonoBehaviour
{
    public abstract Vector3 Apply(Vector3 point);
    public abstract Matrix4x4 Matrix { get; }
}