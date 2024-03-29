﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace RealEstate.App.Constants
{
    public class CityConstants
    {
        public string Name;
        public static List<CityConstants> _cities = new List<CityConstants>()
        {
            new CityConstants {Name="Prishtinë"},
            new CityConstants {Name="Gjilan"},
            new CityConstants {Name="Prizren"},
            new CityConstants {Name="Pejë"},
            new CityConstants {Name="Ferizaj"},
            new CityConstants {Name="Mitrovice"},
            new CityConstants {Name="Gjakovë"},
            new CityConstants {Name="Malishevë"},
            new CityConstants {Name="Podujevë"},
            new CityConstants {Name="Vushtrri"},
            new CityConstants {Name="Skënderaj"},
            new CityConstants {Name="Viti"},
            new CityConstants {Name="Suharekë"}
        };

        //public static List<string> _cities = new List<string>() { "Prishtine", "Gjilan", "Prizren", "Ferizaj", "Pejë", "Mitrovice", "Gjakovë", "Malishevë", "Podujevë", "Vushtrri", "Skënderaj", "Viti", "Suharekë" };
    }
}
