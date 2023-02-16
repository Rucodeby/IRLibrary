using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace IRLibrary {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        
    }
    public partial class CustomExpander : Expander {
        
        StackPanel actualList;
        public CustomExpander(string id, string header, List<string> list) {
            Name = id;
            Header = header;
            actualList = new StackPanel();
            foreach (string item in list) {
                actualList.Children.Add(new ExpanderLabel(item));
            }
            Style = Application.Current.TryFindResource(typeof(Expander)) as Style;
            Content = actualList;
        }

        public void Add(string title) {
            actualList.Children.Add(new ExpanderLabel(title));
        }

        public bool Contains(string title) {
            foreach (ExpanderLabel item in actualList.Children) {
                if (item.Content.ToString() == title) return true;
            }
            return false;
        }
    }

    public partial class ExpanderLabel : Label {
        private Window window;
        public ExpanderLabel(string TitleName) {
            Content = TitleName;
            //this.MouseDown += new MouseButtonEventHandler(this.OpenInfo);
        }

        public void OpenInfo(object nya, MouseButtonEventArgs e) {
            window.Show();
        }
    }

    public partial class RealizeLabel : Label {
        public RealizeLabel(string id, string name) { 
            Name = id;
            Content = name;
        }
    } 

}
