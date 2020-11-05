using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls.Primitives;

namespace TheHarbor2
{
    public class Boat
    {
        public int Place { get; set; }
        public int Type { get; set; }
        public string SerialNumber { get; set; }
        public int Weight { get; set; }
        public double Speed { get; set; }
        public int DaysStay { get; set; }
        public int DaysBeen { get; set; }

        public virtual int Special() { return 1; }
    }

    class Rowboat : Boat //type 0
    {
        public int Passengers;

        public Rowboat(int place, int type, string serialnumber, int weight, double speed, int daystay, int daysbeen, int passengers)
        {
            Place = place;
            Type = type;
            SerialNumber = serialnumber;
            Weight = weight;
            Speed = speed;
            DaysStay = daystay;
            Passengers = passengers;
            DaysBeen = daysbeen;
        }

        public override int Special()
        {
            return Passengers;
        }

    }

    class Motorboat : Boat //type 1
    {
        public int HorsePower { get; set; }

        public Motorboat(int place, int type, string serialnumber, int weight, double speed, int daystay, int daysbeen, int horsepower)
        {
            Place = place;
            Type = type;
            SerialNumber = serialnumber;
            Weight = weight;
            Speed = speed;
            DaysStay = daystay;
            HorsePower = horsepower;
            DaysBeen = daysbeen;
        }

        public override int Special()
        {
            return HorsePower;
        }
    }

    class Sailboat : Boat //type 2
    {
        public int Length { get; set; }

        public Sailboat(int place, int type, string serialnumber, int weight, double speed, int daystay, int daysbeen, int length)
        {
            Place = place;
            Type = type;
            SerialNumber = serialnumber;
            Weight = weight;
            Speed = speed;
            DaysStay = daystay;
            Length = length;
            DaysBeen = daysbeen;
        }

        public override int Special()
        {
            return Length;
        }
    }

    class Catamaran : Boat //type 3
    {
        public int Beds { get; set; }

        public Catamaran(int place, int type, string serialnumber, int weight, double speed, int daystay, int daysbeen, int beds)
        {
            Place = place;
            Type = type;
            SerialNumber = serialnumber;
            Weight = weight;
            Speed = speed;
            DaysStay = daystay;
            Beds = beds;
            DaysBeen = daysbeen;
        }

        public override int Special()
        {
            return Beds;
        }
    }

    class Cargoboat : Boat //type 4
    {
        public int Containers { get; set; }

        public Cargoboat(int place, int type, string serialnumber, int weight, double speed, int daystay, int daysbeen, int containers)
        {
            Place = place;
            Type = type;
            SerialNumber = serialnumber;
            Weight = weight;
            Speed = speed;
            DaysStay = daystay;
            Containers = containers;
            DaysBeen = daysbeen;
        }

        public override int Special()
        {
            return Containers;
        }
    }
}
