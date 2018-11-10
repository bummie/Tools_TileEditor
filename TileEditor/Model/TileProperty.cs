using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEditor.Model
{
    public class TileProperty : ObservableObject
    {
        #region Properties

        private int _textureId;
        public int TextureId { get => _textureId; set { _textureId = value; RaisePropertyChanged("TextureId"); } }

        private float _speedMultiplier;
        public float SpeedMultiplier { get => _speedMultiplier; set { _speedMultiplier = value; RaisePropertyChanged("SpeedMultiplier"); } }

        private float _damage;
        public float Damage { get => _damage; set { _damage = value; RaisePropertyChanged("Damage"); } }


        private float _damageInterval;
        public float DamageInterval { get => _damageInterval; set { _damageInterval = value; RaisePropertyChanged("DamageInterval"); } }

        private bool _walkable;
        public bool Walkable { get => _walkable; set { _walkable = value; RaisePropertyChanged("Walkable"); } }

        private bool _water;
        public bool Water { get => _water; set { _water = value; RaisePropertyChanged("Water"); } }

        #endregion

        public TileProperty(int textureId)
        {
            Reset();
            TextureId = textureId;
        }

        /// <summary>
        /// Resets to default values
        /// </summary>
        public void Reset()
        {
            TextureId = 0;
            SpeedMultiplier = 1;
            Damage = 0;
            DamageInterval = 1;
            Walkable = true;
            Water = false;
        }

        /// <summary>
        /// Copies data from given tileproperty
        /// </summary>
        /// <param name="tileProperty"></param>
        public void CopyData(TileProperty tileProperty)
        {
            if(tileProperty == null) { return; }

            TextureId = tileProperty.TextureId;
            SpeedMultiplier = tileProperty.SpeedMultiplier;
            Damage = tileProperty.Damage;
            DamageInterval = tileProperty.DamageInterval;
            Walkable = tileProperty.Walkable;
            Water = tileProperty.Water;
        }
    }
}
