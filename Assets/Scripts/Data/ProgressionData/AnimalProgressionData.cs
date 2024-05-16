﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class AnimalProgressionData
    {
        public AnimalData.Animal_species animalToUnlock;
        public int dayToUnlock;
    }
}
