
$(document).ready(function () {
    $('.autocompleteSelect').select2({
        minimumInputLength: 3
    });
    $('#inputProducer').on('keyup', complete);
    function complete() {
        let input, filter, table, cards, txtValue;
        input = document.getElementById("inputProducer");
        filter = input.value.toUpperCase();
        table = document.getElementById("mainTable");
        cards = table.getElementsByClassName("card");

        for (let i = 0; i < cards.length; ++i) {
            let prod = cards[i].childNodes[1].childNodes[1].childNodes[1].childNodes[1].childNodes[3].childNodes[1];
            txtValue = prod.innerText.split(' ')[0];
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                cards[i].style.display = "";
            } else {
                cards[i].style.display = "none";
            }
        }
    }
});


