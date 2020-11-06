$(function () {
    $("#DivLoading").hide();
    $("#SearchBoxInput").autocomplete({
        search: function () { $("#DivLoading").show() },
        open: function () { $("#DivLoading").hide() },
      source: "/Home/SearchAutoComplete",
      minLength: 2,
          select: function (event, ui) {
              location.href = '/Products/Details/'+ui.item.id;
      }
    });
});