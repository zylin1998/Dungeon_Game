using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveData 
{
    public virtual void Saved(string path, string fileName)
    {
        Saved(path, fileName, SaveSystem.ESaveType.Json);
    }

    public virtual void Saved(string path, string fileName, SaveSystem.ESaveType saveType)
    {
        SaveSystem.SaveData(this, path, fileName, saveType);
    }

    public abstract ISaveHandler.SavePack GetPack();
}

public interface ISaveHandler 
{
    public abstract class SavePack : PackableObject.PackableObjectPack
    {
        public IPackableHandler.BasicPack[] packs;

        protected SavePack() 
        {

        }

        public SavePack(SaveAsset asset) 
        {
            this.packs = asset.Packables.ConvertAll(convert => convert.Packed).ToArray(); 
        }

        public SavePack(IEnumerable<PackableObject.PackableObjectPack> packs) 
        {
            this.packs = packs.ToArray();
        }

        public virtual T GetData<T>() where T : PackableObject.PackableObjectPack
        {
            return packs
                .ToList()
                .Find(match => match is T pack) as T;
        }
    }

    public List<PackableObject> Packables { get; }
}

public abstract class SaveAsset : PackableObject, ISaveHandler
{
    [SerializeField]
    protected List<PackableObject> packables;

    public List<PackableObject> Packables => packables;

    public virtual T GetPackables<T>() where T : PackableObject
    {
        return packables.Find(match => match is T isMatch) as T;
    }
}
