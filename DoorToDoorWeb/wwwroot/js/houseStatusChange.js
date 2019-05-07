const statusSelect = document.getElementById("house-status-select");
const statusResponseElement = document.getElementById("house-status-update-response");

function updateHouseStatus(event) {
    event.stopPropagation();

    let selectedHouseStatus = Number.parseInt(statusSelect.options[statusSelect.selectedIndex].value);

    let data = {
        HouseID: houseID,
        StatusID: selectedHouseStatus
    }

    fetch(d2dServerUrl + "Salesperson/SetHouseStatus", {
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
                statusResponseElement.innerText = responseData.message;

                if (responseData.updateSuccessful) {
                    statusResponseElement.setAttribute("class", "reset-success");
                }
                else {
                    statusResponseElement.setAttribute("class", "field-validation-error");
                }
            }
            else {
                statusResponseElement.innerText = responseData.error;
                statusResponseElement.setAttribute("class", "field-validation-error");
            }
        });
}

statusSelect.addEventListener('change', updateHouseStatus);

