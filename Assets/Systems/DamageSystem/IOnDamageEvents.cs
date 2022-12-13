using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DamageSystem
{
    public interface IOnTakeDamageEvents
    {
        public event Action<DamageInfo> OnTakeDamage;
    }
}
