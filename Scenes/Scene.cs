using System;
using System.Collections.Generic;


//#pragma warning disable CA1416
namespace ConsoleRenderer;

public class Scene {
    /// <summary> Init </summary>
    public Scene () { }

    /// <summary> Update frame </summary>
    public virtual void Update () { }

    /// <summary> Update frame </summary>
    public virtual void FixedUpdate () { }

    /// <summary> Update frame </summary>
    public virtual void FixedUpdate_Skip () { }

    /// <summary> Inputs </summary>
    public virtual void Inputs () { }

    /*/// <summary> Update frame </summary>
    public virtual void Exit () { }*/

}
