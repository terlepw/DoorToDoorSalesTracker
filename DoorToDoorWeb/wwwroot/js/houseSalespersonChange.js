const salespersonSelect = document.getElementById("assigned-salesperson-select");
const salespersonResponseElement = document.getElementById("assigned-salesperson-update-response");

function reassignHouseSalesperson(event) {
    event.stopPropagation();

    let selectedSalesperson = Number.parseInt(salespersonSelect.options[salespersonSelect.selectedIndex].value);

    let data = {
        HouseID: houseID,
        SalespersonID: selectedSalesperson
    }

    fetch(d2dServerUrl + "Manager/ReassignHouseSalesperson", {
        method: "POST",
        body: JSON.stringify(data),
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then((response) => {
            return response.json();
        })
        .then(function (responseData) {
            if (responseData.updateSuccessful != undefined) {
                salespersonResponseElement.innerText = responseData.message;

                if (responseData.updateSuccessful) {
                    salespersonResponseElement.setAttribute("class", "reset-success");
                }
                else {
                    salespersonResponseElement.setAttribute("class", "field-validation-error");
                }
            }
            else {
                salespersonResponseElement.innerText = responseData.error;
                salespersonResponseElement.setAttribute("class", "field-validation-error");
            }
        });
}

salespersonSelect.addEventListener('change', reassignHouseSalesperson);

