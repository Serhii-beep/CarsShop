﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function cardClick(carId) {
    window.location.href = 'https://carshop12.azurewebsites.net/Cars/Details/?id=' + carId;
}