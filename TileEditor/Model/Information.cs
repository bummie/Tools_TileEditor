using GalaSoft.MvvmLight;

namespace TileEditor.Model
{
    public class Information : ObservableObject 
    {
        private string _infoMousePos;
        public string InfoMousePos { get => _infoMousePos; set { _infoMousePos = $"[{value}]"; RaisePropertyChanged("InfoMousePos"); } }

        private string _infoTilePos;
        public string InfoTilePos { get => _infoTilePos; set { _infoTilePos = $"[{value}]";  RaisePropertyChanged("InfoTilePos"); } }

        private string _infoCameraPosition;
        public string InfoCameraPosition
        {
            get => _infoCameraPosition; set { _infoCameraPosition = $"[{value}]"; RaisePropertyChanged("InfoCameraPosition"); }
        }

        public Information() { }
    }
}
