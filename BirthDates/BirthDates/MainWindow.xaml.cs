using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Windows.Media.Animation;

namespace BirthDates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<FamilyMember> family = new List<FamilyMember>();
        public MainWindow()
        {
            InitializeComponent();
            ImageBrush br = new ImageBrush();
            br.ImageSource=new BitmapImage(new Uri(@"maxresdefault.jpg", UriKind.Relative));
            canvas.Background = br;
            dataSource();
            cb.IsEditable = false;
            cb.IsReadOnly = true;
            cb.ItemsSource = family;
            cb.SelectionChanged += Cb_SelectionChanged;
           
            
            

            //рисуем крутящийся тортик
            Rectangle rc = new Rectangle();
            rc.Width = 100;
            rc.Height = 100;
            ImageBrush id = new ImageBrush();
            id.ImageSource = new BitmapImage(new Uri(@"cake.png", UriKind.Relative));
            rc.Fill = id;
            //анимация тортика
            rc.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform transform = new RotateTransform()
            {
                CenterX = 0.5,
                CenterY = 0.5,
                Angle = 0,
            };
            rc.RenderTransform = transform;
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(3),
                RepeatBehavior = RepeatBehavior.Forever
            };
            transform.BeginAnimation(RotateTransform.AngleProperty, animation);
            //конец анимации
            Canvas.SetLeft(rc, 20);
            Canvas.SetTop(rc, 20);
            canvas.Children.Add(rc);
            //конец рисования тортика

            checkBirthDay();
        }
        public void dataSource() //Создаем список родственников, читаем файл
        {
            
            string path = @"Data.txt";
            string str = null;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((str = sr.ReadLine()) != null)
                    {
                        string[] s = str.Split(' ');
                        FamilyMember member = new FamilyMember
                            (s[0] + " " + s[1] + " " + s[2], int.Parse(s[3]), int.Parse(s[4]), int.Parse(s[5]));
                        family.Add(member);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка чтения файла!");
            }
        }


        private void Cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb.SelectedItem != null)
            {
                FamilyMember m1 = cb.SelectedItem as FamilyMember;
                day.Content = m1.d;
                month.Content = m1.m;
                year.Content = m1.y;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) //Удаление
        {
            string path = @"Data.txt";

            try
            {

            if (cb.SelectedItem != null)
            {
                FamilyMember m1 = cb.SelectedItem as FamilyMember;
                family.Remove(m1);
                cb.Items.Refresh();
                day.Content = "";
                month.Content = "";
                year.Content = "";
                //Перезаписываем файл заново без удаленной записи
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                foreach (var item in family)
                {
                    string[] nameItem = item.name.Split(' ');
                    sw.WriteLine(nameItem[0] + " " + nameItem[1] + " " + nameItem[2] + " " + item.d + " " + item.m + " " + item.y);
                }
                sw.Close();
                fs.Close();
                checkBirthDay2();
                    MessageBox.Show("Удаление произошло успешно!");
            }
            else
            {
                MessageBox.Show("Выберите человека из списка!");
            }

            }
            catch(Exception)
            {
                MessageBox.Show("Ошибка доступа к файлу на запись!");
                this.Close();
            }

        
    }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) //Справка
        {
            MessageBox.Show("Программа разработана компанией \"ILYA@ Biceps Company LTD\" :))\n\n\t\t\t2018 год");
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e) //Добавить
        {
            string path = @"Data.txt";
            try
            {
                Window1 w = new Window1();
            if(w.ShowDialog()==true)
            {
                
                    int d = int.Parse(w.Day);
                    int m = int.Parse(w.Month);
                    int y = int.Parse(w.Year);
                FamilyMember fm = new FamilyMember(w.Surename + " " + w.MemberName + " " + w.Patronimic, d , m, y);
                family.Add(fm);
                cb.ItemsSource = family;
                cb.Items.Refresh();
                    //Перезаписываем изменения в файл
                    FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    foreach (var item in family)
                    {
                        string[] nameItem = item.name.Split(' ');
                        sw.WriteLine(nameItem[0] + " " + nameItem[1] + " " + nameItem[2] + " " + item.d + " " + item.m + " " + item.y);
                    }
                    sw.Close();
                    fs.Close();
                    checkBirthDay2();
                    MessageBox.Show("Данные успешно добавлены!");
            }
            else
            {
                MessageBox.Show("Данные не были добавлены.");
            }
        }
        catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            catch (Exception)
            {
                MessageBox.Show("Ошибка доступа к файлу на запись!");
                this.Close();
            }
}

        private void MenuItem_Click_4(object sender, RoutedEventArgs e) //Изменить
        {
            string path = @"Data.txt";
            try
            {
            if(cb.SelectedItem!=null)
            {
                    FamilyMember m1 = cb.SelectedItem as FamilyMember;
                    string[] s = m1.name.Split(' ');
                    string surename1 = s[0];
                    string name1 = s[1];
                    string patronimic1 = s[2];
                    ChangeData w1 = new ChangeData(name1,surename1,patronimic1,m1.d,m1.m,m1.y);
                if(w1.ShowDialog()==true)
                {
                    int d = int.Parse(w1.Day.ToString());
                    int m = int.Parse(w1.Month.ToString());
                    int y = int.Parse(w1.Year.ToString());
                   
                    m1.name = w1.Surename + " " + w1.MemberName + " " + w1.Patronimic;
                    m1.d = d;
                    m1.m = m;
                        m1.y = y;
                        day.Content = "";
                        month.Content = "";
                        year.Content = "";
                        cb.ItemsSource = family;
                        cb.SelectedItem = null;
                        cb.Items.Refresh();

                        //Перезаписываем изменения в файл
                        FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);
                        foreach (var item in family)
                        {
                            string[] nameItem = item.name.Split(' ');
                            sw.WriteLine(nameItem[0] + " " + nameItem[1] + " " + nameItem[2] + " " + item.d + " " + item.m + " " + item.y);
                        }
                        sw.Close();
                        fs.Close();
                        checkBirthDay2();
                        MessageBox.Show("Данные успешно изменены!");
                }
                else
                {
                    MessageBox.Show("Данные не были изменены!");
                }
            }
            else
            {
                MessageBox.Show("Выберите человека из списка для изменения данных!");
            }

            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка доступа к файлу на запись!");
                this.Close();
            }
        }
        private void checkBirthDay()
        {
            int c = 0;
            c = canvas.Children.Count;
            canvas.Children.RemoveRange(11, c);
            DateTime curDate = DateTime.Now;
            bool yep = false;
            double x = 20;
            foreach (var item in family)
            {

                if (item.d == curDate.Day && item.m == curDate.Month)
                {
                    yep = true;
                    //устанавливаем ФИО именинника
                    Label l1 = new Label();
                    l1.Foreground = new SolidColorBrush(Colors.Snow);
                    Canvas.SetTop(l1, 250 + x);
                    Canvas.SetLeft(l1, 350);
                    l1.Content = item.name;
                    l1.FontSize = 18;

                    canvas.Children.Add(l1);

                    //устанавливаем количество лет
                    Label l2 = new Label();
                    l2.Foreground = new SolidColorBrush(Colors.Snow);
                    Canvas.SetTop(l2, 246 + x);
                    Canvas.SetLeft(l2, 650);
                    l2.Content = (curDate.Year - item.y).ToString() + " лет";
                    l2.FontSize = 18;

                    DoubleAnimation da = new DoubleAnimation();

                    da.From = 20;
                    da.To = 30;
                    da.AutoReverse = true;
                    da.RepeatBehavior = RepeatBehavior.Forever;
                    da.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                    l2.BeginAnimation(Label.FontSizeProperty, da);
                    canvas.Children.Add(l2);
                    x = x + 30;
                }
            }
            if (yep == false)
            {
                Label l1 = new Label();
                l1.FontSize = 18;
                l1.Foreground = new SolidColorBrush(Colors.Snow);
                Canvas.SetTop(l1, 275);
                Canvas.SetLeft(l1, 390);
                l1.Content = "Сегодня его ни у кого нет :))";
                canvas.Children.Add(l1);
            }
            else
            {

                me.Source = new Uri(@"Sound_20947.mp3", UriKind.Relative);
                me.Play();
            }
        }
        private void checkBirthDay2()
        {
            int c = 0;
            c = canvas.Children.Count;
            canvas.Children.RemoveRange(11, c);
            DateTime curDate = DateTime.Now;
            bool yep = false;
            double x = 20;
            foreach (var item in family)
            {

                if (item.d == curDate.Day && item.m == curDate.Month)
                {
                    yep = true;
                    //устанавливаем ФИО именинника
                    Label l1 = new Label();
                    l1.Foreground = new SolidColorBrush(Colors.Snow);
                    Canvas.SetTop(l1, 250 + x);
                    Canvas.SetLeft(l1, 350);
                    l1.Content = item.name;
                    l1.FontSize = 18;

                    canvas.Children.Add(l1);

                    //устанавливаем количество лет
                    Label l2 = new Label();
                    l2.Foreground = new SolidColorBrush(Colors.Snow);
                    Canvas.SetTop(l2, 246 + x);
                    Canvas.SetLeft(l2, 650);
                    l2.Content = (curDate.Year - item.y).ToString() + " лет";
                    l2.FontSize = 18;

                    DoubleAnimation da = new DoubleAnimation();

                    da.From = 20;
                    da.To = 30;
                    da.AutoReverse = true;
                    da.RepeatBehavior = RepeatBehavior.Forever;
                    da.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                    l2.BeginAnimation(Label.FontSizeProperty, da);
                    canvas.Children.Add(l2);
                    x = x + 30;
                }
            }
            if (yep == false)
            {
                Label l1 = new Label();
                l1.FontSize = 18;
                l1.Foreground = new SolidColorBrush(Colors.Snow);
                Canvas.SetTop(l1, 275);
                Canvas.SetLeft(l1, 390);
                l1.Content = "Сегодня его ни у кого нет :))";
                canvas.Children.Add(l1);
            }
            
        }
        public class FamilyMember
    {
        public string name { get; set; }
        public int d { get; set; }
        public int m { get; set; }
        public int y { get; set; }
        
        public FamilyMember(string Name, int D, int M, int Y)
        {
            this.name = Name;
            this.d = D;
            this.m = M;
            this.y = Y;
        }
    }    
  }
}
