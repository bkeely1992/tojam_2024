﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class CrimeProgressionData
    {
        public CrimeData.Crime crimeToUnlock;
        public int dayToUnlock;
    }
}
