using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEditor.Model
{
    public class MapData : ObservableObject
    {

        private string _mapName;
        public string MapName { get => _mapName; set { _mapName = value; RaisePropertyChanged("MapName"); } }

        private string _date;
        public string Date { get => _date; set { _date = value; RaisePropertyChanged("Date"); } }

        private string _tileSet;
        public string TileSet { get => _tileSet; set { _tileSet = value; RaisePropertyChanged("TileSet"); } }

        private int _tileSize;
        public int TileSize { get => _tileSize; set { _tileSize = value; RaisePropertyChanged("TileSize"); } }

        private int _gridWidth;
        public int GridWidth { get => _gridWidth; set { _gridWidth = value; RaisePropertyChanged("GridWidth"); } }

        private int _gridHeight;
        public int GridHeight { get => _gridHeight; set { _gridHeight = value; RaisePropertyChanged("GridHeight"); } }
        
        public MapData() { }
    }
}
