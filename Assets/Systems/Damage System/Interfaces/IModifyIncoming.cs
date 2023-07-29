using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public interface IModifyIncoming<Unit>
        where Unit : ActionUnit
    {
        public void ModifyIncoming(Unit incoming);
    }
}