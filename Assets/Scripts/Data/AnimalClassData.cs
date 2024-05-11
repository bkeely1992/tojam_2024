using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    //Script used for managing the data of an animal-class (mammal, bird, reptile, etc)
    [Serializable]
    public class AnimalClassData
    {
        public AnimalData.Animal_class animalClass;
        public Sprite sprite;
    }
}
