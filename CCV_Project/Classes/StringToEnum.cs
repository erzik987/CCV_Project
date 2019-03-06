using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCV_Project.Classes
{
    public class StringToEnum
    {
        public Models.StoreHouseType GetStoreHouseTypeFromString(string number)
        {
            switch (number)
            {
                case "0":
                    return Models.StoreHouseType.Dry;
                case "1":
                    return Models.StoreHouseType.Frish;
                case "2":
                    return Models.StoreHouseType.OG;
                default: return Models.StoreHouseType.Dry;
            }
        }
    }
}