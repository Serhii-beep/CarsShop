
$(document).ready(function () {
    const table = document.getElementById("mainTable");
    const cards = table.getElementsByClassName("card");
    const producerInput = document.getElementById("inputProducer");
    const yearInput = document.getElementById("inputYear");
    const maxPriceInput = document.getElementById("inputMaxPrice");
    const minPriceInput = document.getElementById("inputMinPrice");
    let textProducer = producerInput.value.toUpperCase();
    let textYear = yearInput.value;
    let textMaxPrice = maxPriceInput.value;
    let textMinPrice = minPriceInput.value;

    producerInput.addEventListener('keyup', (e) => {
        textProducer = e.target.value.toUpperCase();
        autocomplete();
    });

    yearInput.addEventListener('keyup', (e) => {
        textYear = parseInt(e.target.value);
        autocomplete();
    });

    maxPriceInput.addEventListener('keyup', (e) => {
        textMaxPrice = parseInt(e.target.value);
        autocomplete();
    });

    minPriceInput.addEventListener('keyup', (e) => {
        textMinPrice = parseInt(e.target.value);
        autocomplete();
    });
    
    function autocomplete() {
        for (let i = 0; i < cards.length; ++i) {
            let cardProducer = cards[i].getAttribute("data-producer");
            let cardYear = cards[i].getAttribute("data-year");
            let cardPrice = cards[i].getAttribute("data-price");
            let matches = true;
            if (textProducer) {
                if (cardProducer.toUpperCase().indexOf(textProducer.toUpperCase()) <= -1) {
                    matches = false;
                }
            }
            if (textYear) {
                if (cardYear.toString().indexOf(textYear.toString()) <= -1) {
                    matches = false;
                }
            }
            if (textMinPrice) {
                if (cardPrice < textMinPrice) {
                    matches = false;
                }
            }
            if (textMaxPrice) {
                if (cardPrice > textMaxPrice) {
                    matches = false;
                }
            }
            if (matches) {
                cards[i].style.display = "";
            } else {
                cards[i].style.display = "none";
            }
        }
    }
});


