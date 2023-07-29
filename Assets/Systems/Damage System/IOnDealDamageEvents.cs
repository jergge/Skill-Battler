using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public interface IOnDealDamageEvents
    {
        public event Action<DamageReport?> OnDealDamage;
    }
}