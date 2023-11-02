using eFootBallClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace eFootBall
{
    public class MainViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        private DataTable _ListMatch;
        public DataTable ListMatch
        {
            get { return _ListMatch; }
            set
            {
                _ListMatch = value;
                OnPropertyChanged("ListMatch");
            }
        }


        public ICommand LoadDataCommand { get; set; }
        public ICommand NavigateCommand { get; set; }
        public MainViewModel(MainView view)
        {
            LoadDataCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
           {
               try
               {
                   ListMatch = Common.LoadMatch(0).Tables[0];
               }
               catch (Exception ex)
               {
                   System.Windows.MessageBox.Show(ex.Message);
               }
           });

            NavigateCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                try
                {
                    Process.Start("https://www.trauscore.com/match/" + p.ToString() + "/");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
        }
    }
}
