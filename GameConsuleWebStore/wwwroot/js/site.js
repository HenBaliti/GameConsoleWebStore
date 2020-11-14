
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var array = [];
var games = [];
var textH5 = document.createTextNode("Company Name: ");
var textP = document.createTextNode("Games Owned: ");
jQuery(document).ready(function () {

    const $valueSpan = $('.range-slider__value');
    const $value = $('#RangePrice');
    $valueSpan.html($value.val());
    $value.on('input change', () => {

        $valueSpan.html($value.val());
    });
});
window.onload = function () {
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "https://rawg-video-games-database.p.rapidapi.com/developers",
        "method": "GET",
        "headers": {
            "x-rapidapi-host": "rawg-video-games-database.p.rapidapi.com",
            "x-rapidapi-key": "f6f05d56dcmsh4deccfd05227522p1c83efjsn04ee8a8c6cea"
        }
    }

    $.ajax(settings).done(function (response) {
        array = response.results;
        console.log(response);
       
        for (var i = 0; i < array.length; i++) {
            var game = new Object();
            game.img = array[i].image_background;
            game.name = array[i].name;
            game.games_count = array[i].games_count;
            games.push(game);
        }
        draw(games);

    });
 
    function draw(games) {
        var card = createCard();
        editCard(card, games);
        appendCard(card);

    }
    function createCard() {
        const card = {
            cardDiv: document.createElement("div"),
            cardSecondDiv: document.createElement("div"),
            cardImg: document.createElement("img"),
            cardH5: document.createElement("h5"),
            cardP: document.createElement("p"),
          
            
        }
    
        return card;
    }
    function editCard(card, games) {
        //console.log(games);
        const { cardDiv, cardSecondDiv, cardImg, cardH5, cardP } = card;
        var i = Math.floor(Math.random() * 11);
        cardDiv.className = "card bg-dark text-white";
        cardDiv.style.width = "450px"
        cardSecondDiv.className = "card-img-overlay";
        cardImg.src = games[i].img;
        cardImg.className = "card-img";
        cardImg.style.height = "205px";
        cardImg.style.width = "205px";
        cardImg.style.position = "relative";
        cardImg.style.right = "-241px";
        cardImg.style.borderRadius = "25px";
        cardH5.innerText = games[i].name;
        cardH5.className = "card-title";
        cardH5.style.textShadow = " 3px 3px 5px white";
        //cardH5.style = "margin-left:440px";
        //cardP.style = "margin-left:440px";
        cardP.className = "card-text";
        cardP.style.textShadow = " 3px 3px 5px white";
        cardP.innerText = games[i].games_count;
        card.style = "margin-top: 20px"
      

    }
    function appendCard(card) {
        const { cardDiv, cardSecondDiv, cardImg, cardH5, cardP } = card;
        cardDiv.append(cardSecondDiv, cardImg);
        cardSecondDiv.append(cardH5, cardP);
        //textH5 = document.createTextNode("Company Name: ");
        //TextP = document.createTextNode("Games Owned: ");
        cardH5.parentNode.insertBefore(textH5, cardH5);
        cardP.parentNode.insertBefore(textP, cardP);
        var gamed = document.getElementById("gamediv");
        gamed.append(cardDiv);
    }

        document.getElementById("btnnews").addEventListener("click", function () {
            document.getElementById("newsemail").value = "";
            document.getElementById("subscribeSpan").innerText = "Subscribed Successfully"
        });
        
}
