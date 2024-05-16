using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class ConfirmationButtonVariantData
    {
        public enum ConfirmationButtonVariant
        {
            standard, flipped_colours, yes_in_corner, flipped, invalid
        }

        public ConfirmationButtonVariant buttonVariant;
        public int dayUnlocked;
        public GameObject confirmationButton;

    }
}
