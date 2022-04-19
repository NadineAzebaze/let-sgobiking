

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

function initialize( equipments) {
    var carte = L.map("divcarte").setView([49.03898, 2.07501], 16);
    var tuiles = L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        attribution: 'Fond de carte par les contributeurs <a href="https://www.openstreetmap.org/">OpenStreetMap</a>,<a href=" https://creativecommons.org/licenses/by-sa/2.0/ ">CC-BY-SA</a>'
    }).addTo(carte);

    var equipments_lyr =
        L.geoJSON(equipments, {
            pointToLayer: function (feature, latlng) {
                console.log(latlng);
                return L.marker(latlng);
            }
        }).addTo(carte);
}



function sendAdress() {
    var origin = document.getElementById("origin").value;
    var destination = document.getElementById("destination").value;
    var geoJson = {};
    var url = "http://localhost:8732/Design_Time_Addresses/ServeurRouting/Service1/rest/"
    fetch(url + "ComputeItinerary?origin=" + origin + "&destination=" + destination)
        .then(function (response) {
            geoJson = response.json();
            return geoJson;
        }).
        then(function (data) {
            console.log(data);
            this.initialize(geoJson);
        });

}

