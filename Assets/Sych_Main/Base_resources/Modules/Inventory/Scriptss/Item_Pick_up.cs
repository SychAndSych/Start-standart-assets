//Ïמהבטנאולûי ןנוהלוע
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Inventory / Object / Object Pick up")]
    [DisallowMultipleComponent]
    public class Item_Pick_up : Item_Game_object_abstract, I_Pick_up
    {
        protected override void Initialized_stats()
        {
            //throw new System.NotImplementedException();
        }
    }
}