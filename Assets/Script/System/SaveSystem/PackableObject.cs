using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPackableHandler
{ 
    [Serializable]
    public class BasicPack
    {
        public virtual void UnPacked<T>(T packable, Action<T> unpacking)
        {
            if (packable != null && unpacking != null) { unpacking.Invoke(packable); }
        }
    }

    public BasicPack Packed { get; }

    public void Initialized();

    public void Initialized(BasicPack basicPack);
}

public abstract class PackableClass : IPackableHandler
{
    public abstract class PackableClassPack : IPackableHandler.BasicPack
    {
        public override void UnPacked<T>(T packable, Action<T> unpacking)
        {
            var typeCheck = packable is PackableClass packableObject;

            if (typeCheck) { base.UnPacked(packable, unpacking); }

            else { Debug.Log($"Type Error: {packable.GetType().Name}"); }
        }
    }

    public abstract IPackableHandler.BasicPack Packed { get; }

    public abstract void Initialized();

    public abstract void Initialized(IPackableHandler.BasicPack basicPack);

    protected virtual void Unpacking()
    {
        //Unpacking Action
    }
}

public abstract class PackableObject : ScriptableObject, IPackableHandler
{
    public abstract class PackableObjectPack : IPackableHandler.BasicPack
    {
        public override void UnPacked<T>(T packable, Action<T> unpacking)
        {
            var typeCheck = packable is PackableObject packableObject;

            if (typeCheck) { base.UnPacked(packable, unpacking); }

            else { Debug.Log($"Type Error: {packable.GetType().Name}"); }
        }
    }

    public abstract IPackableHandler.BasicPack Packed { get; }

    public abstract void Initialized();

    public abstract void Initialized(IPackableHandler.BasicPack basicPack);

    protected virtual void Unpacking() 
    {
        //Unpacking Action
    }
}
