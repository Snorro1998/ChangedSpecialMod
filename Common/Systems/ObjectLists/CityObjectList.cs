using System.Collections.Generic;
using Terraria;

namespace ChangedSpecialMod.Common.Systems.ObjectLists
{
    public class CityObjectList
    {
        public static List<CityObject> GetElements()
        {
            var citiesEurope = new List<CityObject>()
            {
                // Albania
                // Andorra
                // Armenia
                // Austria
                new CityObject() { name = "Vienna" },
                new CityObject() { name = "Graz" },
                new CityObject() { name = "Linz" },
                new CityObject() { name = "Salzburg" },
                // Azerbaijan
                // Belarus
                // Belgium
                new CityObject() { name = "Antwerp" },
                new CityObject() { name = "Bruges", famousFood = "chocolates" },
                new CityObject() { name = "Brussels" },
                new CityObject() { name = "Mechelen" },
                // Bosnia and Herzegovina
                // Bulgaria
                new CityObject() { name = "Sofia" },
                new CityObject() { name = "Plovdiv" },
                new CityObject() { name = "Varna" },
                new CityObject() { name = "Burgas" },
                // Croatia
                new CityObject() { name = "Zagreb" },
                new CityObject() { name = "Split" },
                new CityObject() { name = "Rijeka" },
                new CityObject() { name = "Osijek" },
                // Cyprus
                // Czechia
                // Denmark
                new CityObject() { name = "Copenhagen" },
                new CityObject() { name = "Aarhus" },
                new CityObject() { name = "Odense" },
                new CityObject() { name = "Aalborg" },
                // England
                new CityObject() { name = "Birmingham" },
                new CityObject() { name = "Liverpool" },
                new CityObject() { name = "London", landmark = "Big Ben" },
                // Estonia
                // Finland
                new CityObject() { name = "Helsinki" },
                new CityObject() { name = "Oulou" },
                new CityObject() { name = "Turku" },
                new CityObject() { name = "Espoo" },
                // France
                new CityObject() { name = "Paris", landmark = "Eiffel Tower" },
                new CityObject() { name = "Lyon" },
                new CityObject() { name = "Marseille" },
                new CityObject() { name = "Toulouse" },
                // Georgia
                // Germany
                new CityObject() { name = "Berlin" },
                new CityObject() { name = "Cologne" },
                new CityObject() { name = "Dortmund" },
                new CityObject() { name = "Hamburg", famousFood = "hamburgers" },
                new CityObject() { name = "Munich" },
                // Greece
                new CityObject() { name = "Athens" },
                new CityObject() { name = "Thessaloniki" },
                new CityObject() { name = "Patras" },
                new CityObject() { name = "Heraklion" },
                // Greenland (Denmark)
                // Hungary
                // Iceland
                // Ireland
                // Italy
                new CityObject() { name = "Brindisi" },
                new CityObject() { name = "Florence" },
                new CityObject() { name = "Milan" },
                new CityObject() { name = "Napels" },
                new CityObject() { name = "Palermo" },
                new CityObject() { name = "Pisa" },
                new CityObject() { name = "Rome" },
                new CityObject() { name = "Turin" },
                new CityObject() { name = "Venice" },
                // Kosovo
                // Latvia
                // Liechtenstein
                // Lithuania
                // Luxembourg
                // Malta
                // Moldova
                // Monaco
                // Montenegro
                // Netherlands
                new CityObject() { name = "Amersfoort" },
                new CityObject() { name = "Amsterdam" },
                new CityObject() { name = "Gouda", famousFood = "cheese" },
                new CityObject() { name = "Rotterdam" },
                new CityObject() { name = "Soesterberg" },
                new CityObject() { name = "The Hague" },
                new CityObject() { name = "Utrecht", landmark = "Dom Tower of Utrecht" },
                // North Macedonia
                // Norway
                new CityObject() { name = "Bergen" },
                new CityObject() { name = "Oslo" },
                new CityObject() { name = "Stavanger" },
                new CityObject() { name = "Trondheim" },
                // Poland
                new CityObject() { name = "Gdansk" },
                new CityObject() { name = "Krakow" },
                new CityObject() { name = "Lodz" },
                new CityObject() { name = "Lubin" },
                new CityObject() { name = "Warsaw" },
                new CityObject() { name = "Wroclaw" },
                // Portugal
                new CityObject() { name = "Lissabon" },
                new CityObject() { name = "Porto" },
                // Romania
                // Russia
                new CityObject() { name = "Moscow" },
                new CityObject() { name = "Novosibirsk" },
                new CityObject() { name = "Omsk" },
                new CityObject() { name = "Saint Petersburg" },
                new CityObject() { name = "Yekaterinburg" },
                // San Marino
                // Serbia
                // Slovakia
                // Slovenia
                // Spain
                new CityObject() { name = "Barcelona" },
                new CityObject() { name = "Gibraltar" },
                new CityObject() { name = "Madrid" },
                new CityObject() { name = "Malaga" },
                new CityObject() { name = "Pamplona" },
                new CityObject() { name = "Seville" },
                new CityObject() { name = "Valencia" },
                // Scotland
                new CityObject() { name = "Edinburgh" },
                new CityObject() { name = "Glasgow" },
                // Sweden
                new CityObject() { name = "Gotenburg" },
                new CityObject() { name = "Malmo" },
                new CityObject() { name = "Stockholm" },
                new CityObject() { name = "Uppsala" },
                // Switzerland
                // Turkey
                // Ukraine
                new CityObject() { name = "Charkov" },
                new CityObject() { name = "Kiev" },
                new CityObject() { name = "Lviv" },
                new CityObject() { name = "Tsjernobyl" },
                // United Kingdom
                // Vatican City
            };

            var citiesAfrica = new List<CityObject>()
            {
                // Algeria
                new CityObject() { name = "Algiers" },
                new CityObject() { name = "Oran" },
                new CityObject() { name = "Constantine" },
                new CityObject() { name = "Annaba" },
                // Angola
                new CityObject() { name = "Luanda" },
                new CityObject() { name = "Huambo" },
                new CityObject() { name = "Lobito" },
                new CityObject() { name = "Benguela" },
                // Benin
                new CityObject() { name = "Cotonou" },
                new CityObject() { name = "Porto-Novo" },
                new CityObject() { name = "Abomey-Calavi" },
                new CityObject() { name = "Parakou" },
                // Botswana
                new CityObject() { name = "Gaborone" },
                new CityObject() { name = "Francistown" },
                new CityObject() { name = "Molepolole" },
                new CityObject() { name = "Maun" },
                // Burkina Faso
                // Burundi
                // Cabo Verde
                // Cameroon
                // Central African Republic
                // Chad
                // Comoros
                // Democratic Republic of the Congo
                // Republic of the Congo
                // Cote d'Ivoire
                // Djibouti
                // Egypt
                // Equatorial Guinea
                // Eritrea
                // Eswatini
                // Ethiopia
                // Gabon
                // The Gambia
                // Ghana
                // Guinea
                // Guinea-Bissau
                // Kenya
                // Lesotho
                // Liberia
                // Libya
                // Madagascar
                // Malawi
                // Mali
                // Mauritania
                // Mauritius
                new CityObject() { name = "Port Louis" },
                new CityObject() { name = "Rose-Belle" },
                new CityObject() { name = "Vacoas-Phoenix" },
                new CityObject() { name = "Curepipe" },
                // Morocco
                // Mozambique
                // Namibia
                // Niger
                // Nigeria
                // Rwanda
                // Sao Tome and Principe
                // Senegal
                // Seychelles
                // Sierra Leone
                // Somalia
                // South Africa
                // South Sudan
                // Sudan
                // Tanzania
                // Togo
                // Tunisia
                // Unganda
                // Zambia
                // Zimbabwe
            };

            var result = new List<CityObject>();
            result.AddRange(citiesEurope);
            result.AddRange(citiesAfrica);
            return result;
        }
    }

    public class CityObject
    {
        public string name = "city";
        public string largestRelion = "";
        public string famousFood = "";
        public string landmark = "";

        public string GetReligousBuilding()
        {
            if (largestRelion == "islam")
                return "mosque";

            return "church";
        }

        public string GetFood()
        {
            if (famousFood != string.Empty)
                return famousFood;

            var options = new List<string>()
            {
                "bread",
                "cupcakes",
                "muffins",
                "pancakes",
                "pizzas",
            };

            return Utils.SelectRandom(Main.rand, options.ToArray());
        }
    }
}
