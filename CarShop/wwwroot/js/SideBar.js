$(document).ready(function () {
    var trigger = $('.hamburger'),
        overlay = $('.overlay'),
        isClosed = false;

    trigger.click(function () {
        hamburger_cross();
    });

    function hamburger_cross() {

        if (isClosed == true) {
            overlay.hide();
            trigger.removeClass('is-open');
            trigger.addClass('is-closed');
            isClosed = false;
        } else {
            overlay.show();
            trigger.removeClass('is-closed');
            trigger.addClass('is-open');
            isClosed = true;
        }
    }

    const a = document.getElementById('categoriesButton');
    a.addEventListener('click', getCategories);

    function getCategories() {
        fetch('https://localhost:44323/api/Categories/AllCategories')
            .then(response => response.json())
            .then(data => displayCategories(data));
    }

    function displayCategories(data) {
        const ul = document.getElementById('categories');
        if (ul.childElementCount == 0) {
            for (let i = 0; i < data.length; ++i) {
                let li = document.createElement('li');
                let li_a = document.createElement('a');
                li_a.textContent = data[i].name;
                li_a.href = `/Cars/Index/${data[i].categoryId}`;
                li.appendChild(li_a);
                ul.appendChild(li);
            }
        }
    }

    $('[data-toggle="offcanvas"]').click(function () {
        $('#wrapper').toggleClass('toggled');
    });
});