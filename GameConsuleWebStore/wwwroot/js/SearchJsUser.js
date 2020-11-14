$(function () {
    $("#DivLoading").hide();
    $("#SearchBoxInput").autocomplete({
        search: function () { $("#DivLoading").show() },
        open: function () { $("#DivLoading").hide() },
        source: "/Users/SearchAutoComplete",
        minLength: 2,
        select: function (event, ui) {
            location.href = '/Users/Details/' + ui.item.id;
        }
    });
});