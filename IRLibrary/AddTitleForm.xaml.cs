using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IRLibrary {
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AddTitleForm : Window, INotifyPropertyChanged {
        private string _searchText;
        private MainWindow window;

        public string SearchText {
            get { return _searchText; }
            set {
                _searchText = value;

                OnPropertyChanged("SearchText");
                OnPropertyChanged("MyFilteredItems");
            }
        }

        public IEnumerable<string> MyItems { get; set; }

        public IEnumerable<string> MyFilteredItems {
            get {
                if (SearchText == null || SearchText.Trim() == "") {
                    SearchList.Visibility = Visibility.Collapsed;
                    return null;
                }

                SearchList.Visibility = Visibility.Visible;
                return MyItems.Where(x => x.ToUpper().StartsWith(SearchText.ToUpper()));
            }
        }


        public AddTitleForm(Dictionary<string, Dictionary<string, string>> main, MainWindow window) {
            InitializeComponent();

            MyItems = main.SelectMany(x => x.Value.Values);

            this.window = window;
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private void SelectRealize(object sender, SelectionChangedEventArgs e) {
            if (window.addTitleToForm((sender as ListBox).SelectedItem as string))
                this.Close();
        }
    }

}
