let map;
let locations = [];
let currentId;

function initLocations(data) {
    for (let i = 0; i < data.length; ++i) {
        locations[i] = {};
        locations[i].latitude = data[i].latitude;
        locations[i].longitude = data[i].longitude;
        locations[i].address = data[i].address;
        locations[i].warehouseId = data[i].warehouseId;
    }

    var infowindow = new google.maps.InfoWindow();

    const icons = {
        current: {
            icon: "/images/icons/icons8-marker-50.png",
        }
    };

    currentId =  $('#warehouseId').attr("data-id");

    for (let i = 0; i < locations.length; i++) {
        if (locations[i].warehouseId == currentId) {
            marker = new google.maps.Marker({
                position: new google.maps.LatLng(locations[i].latitude, locations[i].longitude),
                icon: icons["current"].icon,
                map: map,
            });
        }
        else {
            marker = new google.maps.Marker({
                position: new google.maps.LatLng(locations[i].latitude, locations[i].longitude),
                map: map,
            });
        }

        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                infowindow.setContent(locations[i].warehouseId == currentId ? "(Current warehouse) " + locations[i].address: locations[i].address);
                infowindow.open(map, marker);
            }
        })(marker, i));
    }
}
function initMap() {

    
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 50.450001, lng: 30.523333 },
        zoom: 4,
    });
   
    fetch("https://localhost:44326/api/WarehousesApi").then(response => response.json()).then(data => initLocations(data));

}

function setId(Id) {
    currentId = Id;
}