

function getOrigin() {
    // Let's first retrieve the input:
    var input = document.getElementById("origin");
    return input.value;
}


function getDestination() {
    // Let's first retrieve the input:
    var input = document.getElementById("destination");
    return input.value;
}

function initialize() {
    var map = L.map('map').setView([48.833, 2.333], 7); // LIGNE 18

    var osmLayer = L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', { // LIGNE 20
        attribution: '© OpenStreetMap contributors',
        maxZoom: 19
    });

    map.addLayer(osmLayer);
}



function sendAdress() {
    var origin = document.getElementById("origin").value;
    var destination = document.getElementById("destination").value;
    var url = "http://localhost:8732/Design_Time_Addresses/ServeurRouting/Service1/rest/"
    fetch(url + "ComputeItinerary?origin=" + origin + "&destination=" + destination)
        .then(function (response) {
            return response.json();
        }).
        then(function (data) {
            console.log(data);
        });

}

