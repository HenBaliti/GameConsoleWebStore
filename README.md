# GameConsoleWebStore
Game Console Web Store - Web project during my second year of the degree using ASP.NET , HTML , CSS , JQUERY , JavaScript , C# , Entity Framework.


Key Features
Admin interface - REST based system, see your most viewed games, customers order locations and how the ML algorithm connected the products.
Recommendation system powerd by hashmap algoritem and previous orders.
Integrated with Twitter, GoogleMaps and IGDB
Beautiful and mobile-friendly interface
Offers the customer a products by his previous orders and games he bought.
Shopping Cart system
GameConsoleWebStore uses a number of open source projects to work properly:

ASP.Net Core - is a free and open-source web framework
IGBD - our source for images, descreption, videos of games.
3d.js - great javascript library for graphing stuff
Twitter - using an api tweeter for sending tweets
jQuery - is a JavaScript library designed to simplify the client-side scripting of HTML.
GameConsoleWebStore requires ASP.Net and SQL Server to run.

open ./GameConsoleWebStore/appsetting.json and make sure to replace all "Your Key" with the relevant API keys.
open the sln file on the main folder with Visual Studio, and type the following command in the NuGet console
$ Update-Database
This will update your local SQL Server 30 game loaded from IGDB, give them random prices and create admin user.

![HomePage](https://github.com/[HenBaliti]/[GameConsoleWebStore]/blob/[master]/HomePage.png?raw=true)

![Alt text](relative/path/to/HomePage.png?raw=true "Title")

This project was made for our Web Application Course and was not made for commercial use.
