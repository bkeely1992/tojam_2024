﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    //Data class used for containing the information on a specific type of animal diet
    [Serializable]
    public class AnimalDietData
    {
        public AnimalData.Animal_diet animalDiet;
        public Sprite sprite;
    }
}
