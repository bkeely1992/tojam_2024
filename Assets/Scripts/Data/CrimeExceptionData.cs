using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class CrimeExceptionData
    {
        public CrimeData.Crime crime = CrimeData.Crime.invalid;

        public AnimalData.Animal_class classException = AnimalData.Animal_class.invalid;

        public AnimalData.Animal_diet dietException = AnimalData.Animal_diet.invalid;

        public AnimalData.Animal_species speciesException = AnimalData.Animal_species.invalid;

        public Sprite crimeSprite;
        public Sprite animalSprite;
    }
}
