using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VideoPlayer.Models
{
    //: INotifyPropertyChanged
    public class FoldersModels 
    {
        public string path { get;  set; }
        public string name { get;  set; }
        public int id { get; set; }

        public FoldersModels(int id, string path, string name)
        {
            this.id = id;
            this.path = path;
            this.name = name;

        }



        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
