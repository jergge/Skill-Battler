using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public interface IModifyOutgoing<Unit>
        where Unit : ActionUnit
    {
        public void ModifyOutgoing(Unit outgoing);
    }
}