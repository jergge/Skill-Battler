using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DamageSystem
{
    public interface IOnDamageEvents
    {
        public event Action<DamageInfo> OnTakeDamage;
    }
}
