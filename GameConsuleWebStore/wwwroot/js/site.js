
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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
		//console.log(array[0].name);
		//var src = document.getElementById("gamediv");
		//var img = document.createElement("img");
		//img.src = array[0].image_background;
		//src.appendChild(img);
	});
};
