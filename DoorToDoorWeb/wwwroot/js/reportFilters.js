const addressFilter = document.getElementById("address-filter");
const fromDateFilter = document.getElementById("date-filter1");
const toDateFilter = document.getElementById("date-filter2");
const fromAmountFilter = document.getElementById("amount-filter1");
const toAmountFilter = document.getElementById("amount-filter2");
const productFilter = document.getElementById("product-filter");
const salespersonFilter = document.getElementById("salesperson-filter");
const filterButton = document.getElementById("report-filter-button");
const filterClear = document.getElementById("report-filter-clear");
const reportList = document.getElementById("report-list");
const reportHeader = document.getElementById("report-header");

function getFormattedDate(dateObject) {
    let month = dateObject.getMonth() + 1;
    let day = dateObject.getDate();
    let year = dateObject.getFullYear();
    let dayNight = "AM";
    let hour = dateObject.getHours();
    if (hour == 0) {
        hour = 12;
    }
    else if (hour == 12) {
        dayNight = "PM";
    }
    else if (hour > 12) {
        dayNight = "PM";
        hour -= 12;
    }
    let minute = dateObject.getMinutes();
    let second = dateObject.getSeconds();

    return `${month}/${day}/${year} ${hour}:${minute}:${second} ${dayNight}`;
}

function applyReportFilter(event) {
    event.preventDefault();
    event.stopPropagation();

    while (reportList.firstChild) {
        reportList.firstChild.remove();
    }
    
    reportList.insertAdjacentElement("beforeend", reportHeader);

    let filteredData = reportData

    if (addressFilter.value != "") {
        filteredData = filteredData.filter((reportItem) => {
            return reportItem.address.includes(addressFilter.value.toLowerCase());
        });
    }

    if (((fromDateFilter.value != "") && (toDateFilter.value != "")) && (new Date(fromDateFilter.value) < new Date(toDateFilter.value))) {
        filteredData = filteredData.filter((reportItem) => {
            return ((new Date(reportItem.date) >= new Date(fromDateFilter.value)) && (new Date(reportItem.date) <= new Date(toDateFilter.value)));
        });
    }

    if ((fromDateFilter.value != "") && (toDateFilter.value == "")) {
        filteredData = filteredData.filter((reportItem) => {
            return (new Date(reportItem.date) >= new Date(fromDateFilter.value));
        });
    }

    if ((fromDateFilter.value == "") && (toDateFilter.value != "")) {
        filteredData = filteredData.filter((reportItem) => {
            return (new Date(reportItem.date) <= new Date(toDateFilter.value));
        });
    }

    if (((fromAmountFilter.value != "") && (toAmountFilter.value != "")) && (fromAmountFilter.value < toAmountFilter.value)) {
        filteredData = filteredData.filter((reportItem) => {
            return ((reportItem.amount >= fromAmountFilter.value) && (reportItem.amount <= toAmountFilter.value));
        });
    }
    else if ((fromAmountFilter.value != "") && (toAmountFilter.value == "")) {
        filteredData = filteredData.filter((reportItem) => {
            return (reportItem.amount >= fromAmountFilter.value);
        });
    }
    else if ((fromAmountFilter.value == "") && (toAmountFilter.value != "")) {
        filteredData = filteredData.filter((reportItem) => {
            return (reportItem.amount <= toAmountFilter.value);
        });
    }

    if (productFilter.value != "") {
        filteredData = filteredData.filter((reportItem) => {
            return reportItem.productName.includes(productFilter.value.toLowerCase());
        });
    }

    if (salespersonFilter.value != "") {
        filteredData = filteredData.filter((reportItem) => {
            return reportItem.salespersonName.includes(salespersonFilter.value);
        });
    }

    filteredData.forEach((item) => {
        const listItemTag = document.createElement("li");
        listItemTag.setAttribute("class", "report-item");

        const listItemID = document.createElement("div");
        listItemID.setAttribute("class", "report-item-id");
        listItemID.innerText = item.id;
        listItemTag.insertAdjacentElement("beforeend", listItemID);

        const listItemAddress = document.createElement("div");
        listItemAddress.setAttribute("class", "report-item-address");
        listItemAddress.innerText = item.address;
        listItemTag.insertAdjacentElement("beforeend", listItemAddress);

        const listItemDate = document.createElement("div");
        listItemDate.setAttribute("class", "report-item-date");
        listItemDate.innerText = getFormattedDate(new Date(item.date));
        listItemTag.insertAdjacentElement("beforeend", listItemDate);

        const listItemAmount = document.createElement("div");
        listItemAmount.setAttribute("class", "report-item-amount");
        listItemAmount.innerText = `$${item.amount.toFixed(2)}`;
        listItemTag.insertAdjacentElement("beforeend", listItemAmount);

        const listItemProduct = document.createElement("div");
        listItemProduct.setAttribute("class", "report-item-product");
        listItemProduct.innerText = item.productName;
        listItemTag.insertAdjacentElement("beforeend", listItemProduct);

        const listItemSalesperson = document.createElement("div");
        listItemSalesperson.setAttribute("class", "report-item-salesperson");
        listItemSalesperson.innerText = item.salespersonName;
        listItemTag.insertAdjacentElement("beforeend", listItemSalesperson);

        reportList.insertAdjacentElement("beforeend", listItemTag);
    });
}

function clearFilters(event) {
    event.preventDefault();
    event.stopPropagation();

    addressFilter.value = "";
    fromDateFilter.value = "";
    toDateFilter.value = "";
    fromAmountFilter.value = "";
    toAmountFilter.value = "";
    productFilter.value = "";
    salespersonFilter.value = "";

    applyReportFilter(event);
}

filterButton.addEventListener('click', applyReportFilter);
filterClear.addEventListener('click', clearFilters);