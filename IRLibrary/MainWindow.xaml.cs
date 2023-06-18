using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IRLibrary {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        bool IsUserLogIn = false;
        //JsonTemplate.titlesList titlesList = JsonSerializer.Deserialize<JsonTemplate.titlesList>(File.ReadAllText("titlesList.json"));

        Dictionary<string, List<string>> usersList = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(File.ReadAllText("userList.json"));

        Dictionary<string, ArrayList> titlesList = new Dictionary<string, ArrayList>();

        Dictionary<string, Dictionary<string, string>> localizedNames = JsonSerializer.Deserialize< Dictionary<string, Dictionary<string, string>>>(File.ReadAllText("localizedNames.json"));

        public MainWindow() {
            InitializeComponent();
        }


        private void Login(object sender, RoutedEventArgs e) {
            if (!IsUserLogIn) {
                LoginForm loginForm = new LoginForm(this);
                loginForm.ShowDialog( );
            }
            else {
                // I just don't wanna do smt now
                MessageBox.Show("SOMETHING WRONG");
            }
        }

        private void OpenAddForm(object sender, RoutedEventArgs e) {
            AddTitleForm form = new AddTitleForm(localizedNames, this);
            form.ShowDialog( );
        }

        public void updateData(bool Logined, string name) {
            IsUserLogIn = Logined;
            ButtonUserName.Text = name;
            if (usersList.ContainsKey(name)) {
                foreach (string id in usersList[name]) {
                    addTitleToList(id);
                }
            }
            foreach (var pair in titlesList) {
                ArrayList names = new ArrayList();
                foreach (string item in pair.Value) {
                    names.Add(localizedNames[pair.Key][item]);
                }
                if (names.Count > 1) {
                    CustomExpander expander = new(pair.Key, localizedNames[pair.Key].First().Value, names.Cast<string>().ToList());
                    ExpanderPanel.Children.Add(expander);
                } else {
                    RealizeLabel label = new(pair.Key, localizedNames[pair.Key].First().Value);
                    ExpanderPanel.Children.Add(label);
                }
            }
            updateCounters();
        }

        public void addTitleToList(string id) {
            var ID = id.Split("|");
            if (!titlesList.ContainsKey(ID[0])) {
                titlesList[ID[0]] = new ArrayList();
            }
            titlesList[ID[0]].Add(ID[1]);
        }

        public bool addTitleToForm(string name) {
            string ID = localizedNames.FirstOrDefault(x => x.Value.ContainsValue(name)).Key;
            FrameworkElement el = findTitle(ID);
            if (el is CustomExpander) {
                if ((el as CustomExpander).Contains(name)) {
                    MessageBox.Show("Данный релиз уже есть в вашем списке");
                    return false;
                }
                (el as CustomExpander).Add(name);
            } else if (el is RealizeLabel) {
                RealizeLabel tmp = (RealizeLabel)el;
                if (name == tmp.Content.ToString()) {
                    MessageBox.Show("Данный релиз уже есть в вашем списке");
                    return false;
                }
                ExpanderPanel.Children.Add(new CustomExpander(ID, tmp.Content.ToString(), new List<string>() { tmp.Content.ToString(), name }));
                ExpanderPanel.Children.Remove(tmp);
            } else if (el == null & name != null) {
                ExpanderPanel.Children.Add(new RealizeLabel(ID, name));
            } else
                return false;
            updateCounters();
            return true;
        }

        public FrameworkElement findTitle(string title) {
            foreach(FrameworkElement item in ExpanderPanel.Children)
                if (item.Name == title) return item;
            return null;
        }

        private void updateCounters() {
            TitlesCount.Content = ExpanderPanel.Children.Count != 0 ? ExpanderPanel.Children.Count : null;
            int realizes = 0;
            foreach (FrameworkElement elem in ExpanderPanel.Children) {
                if (elem is CustomExpander) realizes += ((elem as CustomExpander).Content as StackPanel).Children.Count;
                else realizes += 1;
            }
            RealizeCount.Content = realizes != 0 ? realizes : null;
        }
    }
}
