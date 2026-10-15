using ChangedSpecialMod.Common.Systems.ObjectLists;
using System.Collections.Generic;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class CityTagObject : TagObject
    {
        public string famousFood = null;
        public string landmark = null;

        public CityTagObject() : base()
        {
            tagCategory = "City";
        }
    }

    public class CityTagObjectList : TagObjectList<CityTagObject>
    {
        public override void Init()
        {
            var citiesEurope = new List<CityTagObject>()
            {
                // Albania
                // Andorra
                // Armenia
                // Austria
                new CityTagObject() { name = "Vienna" },
                new CityTagObject() { name = "Graz" },
                new CityTagObject() { name = "Linz" },
                new CityTagObject() { name = "Salzburg" },
                // Azerbaijan
                // Belarus
                // Belgium
                new CityTagObject() { name = "Antwerp" },
                new CityTagObject() { name = "Bruges", famousFood = "chocolates" },
                new CityTagObject() { name = "Brussels" },
                new CityTagObject() { name = "Mechelen" },
                // Bosnia and Herzegovina
                // Bulgaria
                new CityTagObject() { name = "Sofia" },
                new CityTagObject() { name = "Plovdiv" },
                new CityTagObject() { name = "Varna" },
                new CityTagObject() { name = "Burgas" },
                // Croatia
                new CityTagObject() { name = "Zagreb" },
                new CityTagObject() { name = "Split" },
                new CityTagObject() { name = "Rijeka" },
                new CityTagObject() { name = "Osijek" },
                // Cyprus
                // Czechia
                // Denmark
                new CityTagObject() { name = "Copenhagen" },
                new CityTagObject() { name = "Aarhus" },
                new CityTagObject() { name = "Odense" },
                new CityTagObject() { name = "Aalborg" },
                // England
                new CityTagObject() { name = "Birmingham" },
                new CityTagObject() { name = "Liverpool" },
                new CityTagObject() { name = "London", landmark = "Big Ben" },
                // Estonia
                // Finland
                new CityTagObject() { name = "Helsinki" },
                new CityTagObject() { name = "Oulou" },
                new CityTagObject() { name = "Turku" },
                new CityTagObject() { name = "Espoo" },
                // France
                new CityTagObject() { name = "Paris", landmark = "Eiffel Tower" },
                new CityTagObject() { name = "Lyon" },
                new CityTagObject() { name = "Marseille" },
                new CityTagObject() { name = "Toulouse" },
                // Georgia
                // Germany
                new CityTagObject() { name = "Berlin" },
                new CityTagObject() { name = "Cologne" },
                new CityTagObject() { name = "Dortmund" },
                new CityTagObject() { name = "Hamburg", famousFood = "hamburgers" },
                new CityTagObject() { name = "Munich" },
                // Greece
                new CityTagObject() { name = "Athens" },
                new CityTagObject() { name = "Thessaloniki" },
                new CityTagObject() { name = "Patras" },
                new CityTagObject() { name = "Heraklion" },
                // Greenland (Denmark)
                // Hungary
                // Iceland
                // Ireland
                // Italy
                new CityTagObject() { name = "Brindisi" },
                new CityTagObject() { name = "Florence" },
                new CityTagObject() { name = "Milan" },
                new CityTagObject() { name = "Napels" },
                new CityTagObject() { name = "Palermo" },
                new CityTagObject() { name = "Pisa" },
                new CityTagObject() { name = "Rome" },
                new CityTagObject() { name = "Turin" },
                new CityTagObject() { name = "Venice" },
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
                new CityTagObject() { name = "Amersfoort" },
                new CityTagObject() { name = "Amsterdam" },
                new CityTagObject() { name = "Gouda", famousFood = "cheese" },
                new CityTagObject() { name = "Rotterdam" },
                new CityTagObject() { name = "Soesterberg" },
                new CityTagObject() { name = "The Hague" },
                new CityTagObject() { name = "Utrecht", landmark = "Dom Tower of Utrecht" },
                // North Macedonia
                // Norway
                new CityTagObject() { name = "Bergen" },
                new CityTagObject() { name = "Oslo" },
                new CityTagObject() { name = "Stavanger" },
                new CityTagObject() { name = "Trondheim" },
                // Poland
                new CityTagObject() { name = "Gdansk" },
                new CityTagObject() { name = "Krakow" },
                new CityTagObject() { name = "Lodz" },
                new CityTagObject() { name = "Lubin" },
                new CityTagObject() { name = "Warsaw" },
                new CityTagObject() { name = "Wroclaw" },
                // Portugal
                new CityTagObject() { name = "Lissabon" },
                new CityTagObject() { name = "Porto" },
                // Romania
                // Russia
                new CityTagObject() { name = "Moscow" },
                new CityTagObject() { name = "Novosibirsk" },
                new CityTagObject() { name = "Omsk" },
                new CityTagObject() { name = "Saint Petersburg" },
                new CityTagObject() { name = "Yekaterinburg" },
                // San Marino
                // Serbia
                // Slovakia
                // Slovenia
                // Spain
                new CityTagObject() { name = "Barcelona" },
                new CityTagObject() { name = "Gibraltar" },
                new CityTagObject() { name = "Madrid" },
                new CityTagObject() { name = "Malaga" },
                new CityTagObject() { name = "Pamplona" },
                new CityTagObject() { name = "Seville" },
                new CityTagObject() { name = "Valencia" },
                // Scotland
                new CityTagObject() { name = "Edinburgh" },
                new CityTagObject() { name = "Glasgow" },
                // Sweden
                new CityTagObject() { name = "Gothenburg" },
                new CityTagObject() { name = "Malmo" },
                new CityTagObject() { name = "Stockholm" },
                new CityTagObject() { name = "Uppsala" },
                // Switzerland
                // Turkey
                // Ukraine
                new CityTagObject() { name = "Charkov" },
                new CityTagObject() { name = "Kiev" },
                new CityTagObject() { name = "Lviv" },
                new CityTagObject() { name = "Tsjernobyl" },
                // United Kingdom
                // Vatican City
            };

            var citiesAfrica = new List<CityTagObject>()
            {
                // Algeria
                new CityTagObject() { name = "Algiers" },
                new CityTagObject() { name = "Oran" },
                new CityTagObject() { name = "Constantine" },
                new CityTagObject() { name = "Annaba" },
                // Angola
                new CityTagObject() { name = "Luanda" },
                new CityTagObject() { name = "Huambo" },
                new CityTagObject() { name = "Lobito" },
                new CityTagObject() { name = "Benguela" },
                // Benin
                new CityTagObject() { name = "Cotonou" },
                new CityTagObject() { name = "Porto-Novo" },
                new CityTagObject() { name = "Abomey-Calavi" },
                new CityTagObject() { name = "Parakou" },
                // Botswana
                new CityTagObject() { name = "Gaborone" },
                new CityTagObject() { name = "Francistown" },
                new CityTagObject() { name = "Molepolole" },
                new CityTagObject() { name = "Maun" },
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
                new CityTagObject() { name = "Port Louis" },
                new CityTagObject() { name = "Rose-Belle" },
                new CityTagObject() { name = "Vacoas-Phoenix" },
                new CityTagObject() { name = "Curepipe" },
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

            elements = [
                .. citiesEurope,
                .. citiesAfrica
            ];
        }
    }
}
