
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var array = [];
var games = [];
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
            game.counter = array[i].games_count;
            games.push(game);
        }
        console.log(games);
 //       var src = document.getElementById("gamediv");
   //     var img = document.createElement("img");
   //     img.src = array[0].image_background;
   //     src.appendChild(img);
    });
};
