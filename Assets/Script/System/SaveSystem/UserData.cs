using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Save Data", menuName = "System/Save Data", order = 1)]
public class UserData : SaveAsset
{
    [System.Serializable]
    public class Pack : ISaveHandler.SavePack
    {
        protected Pack() : base() { }

        public Pack(UserData asset) : base(asset) { }

        public Pack(IEnumerable<PackableObjectPack> packs) : base(packs) { }
    }

    public override IPackableHandler.BasicPack Packed => new Pack(this);

    public override void Initialized() 
    {
        this.packables.ForEach(packable => packable.Initialized());
    }

    public override void Initialized(IPackableHandler.BasicPack basicPack) 
    {
        if (basicPack is Pack pack) 
        {
            pack
                .UnPacked(this, packables => packables.packables
                    .ForEach(packable => pack.packs
                        .ToList()
                        .ForEach(p => packable.Initialized(p))));
        }  
    }
}
