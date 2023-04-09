using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration
{
   public abstract class ITerrainGenerator : MonoBehaviour
   {
      public abstract void GenerateAll();
   }

}
