using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheHarbor2;
using System.Threading;
using System.IO;

namespace TheHarbor
{

    public partial class MainWindow : Window
    {
        int BoatsNotDocked = 0;

        public bool[] dock1 = new bool[64];
        public bool[] dock2 = new bool[128];
        List<Boat> harbor = new List<Boat>();
        List<Boat> newBoats = new List<Boat>();
        List<Boat> buggedBoats = new List<Boat>();
        
        Random rnd = new Random();
        Func<Random, char> GL = GetLetter;

        ImageBrush RowBoatSprite = new ImageBrush();
        ImageBrush MotorboatSprite = new ImageBrush();
        ImageBrush SailboatSprite = new ImageBrush();
        ImageBrush CatamaranSprite = new ImageBrush();
        ImageBrush CargoboatSprite = new ImageBrush();
        ImageBrush RowBoatFlippedSprite = new ImageBrush();
        ImageBrush MotorboatFlippedSprite = new ImageBrush();
        ImageBrush SailboatFlippedSprite = new ImageBrush();
        ImageBrush CatamaranFlippedSprite = new ImageBrush();
        ImageBrush CargoboatFlippedSprite = new ImageBrush();

        public MainWindow()
        {
            InitializeComponent();
            CreateHarbors();
            ReadDataFile();
            AddBoatsStart();
        }

        public void CreateHarbors()
        {
            ImageBrush HarborTopSprite = new ImageBrush();
            ImageBrush HarborBottomSprite = new ImageBrush();
            HarborTopSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Harbor1Top.png"));
            HarborBottomSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Harbor2Bottom.png"));

            string name = "HarborTop";
            Rectangle Rectangle = new Rectangle();
            Rectangle.Width = 768;
            Rectangle.Height = 24;
            RegisterName(name, Rectangle);
            Canvas.SetLeft(Rectangle, 0);
            Canvas.SetTop(Rectangle, 0);
            mainCanvas.Children.Add(Rectangle);

            Rectangle HarborTopFill = (Rectangle)FindName("HarborTop");
            if (HarborTopFill != null)
            {
                HarborTopFill.Fill = HarborTopSprite;
            }

            name = "HarborBottom";
            Rectangle = new Rectangle();
            Rectangle.Width = 768;
            Rectangle.Height = 24;
            RegisterName(name, Rectangle);
            Canvas.SetLeft(Rectangle, 0);
            Canvas.SetTop(Rectangle, 384);
            mainCanvas.Children.Add(Rectangle);

            Rectangle HarborBottomFill = (Rectangle)FindName("HarborBottom");
            if (HarborBottomFill != null)
            {
                HarborBottomFill.Fill = HarborBottomSprite;
            }
        }
        public void Update(int BoatsToSpawn)
        {

            var leavingBoats = harbor
                .Where(b => b.DaysBeen >= b.DaysStay)
                .ToList();
            harbor = harbor
                .Except(leavingBoats)
                .ToList();

            RemoveBoats(leavingBoats);
            

            for (int i = 0; i < BoatsToSpawn; i++)
                {
                    int z = rnd.Next(0, 4 + 1);
                    switch (z)
                    {
                        case 0:
                            int x = DockBoatHalf(dock1, dock2);
                            if (x < 64)
                            {
                                dock1[x] = true;
                                var boat = (new Rowboat(x, 0, ($"{GL(rnd)}{GL(rnd)}{GL(rnd)}"), rnd.Next(100, 300 + 1), rnd.Next(0, 3 + 1), 1, 0, rnd.Next(1, 6 + 1)));
                                harbor.Add(boat);
                                newBoats.Add(boat);
                            }
                            else if (x < 128)
                            {
                                dock2[x] = true;
                                var boat = (new Rowboat(x, 0, ($"{GL(rnd)}{GL(rnd)}{GL(rnd)}"), rnd.Next(100, 300 + 1), rnd.Next(0, 3 + 1), 1, 0, rnd.Next(1, 6 + 1)));
                                harbor.Add(boat);
                                newBoats.Add(boat);
                            }
                            else
                            {

                            StreamWriter swBND = new StreamWriter("BoatsNotDocked.txt", false);
                            BoatsNotDocked++;
                            swBND.WriteLine(BoatsNotDocked);
                            swBND.Close();                                
                            }
                            break;
                        case 1:
                            x = DockBoatOne(dock1, dock2);
                            if (x < 64)
                            {
                                dock1[x] = true;
                                dock1[x + 1] = true;
                                var boat = (new Motorboat(x, 1, ($"{GL(rnd)}{GL(rnd)}{GL(rnd)}"), rnd.Next(200, 3000 + 1), rnd.Next(0, 60 + 1), 3, 0, rnd.Next(10, 1000 + 1)));
                                harbor.Add(boat);
                                newBoats.Add(boat);
                            }
                            else if (x < 128)
                            {
                                dock2[x] = true;
                                dock2[x + 1] = true;
                                var boat = (new Motorboat(x, 1, ($"{GL(rnd)}{GL(rnd)}{GL(rnd)}"), rnd.Next(200, 3000 + 1), rnd.Next(0, 60 + 1), 3, 0, rnd.Next(10, 1000 + 1)));
                                harbor.Add(boat);
                                newBoats.Add(boat);
                            }
                            else
                            {
                            StreamWriter swBND = new StreamWriter("BoatsNotDocked.txt", false);
                            BoatsNotDocked++;
                            swBND.WriteLine(BoatsNotDocked);
                            swBND.Close();
                        }
                            break;
                        case 2:
                            x = DockBoatTwo(dock1, dock2);
                            if (x < 64)
                            {
                                for (int j = 0; j <= 3; j++)
                                {
                                    dock1[x + j] = true;
                                }
                                var boat = (new Sailboat(x, 2, ($"{GL(rnd)}{GL(rnd)}{GL(rnd)}"), rnd.Next(800, 6000 + 1), rnd.Next(0, 12 + 1), 4, 0, rnd.Next(10, 60 + 1)));
                                    harbor.Add(boat);
                                    newBoats.Add(boat);
                            }
                            else if (x < 128)
                            {
                                for (int j = 0; j <= 3; j++)
                                {
                                    dock2[x + j] = true;
                                }
                            var boat = (new Sailboat(x, 2, ($"{GL(rnd)}{GL(rnd)}{GL(rnd)}"), rnd.Next(800, 6000 + 1), rnd.Next(0, 12 + 1), 4, 0, rnd.Next(10, 60 + 1)));
                                harbor.Add(boat);
                                newBoats.Add(boat);
                            }
                            else
                            {
                                StreamWriter swBND = new StreamWriter("BoatsNotDocked.txt", false);
                                BoatsNotDocked++;
                                swBND.WriteLine(BoatsNotDocked);
                                swBND.Close();
                            }
                        break;
                        case 3:
                            x = DockBoatThree(dock1, dock2);
                            if (x < 64)
                            {
                                for (int j = 0; j <= 5; j++)
                                {
                                    dock1[x + j] = true;
                                }
                                var boat = (new Catamaran(x, 3, ($"{GL(rnd)}{GL(rnd)}{GL(rnd)}"), rnd.Next(1200, 8000 + 1), rnd.Next(0, 12 + 1), 3, 0, rnd.Next(1, 4 + 1)));
                                    harbor.Add(boat);
                                    newBoats.Add(boat);
                            }
                            else if (x < 128)
                            {
                                for (int j = 0; j <= 5; j++)
                                {
                                    dock2[x + j] = true;
                                }
                                var boat = (new Catamaran(x, 3, ($"{GL(rnd)}{GL(rnd)}{GL(rnd)}"), rnd.Next(1200, 8000 + 1), rnd.Next(0, 12 + 1), 3, 0, rnd.Next(1, 4 + 1)));
                                    harbor.Add(boat);
                                    newBoats.Add(boat);
                            }
                            else
                            {
                                StreamWriter swBND = new StreamWriter("BoatsNotDocked.txt", false);
                                BoatsNotDocked++;
                                swBND.WriteLine(BoatsNotDocked);
                                swBND.Close();
                            }
                        break;
                        case 4:
                            x = DockBoatFour(dock1,dock2);
                            if (x < 64)
                            {
                                for (int j = 0; j <= 7; j++)
                                {
                                dock1[x+j] = true;
                                }    
                            
                                var boat = (new Cargoboat(x, 4, ($"{GL(rnd)}{GL(rnd)}{GL(rnd)}"), rnd.Next(3000, 20000 + 1), rnd.Next(0, 20 + 1), 6, 0, rnd.Next(0, 500 + 1)));
                                harbor.Add(boat);
                                newBoats.Add(boat);
                            }
                            else if (x < 128)
                            {
                                for (int j = 0; j <= 7; j++)
                                {
                                    dock2[x + j] = true;
                                }
                                var boat = (new Cargoboat(x, 4, ($"{GL(rnd)}{GL(rnd)}{GL(rnd)}"), rnd.Next(3000, 20000 + 1), rnd.Next(0, 20 + 1), 6, 0, rnd.Next(0, 500 + 1)));
                                    harbor.Add(boat);
                                    newBoats.Add(boat);
                            }
                            else
                            {
                                StreamWriter swBND = new StreamWriter("BoatsNotDocked.txt", false);
                                BoatsNotDocked++;
                                swBND.WriteLine(BoatsNotDocked);
                                swBND.Close();
                            }
                        break;
                    }
            }

            StreamWriter sw = new StreamWriter("Harbor.txt");
            foreach (var b in harbor)
            {
                sw.WriteLine($"{b.Place},{b.Type},{b.SerialNumber},{b.Weight},{b.Speed},{b.DaysStay},{b.DaysBeen},{b.Special()}");
                b.DaysBeen++;
            }
            sw.Close();

            AddBoats(newBoats);
            newBoats = new List<Boat>();
            leavingBoats = new List<Boat>();
            DockDataFiles();
        }

        public static int DockBoatHalf(bool[] dock1, bool[] dock2)
        {
            int i;
            bool docked = false;

            //check spot availibility for boat specific size.
            for (i = 0; i <= 64; i++)
            {
                try
                {
                    if (dock1[i - 1] && !dock1[i] && dock1[i + 1])
                    {
                        docked = true;
                        break;
                    }
                    else if (dock2[i - 1 + 64] && !dock2[i + 64] && dock2[i+64+1])
                    {
                        i = i + 64;
                        docked = true;
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }

            //check NEXT availible spot
            if (!docked)
            { 
                for (i = 0; i <= 64; i++)
                {
                    try
                    {
                        if (!dock1[i])
                        {
                            break;
                        }
                        else if (!dock2[i + 64])
                        {
                            i = i + 64;
                            break;
                        }
                    }
                    catch
                    {
                        i = 128;
                        break;

                    }
                }
            }

            return i;
        }
        public static int DockBoatOne(bool[] dock1, bool[] dock2)
        {
            int i;
            bool docked = false;

            //check spot availible for boat specific size.
            for (i = 2; i <= 64; i++)
            {
                try
                {
                    if (dock1[i - 1] && !dock1[i] && !dock1[i + 1] && dock1[i+2])
                    {
                        docked = true;
                        break;
                    }
                    else if (dock2[i - 1 + 64] && !dock2[i + 64] && !dock2[i + 1 + 64] && dock2[i + 2 + 64])
                    {
                        i = i + 64;
                        docked = true;
                        break;
                    }
                }
                catch
                {
                    break;
                }
                i++;
            }

            //check NEXT availible spot
            if (!docked)
            {

                for (i = 0; i <= 64; i++)
                {
                    try
                    {
                        if (!dock1[i] && !dock1[i + 1])
                        {
                            break;
                        }
                        else if (!dock2[i + 64] && !dock2[i + 1 + 64])
                        {
                            i = i + 64;
                            break;
                        }
                    }
                    catch
                    {
                        i = 128;
                        break;
                    }
                    i++;
                }
            }

            return i;
        }
        public static int DockBoatTwo(bool[] dock1, bool[] dock2)
        {
            int i;
            bool docked = false;

            //check spot availible for boat specific size.
            for (i = 2; i <= 64; i++)
            {
                try
                {
                    if (dock1[i - 1] && !dock1[i] && !dock1[i + 1] && !dock1[i + 2] && !dock1[i + 3] && dock1[4])
                    {
                        docked = true;
                        break;
                    }
                    else if (dock2[i - 1 + 64] && !dock2[i + 64] && !dock2[i + 1 + 64] && !dock2[i + 2 + 64] && !dock2[i + 3 + 64] && dock2[i + 4 + 64])
                    {
                        i = i + 64;
                        docked = true;
                        break;
                    }
                    i++;
                }
                catch
                {
                    break;
                }
            }

            //check NEXT availible spot
            if (!docked)
            {
                for (i = 0; i <= 64; i++)
                {
                    try
                    {
                        if (!dock1[i] && !dock1[i + 1] && !dock1[i + 2] && !dock1[i + 3])
                        {
                            break;
                        }
                        else if (!dock2[i + 64] && !dock2[i + 1 + 64] && !dock2[i + 2 + 64] && !dock2[i + 3 + 64])
                        {
                            i = i + 64;
                            break;
                        }
                    }
                    catch
                    {
                        i = 128;
                        break;
                    }
                    i++;
                }
            }

            return i;
        }
        public static int DockBoatThree(bool[] dock1, bool[] dock2)
        {
            int i;
            bool docked = false;

            //check spot availible for boat specific size.
            for (i = 2; i <= 64; i++)
            {
                try
                {
                    if (dock1[i - 1] && !dock1[i] && !dock1[i + 1] && !dock1[i + 2] && !dock1[i + 3] && !dock1[i + 4] && !dock1[i + 5] && dock1[i + 6])
                    {
                        docked = true;
                        break;
                    }
                    else if (dock2[i - 1 + 64] && !dock2[i + 64] && !dock2[i + 1 + 64] && !dock2[i + 2 + 64] && !dock2[i + 3 + 4] && !dock2[i + 4 + 64] && !dock2[i + 5 + 64] && dock2[i + 6 + 64])
                    {
                        i = i + 64;
                        docked = true;
                        break;
                    }
                    i++;
                }
                catch
                {
                    break;
                }
            }

            //check NEXT availible spot
            if (!docked)
            {
                for (i = 0; i <= 64; i++)
                {
                    try
                    {
                        if (!dock1[i] && !dock1[i + 1] && !dock1[i + 2] && !dock1[i + 3] && !dock1[i + 4] && !dock1[i + 5])
                        {
                            break;
                        }
                        else if (!dock2[i + 64] && !dock2[i + 1 + 64] && !dock2[i + 2 + 64] && !dock2[i + 3 + 4] && !dock2[i + 4 + 64] && !dock2[i + 5 + 64])
                        {
                            i = i + 64;
                            break;
                        }
                    }
                    catch
                    {
                        i = 128;
                        break;
                    }
                    i++;
                }
            }

            return i;
        }
        public static int DockBoatFour(bool[] dock1, bool[]dock2)
        {
            int i;
            bool docked = false;

            //check spot availible for boat specific size.
            for (i = 2; i <= 64; i++)
            {
                try
                {
                    if (dock1[i-1] && !dock1[i] && !dock1[i + 1] && !dock1[i + 2] && !dock1[i + 3] && !dock1[i + 4] && !dock1[i + 5] && !dock1[i + 6] && !dock1[i + 7] && dock1[i+8])
                    {
                        docked = true;
                        break;
                    }
                    else if (dock2[i - 1 + 64] && !dock2[i + 64] && !dock2[i + 1 + 64] && !dock2[i + 2 + 64] && !dock2[i + 3 + 64] && !dock2[i + 4 + 64] && !dock2[i + 5 + 64] && !dock2[i + 6 + 64] && !dock2[i + 7 + 64] && dock2[i + 8 + 64])
                    {
                        i = i + 64;
                        docked = true;
                        break;
                    }
                    i++;
                }
                catch
                {
                    break;
                }
            }

            //check NEXT availible spot
            if (!docked)
            {
                for (i = 0; i <= 64; i++)
                {
                    try
                    {
                        if (!dock1[i] && !dock1[i + 1] && !dock1[i + 2] && !dock1[i + 3] && !dock1[i + 4] && !dock1[i + 5] && !dock1[i + 6] && !dock1[i + 7])
                        {
                            break;
                        }
                        else if (!dock2[i + 64] && !dock2[i + 1 + 64] && !dock2[i + 2 + 64] && !dock2[i + 3 + 64] && !dock2[i + 4 + 64] && !dock2[i + 5 + 64] && !dock2[i + 6 + 64] && !dock2[i + 7 + 64])
                        {
                            i = i + 64;
                            break;
                        }
                    }
                    catch
                    {
                        i = 128;
                        break;
                    }
                    i++;
                }
            }

            return i;
        }

        public void AddBoats(List<Boat> newBoats)
        {
            RowBoatSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Rowboat.png"));
            SailboatSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Sailboat.png"));
            CatamaranSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Catamaran.png"));
            CargoboatSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/CargoShip.png"));
            MotorboatSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Motorboat.png"));
            RowBoatFlippedSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Rowboat.png"));
            SailboatFlippedSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/SailboatFlipped.png"));
            CatamaranFlippedSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/CatamaranFlipped.png"));
            CargoboatFlippedSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/CargoShipFlipped.png"));
            MotorboatFlippedSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/MotorboatFliped.png"));

            foreach (Boat b in newBoats)
            {
                if(b is Rowboat)
                {
                    string name = $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber;
                    Rectangle Rowboat = new Rectangle();
                    Rowboat.Name = name;
                    Rowboat.Width = 12;
                    Rowboat.Height = 24;
                    Rowboat.MouseEnter += new MouseEventHandler(UIElement_MouseEnter);
                    Rowboat.MouseLeave += new MouseEventHandler(UIElement_MouseLeave);
                    try { RegisterName(name, Rowboat); }
                    catch
                    {
                        buggedBoats = harbor
                            .Where(b => Enum.GetName(typeof(Type), b.Type) + b.SerialNumber == Enum.GetName(typeof(Type), b.Type) + b.SerialNumber)
                            .ToList();
                    }


                    if (b.Place < 64)
                    {
                        Canvas.SetLeft(Rowboat, 12 * b.Place);
                        Canvas.SetTop(Rowboat, 24);
                        mainCanvas.Children.Add(Rowboat);

                        Rectangle RowBoatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (RowBoatFill != null)
                        {
                            RowBoatFill.Fill = RowBoatSprite;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(Rowboat, 12 * (b.Place - 64));
                        Canvas.SetTop(Rowboat, 360);
                        mainCanvas.Children.Add(Rowboat);

                        Rectangle RowBoatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (RowBoatFill != null)
                        {
                            RowBoatFill.Fill = RowBoatFlippedSprite;
                        }
                    }
                }

                else if (b is Motorboat)
                {
                    string name = $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber;
                    Rectangle Motorboat = new Rectangle();
                    Motorboat.Name = name;
                    Motorboat.Width = 24;
                    Motorboat.Height = 24;
                    Motorboat.MouseEnter += new MouseEventHandler(UIElement_MouseEnter);
                    Motorboat.MouseLeave += new MouseEventHandler(UIElement_MouseLeave);
                    try { RegisterName(name, Motorboat); }
                    catch
                    {
                        buggedBoats = harbor
                            .Where(b => Enum.GetName(typeof(Type), b.Type) + b.SerialNumber == Enum.GetName(typeof(Type), b.Type) + b.SerialNumber)
                            .ToList();
                    }

                    if (b.Place < 64)
                    {
                        Canvas.SetLeft(Motorboat, 12 * b.Place);
                        Canvas.SetTop(Motorboat, 24);
                        mainCanvas.Children.Add(Motorboat);

                        Rectangle MotorboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (MotorboatFill != null)
                        {
                            MotorboatFill.Fill = MotorboatSprite;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(Motorboat, 12 * (b.Place - 64));
                        Canvas.SetTop(Motorboat, 360);
                        mainCanvas.Children.Add(Motorboat);

                        Rectangle MotorboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (MotorboatFill != null)
                        {
                            MotorboatFill.Fill = MotorboatFlippedSprite;
                        }
                    }
                }

                else if (b is Sailboat)
                {
                    string name = $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber;
                    Rectangle Sailboat = new Rectangle();
                    Sailboat.Name = name;
                    Sailboat.Width = 48;
                    Sailboat.Height = 24;
                    Sailboat.MouseEnter += new MouseEventHandler(UIElement_MouseEnter);
                    Sailboat.MouseLeave += new MouseEventHandler(UIElement_MouseLeave);
                    try{ RegisterName(name, Sailboat); }
                    catch
                    {
                        buggedBoats = harbor
                            .Where(b => Enum.GetName(typeof(Type), b.Type) + b.SerialNumber == Enum.GetName(typeof(Type), b.Type) + b.SerialNumber)
                            .ToList();
                    }

                    if (b.Place < 64)
                    {
                        Canvas.SetLeft(Sailboat, 12 * b.Place);
                        Canvas.SetTop(Sailboat, 24);
                        mainCanvas.Children.Add(Sailboat);

                        Rectangle SailboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (SailboatFill != null)
                        {
                            SailboatFill.Fill = SailboatSprite;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(Sailboat, 12 * (b.Place - 64));
                        Canvas.SetTop(Sailboat, 360);
                        mainCanvas.Children.Add(Sailboat);

                        Rectangle SailboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (SailboatFill != null)
                        {
                            SailboatFill.Fill = SailboatFlippedSprite;
                        }
                    }

                }

                else if (b is Catamaran)
                {
                    string name = $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber;
                    Rectangle Catamaran = new Rectangle();
                    Catamaran.Name = name;
                    Catamaran.Width = 72;
                    Catamaran.Height = 48;
                    Catamaran.MouseEnter += new MouseEventHandler(UIElement_MouseEnter);
                    Catamaran.MouseLeave += new MouseEventHandler(UIElement_MouseLeave);
                    try{ RegisterName(name, Catamaran); }
                    catch
                    {
                        buggedBoats = harbor
                            .Where(b => Enum.GetName(typeof(Type), b.Type) + b.SerialNumber == Enum.GetName(typeof(Type), b.Type) + b.SerialNumber)
                            .ToList();
                    }

                    if (b.Place < 64)
                    {
                        Canvas.SetLeft(Catamaran, 12 * b.Place);
                        Canvas.SetTop(Catamaran, 24);
                        mainCanvas.Children.Add(Catamaran);

                        Rectangle CatamaranFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (CatamaranFill != null)
                        {
                            CatamaranFill.Fill = CatamaranSprite;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(Catamaran, 12 * (b.Place - 64));
                        Canvas.SetTop(Catamaran, 336);
                        mainCanvas.Children.Add(Catamaran);

                        Rectangle CatamaranFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (CatamaranFill != null)
                        {
                            CatamaranFill.Fill = CatamaranFlippedSprite;
                        }
                    }
                    
                }

                else if (b is Cargoboat)
                {
                    string name = $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber;
                    Rectangle Cargoboat = new Rectangle();
                    Cargoboat.Name = name;
                    Cargoboat.Width = 96;
                    Cargoboat.Height = 48;
                    Cargoboat.MouseEnter += new MouseEventHandler(UIElement_MouseEnter);
                    Cargoboat.MouseLeave += new MouseEventHandler(UIElement_MouseLeave);
                    try{ RegisterName(name, Cargoboat); }
                    catch
                    {
                        buggedBoats = harbor
                            .Where(b => Enum.GetName(typeof(Type), b.Type) + b.SerialNumber == Enum.GetName(typeof(Type), b.Type) + b.SerialNumber)
                            .ToList();
                    }

                    if (b.Place < 64)
                    {
                        Canvas.SetLeft(Cargoboat, 12 * b.Place);
                        Canvas.SetTop(Cargoboat, 24);
                        mainCanvas.Children.Add(Cargoboat);

                        Rectangle CargoboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (CargoboatFill != null)
                        {
                            CargoboatFill.Fill = CargoboatSprite;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(Cargoboat, 12 * (b.Place - 64));
                        Canvas.SetTop(Cargoboat, 336);
                        mainCanvas.Children.Add(Cargoboat);

                        Rectangle CargoboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (CargoboatFill != null)
                        {
                            CargoboatFill.Fill = CargoboatFlippedSprite;
                        }
                    }

                }


            }


            //Om en båt registreras med exakt samma namn (random, lite chans) kan inte rectangle skapas, denna dubbla båten tas då bort.
            harbor = harbor
                .Except(buggedBoats)
                .ToList();
            foreach (var b in buggedBoats)
            {
                if (b is Rowboat)
                {
                    if (b.Place < 64)
                    {
                        dock1[b.Place] = false;
                    }
                    else
                    {
                        dock2[b.Place] = false;
                    }
                }

                else if (b is Motorboat)
                {
                    if (b.Place < 64)
                    {
                        dock1[b.Place] = false;
                        dock1[b.Place + 1] = false;
                    }
                    else
                    {
                        dock2[b.Place] = false;
                        dock2[b.Place + 1] = false;
                    }
                }

                else if (b is Sailboat)
                {
                    if (b.Place < 64)
                    {
                        dock1[b.Place] = false;
                        dock1[b.Place + 1] = false;
                        dock1[b.Place + 2] = false;
                        dock1[b.Place + 3] = false;
                    }
                    else
                    {
                        dock2[b.Place] = false;
                        dock2[b.Place + 1] = false;
                        dock2[b.Place + 2] = false;
                        dock2[b.Place + 3] = false;
                    }
                }

                else if (b is Catamaran)
                {
                    if (b.Place < 64)
                    {
                        dock1[b.Place] = false;
                        dock1[b.Place + 1] = false;
                        dock1[b.Place + 2] = false;
                        dock1[b.Place + 3] = false;
                        dock1[b.Place + 4] = false;
                        dock1[b.Place + 5] = false;
                    }
                    else
                    {
                        dock2[b.Place] = false;
                        dock2[b.Place + 1] = false;
                        dock2[b.Place + 2] = false;
                        dock2[b.Place + 3] = false;
                        dock2[b.Place + 4] = false;
                        dock2[b.Place + 5] = false;
                    }
                }

                else if (b is Cargoboat)
                {
                    if (b.Place < 64)
                    {
                        dock1[b.Place] = false;
                        dock1[b.Place + 1] = false;
                        dock1[b.Place + 2] = false;
                        dock1[b.Place + 3] = false;
                        dock1[b.Place + 4] = false;
                        dock1[b.Place + 5] = false;
                        dock1[b.Place + 6] = false;
                        dock1[b.Place + 7] = false;
                    }
                    else
                    {
                        dock2[b.Place] = false;
                        dock2[b.Place + 1] = false;
                        dock2[b.Place + 2] = false;
                        dock2[b.Place + 3] = false;
                        dock2[b.Place + 4] = false;
                        dock2[b.Place + 5] = false;
                        dock2[b.Place + 6] = false;
                        dock2[b.Place + 7] = false;
                    }
                }
            }
            buggedBoats = new List<Boat>();

            //Sammanfattning
            double snitthastighet = 0;
            try { snitthastighet = Math.Round((harbor.Average(b => b.Speed) * 1.8), 1, MidpointRounding.ToEven); } catch { snitthastighet = 0; }
            sammanfattningLabel.Content = $"Roddbåtar: {harbor.Count(b => b.Type == 0)}" + ('\n') +
                $"Motorbåtar: {harbor.Count(b => b.Type == 1)}" + ('\n') +
                $"Segelbåtar: {harbor.Count(b => b.Type == 2)}" + ('\n') +
                $"Katamaraner: {harbor.Count(b => b.Type == 3)}" + ('\n') +
                $"Lastfartyg: {harbor.Count(b => b.Type == 4)}" + ('\n') + ('\n') +
                $"Totalvikt: {harbor.Sum(b => b.Weight)} kg" + ('\n') +
                $"Medelhastighet: {snitthastighet} km/h" + ('\n') +
                $"Lediga platser: {(64 - (harbor.Count(b => b.Type == 0) * 0.5) - (harbor.Count(b => b.Type == 1) * 1) - (harbor.Count(b => b.Type == 2) * 2) - (harbor.Count(b => b.Type == 3) * 3) - (harbor.Count(b => b.Type == 4) * 4))}" + ('\n') +
                $"Avvisade båtar: {BoatsNotDocked}";
        }
        public void AddBoatsStart()
        {
            //Denna metod körs endast vid start av programmet.

            RowBoatSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Rowboat.png"));
            SailboatSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Sailboat.png"));
            CatamaranSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Catamaran.png"));
            CargoboatSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/CargoShip.png"));
            MotorboatSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Motorboat.png"));
            RowBoatFlippedSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Rowboat.png"));
            SailboatFlippedSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/SailboatFlipped.png"));
            CatamaranFlippedSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/CatamaranFlipped.png"));
            CargoboatFlippedSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/CargoShipFlipped.png"));
            MotorboatFlippedSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/MotorboatFliped.png"));

            foreach (Boat b in harbor)
            {
                if (b is Rowboat)
                {
                    string name = $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber;
                    Rectangle Rowboat = new Rectangle();
                    Rowboat.Name = name;
                    Rowboat.Width = 12;
                    Rowboat.Height = 24;
                    Rowboat.MouseEnter += new MouseEventHandler(UIElement_MouseEnter);
                    Rowboat.MouseLeave += new MouseEventHandler(UIElement_MouseLeave);
                    try { RegisterName(name, Rowboat); }
                    catch
                    {
                        buggedBoats = harbor
                            .Where(b => Enum.GetName(typeof(Type), b.Type) + b.SerialNumber == Enum.GetName(typeof(Type), b.Type) + b.SerialNumber)
                            .ToList();
                    }


                    if (b.Place < 64)
                    {
                        Canvas.SetLeft(Rowboat, 12 * b.Place);
                        Canvas.SetTop(Rowboat, 24);
                        mainCanvas.Children.Add(Rowboat);

                        Rectangle RowBoatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (RowBoatFill != null)
                        {
                            RowBoatFill.Fill = RowBoatSprite;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(Rowboat, 12 * (b.Place - 64));
                        Canvas.SetTop(Rowboat, 360);
                        mainCanvas.Children.Add(Rowboat);

                        Rectangle RowBoatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (RowBoatFill != null)
                        {
                            RowBoatFill.Fill = RowBoatFlippedSprite;
                        }
                    }
                }

                else if (b is Motorboat)
                {
                    string name = $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber;
                    Rectangle Motorboat = new Rectangle();
                    Motorboat.Name = name;
                    Motorboat.Width = 24;
                    Motorboat.Height = 24;
                    Motorboat.MouseEnter += new MouseEventHandler(UIElement_MouseEnter);
                    Motorboat.MouseLeave += new MouseEventHandler(UIElement_MouseLeave);
                    try { RegisterName(name, Motorboat); }
                    catch
                    {
                        buggedBoats = harbor
                            .Where(b => Enum.GetName(typeof(Type), b.Type) + b.SerialNumber == Enum.GetName(typeof(Type), b.Type) + b.SerialNumber)
                            .ToList();
                    }

                    if (b.Place < 64)
                    {
                        Canvas.SetLeft(Motorboat, 12 * b.Place);
                        Canvas.SetTop(Motorboat, 24);
                        mainCanvas.Children.Add(Motorboat);

                        Rectangle MotorboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (MotorboatFill != null)
                        {
                            MotorboatFill.Fill = MotorboatSprite;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(Motorboat, 12 * (b.Place - 64));
                        Canvas.SetTop(Motorboat, 360);
                        mainCanvas.Children.Add(Motorboat);

                        Rectangle MotorboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (MotorboatFill != null)
                        {
                            MotorboatFill.Fill = MotorboatFlippedSprite;
                        }
                    }
                }

                else if (b is Sailboat)
                {
                    string name = $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber;
                    Rectangle Sailboat = new Rectangle();
                    Sailboat.Name = name;
                    Sailboat.Width = 48;
                    Sailboat.Height = 24;
                    Sailboat.MouseEnter += new MouseEventHandler(UIElement_MouseEnter);
                    Sailboat.MouseLeave += new MouseEventHandler(UIElement_MouseLeave);
                    try { RegisterName(name, Sailboat); }
                    catch
                    {
                        buggedBoats = harbor
                            .Where(b => Enum.GetName(typeof(Type), b.Type) + b.SerialNumber == Enum.GetName(typeof(Type), b.Type) + b.SerialNumber)
                            .ToList();
                    }

                    if (b.Place < 64)
                    {
                        Canvas.SetLeft(Sailboat, 12 * b.Place);
                        Canvas.SetTop(Sailboat, 24);
                        mainCanvas.Children.Add(Sailboat);

                        Rectangle SailboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (SailboatFill != null)
                        {
                            SailboatFill.Fill = SailboatSprite;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(Sailboat, 12 * (b.Place - 64));
                        Canvas.SetTop(Sailboat, 360);
                        mainCanvas.Children.Add(Sailboat);

                        Rectangle SailboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (SailboatFill != null)
                        {
                            SailboatFill.Fill = SailboatFlippedSprite;
                        }
                    }

                }

                else if (b is Catamaran)
                {
                    string name = $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber;
                    Rectangle Catamaran = new Rectangle();
                    Catamaran.Name = name;
                    Catamaran.Width = 72;
                    Catamaran.Height = 48;
                    Catamaran.MouseEnter += new MouseEventHandler(UIElement_MouseEnter);
                    Catamaran.MouseLeave += new MouseEventHandler(UIElement_MouseLeave);
                    try { RegisterName(name, Catamaran); }
                    catch
                    {
                        buggedBoats = harbor
                            .Where(b => Enum.GetName(typeof(Type), b.Type) + b.SerialNumber == Enum.GetName(typeof(Type), b.Type) + b.SerialNumber)
                            .ToList();
                    }

                    if (b.Place < 64)
                    {
                        Canvas.SetLeft(Catamaran, 12 * b.Place);
                        Canvas.SetTop(Catamaran, 24);
                        mainCanvas.Children.Add(Catamaran);

                        Rectangle CatamaranFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (CatamaranFill != null)
                        {
                            CatamaranFill.Fill = CatamaranSprite;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(Catamaran, 12 * (b.Place - 64));
                        Canvas.SetTop(Catamaran, 336);
                        mainCanvas.Children.Add(Catamaran);

                        Rectangle CatamaranFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (CatamaranFill != null)
                        {
                            CatamaranFill.Fill = CatamaranFlippedSprite;
                        }
                    }

                }

                else if (b is Cargoboat)
                {
                    string name = $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber;
                    Rectangle Cargoboat = new Rectangle();
                    Cargoboat.Name = name;
                    Cargoboat.Width = 96;
                    Cargoboat.Height = 48;
                    Cargoboat.MouseEnter += new MouseEventHandler(UIElement_MouseEnter);
                    Cargoboat.MouseLeave += new MouseEventHandler(UIElement_MouseLeave);
                    try { RegisterName(name, Cargoboat); }
                    catch
                    {
                        buggedBoats = harbor
                            .Where(b => Enum.GetName(typeof(Type), b.Type) + b.SerialNumber == Enum.GetName(typeof(Type), b.Type) + b.SerialNumber)
                            .ToList();
                    }

                    if (b.Place < 64)
                    {
                        Canvas.SetLeft(Cargoboat, 12 * b.Place);
                        Canvas.SetTop(Cargoboat, 24);
                        mainCanvas.Children.Add(Cargoboat);

                        Rectangle CargoboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (CargoboatFill != null)
                        {
                            CargoboatFill.Fill = CargoboatSprite;
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(Cargoboat, 12 * (b.Place - 64));
                        Canvas.SetTop(Cargoboat, 336);
                        mainCanvas.Children.Add(Cargoboat);

                        Rectangle CargoboatFill = (Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber);
                        if (CargoboatFill != null)
                        {
                            CargoboatFill.Fill = CargoboatFlippedSprite;
                        }
                    }

                }


            }

            double snitthastighet = 0;
            try { snitthastighet = Math.Round((harbor.Average(b => b.Speed) * 1.8), 1, MidpointRounding.ToEven); } catch { snitthastighet = 0; }
            sammanfattningLabel.Content = $"Roddbåtar: {harbor.Count(b => b.Type == 0)}" + ('\n') +
                $"Motorbåtar: {harbor.Count(b => b.Type == 1)}" + ('\n') +
                $"Segelbåtar: {harbor.Count(b => b.Type == 2)}" + ('\n') +
                $"Katamaraner: {harbor.Count(b => b.Type == 3)}" + ('\n') +
                $"Lastfartyg: {harbor.Count(b => b.Type == 4)}" + ('\n') + ('\n') +
                $"Totalvikt: {harbor.Sum(b => b.Weight)} kg" + ('\n') +
                $"Medelhastighet: {snitthastighet} km/h" + ('\n') +
                $"Lediga platser: {(64 - (harbor.Count(b => b.Type == 0) * 0.5) - (harbor.Count(b => b.Type == 1) * 1) - (harbor.Count(b => b.Type == 2) * 2) - (harbor.Count(b => b.Type == 3) * 3) - (harbor.Count(b => b.Type == 4) * 4))}" + ('\n') +
                $"Avvisade båtar: {BoatsNotDocked}";
        }
        public void RemoveBoats(List<Boat> leavingBoats)
        {
            //sätt bool[] till false utifrån de båtar som lämnar.
            foreach (var b in leavingBoats)
            {
                if(b is Rowboat)
                {
                    if (b.Place < 64)
                    {
                        dock1[b.Place] = false;
                    }
                    else
                    {
                        dock2[b.Place] = false;
                    }
                }

                else if (b is Motorboat)
                {
                    if (b.Place < 64)
                    {
                        dock1[b.Place] = false;
                        dock1[b.Place + 1] = false;
                    }
                    else
                    {
                        dock2[b.Place] = false;
                        dock2[b.Place + 1] = false;
                    }
                }

                else if (b is Sailboat)
                {
                    if (b.Place < 64)
                    {
                        for (int i = 0; i <= 3; i++)
                            dock1[b.Place + i] = false;
                    }
                    else
                    {
                        for(int i = 0; i <= 3; i++)
                            dock2[b.Place + i] = false;
                    }
                }

                else if (b is Catamaran)
                {
                    if (b.Place < 64)
                    {
                        for (int i = 0; i <= 5; i++)
                            dock1[b.Place + i] = false;
                    }
                    else
                    {
                        for (int i = 0; i <= 5; i++)
                            dock2[b.Place + i] = false;
                    }
                }

                else if (b is Cargoboat)
                {
                    if (b.Place < 64)
                    {
                        for (int i = 0; i <= 7; i++)
                            dock1[b.Place + i] = false;
                    }
                    else
                    {
                        for (int i = 0; i <= 7; i++)
                            dock2[b.Place + i] = false;
                    }
                }

                //ta bort rectangle utifrån child name.
                mainCanvas.Children.Remove((Rectangle)FindName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber));
                try { UnregisterName($"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber); } catch { }
            }
        }
        public static char GetLetter(Random rnd)
        {
            //Metod för att få ut random char.
            int num = rnd.Next(0, 26);
            char serialnumber = (char)('a' + num);
            return serialnumber;
        }

        public void ReadDataFile()
        {
            //metod som läser datafilerna vid start
            #region HarborFile
            string text = "";

            try { text = File.ReadAllText("BoatsNotDocked.txt"); BoatsNotDocked = int.Parse(text); }
            catch { BoatsNotDocked = 0; }
            
            try {text = File.ReadAllText("Harbor.txt");}
            catch {StreamWriter sw = new StreamWriter("Harbor.txt", false);sw.Close();}
            string[] valuesInHarbor = text.Split('\n');

            foreach (string v in valuesInHarbor)
            {
                try
                {
                    string[] keyValue = v.Split(',');
                    int place = int.Parse(keyValue[0]);
                    int type = int.Parse(keyValue[1]);
                    string serialnumber = keyValue[2];
                    int weight = int.Parse(keyValue[3]);
                    double speed = double.Parse(keyValue[4]);
                    int daysstay = int.Parse(keyValue[5]);
                    int daysbeen = int.Parse(keyValue[6]);
                    int special = int.Parse(keyValue[7]);
                    
                    if (type == 0)
                    {
                        harbor.Add(new Rowboat(place, type, serialnumber, weight, speed, daysstay, daysbeen, special));
                    }
                    else if (type == 1)
                    {
                        harbor.Add(new Motorboat(place, type, serialnumber, weight, speed, daysstay, daysbeen, special));
                    }
                    else if (type == 2)
                    {
                        harbor.Add(new Sailboat(place, type, serialnumber, weight, speed, daysstay, daysbeen, special));
                    }
                    else if (type == 3)
                    {
                        harbor.Add(new Catamaran(place, type, serialnumber, weight, speed, daysstay, daysbeen, special));
                    }
                    else if (type == 4)
                    {
                        harbor.Add(new Cargoboat(place, type, serialnumber, weight, speed, daysstay, daysbeen, special));
                    }
                }
                catch { }
            }
            #endregion

            try { text = File.ReadAllText("Dock1.txt"); }
            catch { StreamWriter sw1 = new StreamWriter("Dock1.txt", false); sw1.Close(); }
            string[] valuesInDock = text.Split('\n');
            int i = 0;

            foreach (var b in valuesInDock)
            {
                try { dock1[i] = bool.Parse(b); i++; }
                catch (FormatException) { break; }
            }

            try { text = File.ReadAllText("Dock2.txt"); }
            catch { StreamWriter sw2 = new StreamWriter("Dock2.txt", false); sw2.Close(); }
            valuesInDock = text.Split('\n');
            i = 0;

            foreach (var b in valuesInDock)
            {
                try { dock2[i] = bool.Parse(b); i++; }
                catch (FormatException) { break; }
            }

        }
        public void DockDataFiles()
        {
            //metod som uppdaterar dock1 och dock2 filerna

            StreamWriter sw1 = new StreamWriter("Dock1.txt", false);
            StreamWriter sw2 = new StreamWriter("Dock2.txt", false);

            foreach (var b in dock1)
            {
                sw1.WriteLine(b);
            }

            foreach (var b in dock2)
            {
                sw2.WriteLine(b);
            }

            sw1.Close();
            sw2.Close();
        }

        public enum Type
        {
            R,
            M,
            S,
            K,
            L,
        }

        private void numberOfBoats_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            numberOfBoats.Text = "";
        }
        private void nextDayButton_Button_Click(object sender, RoutedEventArgs e)
        {
            int BoatsToSpawn = 0;   
            
            try
            {
                BoatsToSpawn = int.Parse(numberOfBoats.Text);
                if(BoatsToSpawn > 20)
                {
                    BoatsToSpawn = 20;
                    numberOfBoats.Text = "20";
                }
                else if (BoatsToSpawn < 0)
                {
                    BoatsToSpawn = 0;
                    numberOfBoats.Text = "0";
                }
            }
            catch
            {
                BoatsToSpawn = 5;
                numberOfBoats.Text = "5";
            }
            Update(BoatsToSpawn);
        }
        private void UIElement_MouseEnter(object sender, MouseEventArgs e)
        {

            string selectedElement = ((Rectangle)sender).Name;

            foreach (var b in harbor)
            {
                if (selectedElement == $"{Enum.GetName(typeof(Type), b.Type)}" + b.SerialNumber)
                {
                    double km = Math.Round((b.Speed*1.8), 1, MidpointRounding.ToEven);

                    if (b is Rowboat)
                    {
                        singleBoatTitle.Content = "Roddbåt";
                        SingleBoatLabel.Content = $"Serienummer: {Enum.GetName(typeof(Type), b.Type)}-" + b.SerialNumber  + ('\n') + $"Vikt: {b.Weight} kg" + ('\n') + $"Hastighet: {km} km/h" + ('\n') + $"Max Passagerare: {b.Special()} st";
                    }
                    else if (b is Motorboat)
                    {
                        singleBoatTitle.Content = "Motorbåt";
                        SingleBoatLabel.Content = $"Serienummer: {Enum.GetName(typeof(Type), b.Type)}-" + b.SerialNumber + ('\n') + $"Vikt: {b.Weight} kg" + ('\n') + $"Hastighet: {km} km/h" + ('\n') + $"Hästkrafter: {b.Special()} hk";
                    }
                    else if (b is Sailboat)
                    {
                        singleBoatTitle.Content = "Segelbåt";
                        SingleBoatLabel.Content = $"Serienummer: {Enum.GetName(typeof(Type), b.Type)}-" + b.SerialNumber + ('\n') + $"Vikt: {b.Weight} kg" + ('\n') + $"Hastighet: {km} km/h" + ('\n') + $"Längd: {b.Special()} m";
                    }
                    else if (b is Catamaran)
                    {
                        singleBoatTitle.Content = "Katamaran";
                        SingleBoatLabel.Content = $"Serienummer: {Enum.GetName(typeof(Type), b.Type)}-" + b.SerialNumber + ('\n') + $"Vikt: {b.Weight} kg" + ('\n') + $"Hastighet: {km} km/h" + ('\n') + $"Sängar: {b.Special()} st";
                    }
                    else if(b is Cargoboat)
                    {
                        singleBoatTitle.Content = "Lastfartyg";
                        SingleBoatLabel.Content = $"Serienummer: {Enum.GetName(typeof(Type), b.Type)}-" + b.SerialNumber + ('\n') + $"Vikt: {b.Weight} kg" + ('\n') + $"Hastighet: {km} km/h" + ('\n') + $"Containers: {b.Special()} st";
                    }
                }
            }

        }
        private void UIElement_MouseLeave(object sender, MouseEventArgs e)
        {
            singleBoatTitle.Content = "";
            SingleBoatLabel.Content = "";
        }
        private void reset_Button_Click(object sender, RoutedEventArgs e)
        {
            RemoveBoats(harbor);
            StreamWriter sw1 = new StreamWriter("Dock1.txt", false);
            sw1.Write("");
            sw1.Close();
            sw1 = new StreamWriter("Dock2.txt", false);
            sw1.Write("");
            sw1.Close();
            sw1 = new StreamWriter("Harbor.txt", false);
            sw1.Write("");
            sw1.Close();
            sw1 = new StreamWriter("BoatsNotDocked.txt", false);
            sw1.Write("");
            sw1.Close();

            harbor = new List<Boat>();
            newBoats = new List<Boat>();
            BoatsNotDocked = 0;

            sammanfattningLabel.Content = $"Roddbåtar: 0" + ('\n') +
                $"Motorbåtar: 0" + ('\n') +
                $"Segelbåtar: 0" + ('\n') +
                $"Katamaraner: 0" + ('\n') +
                $"Lastfartyg: 0" + ('\n') + ('\n') +
                $"Totalvikt: 0 kg" + ('\n') +
                $"Medelhastighet: 0 km/h" + ('\n') +
                $"Lediga platser: 0" + ('\n') +
                $"Avvisade båtar: 0";

            ReadDataFile();

        }
    }
}
