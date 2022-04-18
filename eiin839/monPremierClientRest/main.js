// JavaScript source code

function getApiKey() {
    // Let's first retrieve the input:
    var input = document.getElementById("apiKey");
    return input.value;
}


function callAPI(url, requestType, params, finishHandler) {
    var fullUrl = url;

    // Ajout des paramètres dans l'url
    if (params) {
        fullUrl += "?" + params.join("&");
    }

    // Appel aux serveurs externes : XMLHttpRequest
    var request = new XMLHttpRequest();
    request.open(requestType, fullUrl, true);
    // Seulement les requêtes dont la réponse est OK
    request.setRequestHeader("Accept", "application/json");
    // Onload ==> fonction à appeler à la fin
    request.onload = contratsRetrieved;

    request.send();
}

function contratsRetrieved() {
    if (status !== 200) {
        console.log("ERROR ERROR ERROR");
    }
    else {
        var response = JSON.parse(this.responseText);
        var contrats = responseObject.map(function (contract) {
            return contract.name;
        });
        var list = getElementById("contractsList");
        for (contract in list) {
            var option = doc.createElement('option').setAttribute(contract.name, contract.name);
            var elementAjoute = doc.getElementById("choices").appendChild(option);
        }
    }
}

function retrieveAllContracts()
{
    var params = ["apiKey=" + getApiKey()];
    var onFinish = contratsRetrieved;
    callAPI("https://api.jcdecaux.com/vls/v3/contracts", "GET", params, onFinish);
}

function retrieveAllContractsStations() {
    var params = ["apiKey=" + getApiKey()];
    var onFinish = contratsRetrieved;
    callAPI("https://api.jcdecaux.com/vls/v3/contracts", "GET", params, onFinish);
}

