using System.Collections.Generic;
using System;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class WeaponTypeModel<Type> where Type : Enum
    {
        protected List<Type> _data = new();
        protected int _current;

        public Type current => _data[_current % _data.Count];
        public Type next => _data[++_current % _data.Count];
    }

    public class ExtendedWeaponTypeModel<Type> : WeaponTypeModel<Type> where Type : Enum
    {
        public bool TryAddNewType(Type type)
        {
            bool success = _data.Contains(type);
            if (!success) _data.Add(type);
            return !success; // inversion
        }
    }
}