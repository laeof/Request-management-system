var sideBar = document.getElementById("leftmenu");
var container = document.getElementById("container");
var table = document.getElementById("table");
var body = document.getElementById("body");

var minimized = false;
function toggleRequests(arg) {
    var ulItem = document.getElementById(arg);
    var liItems = ulItem.getElementsByTagName('li');
    var imgItem = document.getElementById(arg + 'img');

    if (ulItem.classList.contains("drop-list")) {
        ulItem.classList.remove("drop-list");
        ulItem.classList.add("list");
    } else {
        ulItem.classList.remove("list");
        ulItem.classList.add("drop-list");
    }

    if (imgItem.classList.contains("bottom")) {
        imgItem.classList.remove("bottom");
    }
    else {
        imgItem.classList.add("bottom");
    }

    for (var i = 0; i < liItems.length; i++) {
        if (liItems[i].classList.contains("drop-item")) {
            liItems[i].classList.remove("drop-item");
            liItems[i].classList.add("item");
        } else {
            liItems[i].classList.remove("item");
            liItems[i].classList.add("drop-item");
        }
    }
}
function togglePageMode(isDark) {
    if (isDark) {
        body.classList.add("night");
    }
    else {
        body.classList.remove("night");
    }
}
function toggleTable(isCard) {
    if (!isCard) {
        table.classList.remove("card");
    }
    else {
        table.classList.add("card");
    }
}
function toggleLeftSideBar() {
    if (sideBar.classList.contains("minimized")) {
        sideBar.classList.remove("minimized");
        container.classList.remove("minimized");
        minimized = false;
    }
    else {
        sideBar.classList.add("minimized");
        container.classList.add("minimized");
        minimized = true;
    }
}
function handleHover() {
    if (window.innerWidth < 1024) {
        if (minimized) {
            sideBar.classList.remove("minimized");
            sideBar.classList.add("min");
        }
        else {
            sideBar.classList.remove("min");
            sideBar.classList.add("minimized");
        }
    }
    else {
        if (minimized) {
            sideBar.classList.remove("minimized");
        }
    }
}
function handleOver() {
    if (window.innerWidth < 1024) {
        if (minimized) {
            sideBar.classList.remove("min");
            sideBar.classList.add("minimized");
        }
        else {
            sideBar.classList.remove("minimized");
            sideBar.classList.add("min");
        }
    }
    else {
        if (minimized) {
            sideBar.classList.add("minimized");
        }
    }
}
function savePreference(parameterName, checkboxId, applyFunction) {
    var checkbox = document.getElementById(checkboxId);
    var parameterValue = checkbox.checked;

    document.cookie = parameterName + '=' + parameterValue + '; path=/';

    applyFunction(parameterValue);
}
function checkPreference(parameterName, checkboxId, applyFunction) {
    var parameterValue = document.cookie.replace(new RegExp('(?:(?:^|.*;\\s*)' + parameterName + '\\s*\\=\\s*([^;]*).*$)|^.*$'), '$1');
    var checkbox = document.getElementById(checkboxId);

    if (parameterValue === 'true') {
        checkbox.checked = true;
    } else {
        checkbox.checked = false;
    }

    applyFunction(checkbox.checked);
}

$(document).ready(function () {
    $('#searchabon').select2({
        multiple: true,
        ajax: {
            url: '/api/Abonents/search',
            type: 'GET',
            dataType: 'json',
            data: function (params) {
                return {
                    searchText: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data.map(function (abon) {
                        var addres = abon.addressFlat == "" ? abon.addressStreet + " буд. " + abon.addressBuild :
                            abon.addressStreet + " буд. " + abon.addressBuild + " кв. " + abon.addressFlat
                        return {
                            id: abon.uid,
                            text: abon.fio,
                            address: addres
                        };
                    })
                };
            }
        }
    }).on('select2:select', function (e) {

        var selectedAbon = e.params.data;

        $('#searchabon').append(new Option(selectedAbon.text, selectedAbon.id, true, true)).trigger('change');
        $('#address').val(selectedAbon.address);

    });
});

sideBar.addEventListener('mouseleave', handleOver);
sideBar.addEventListener('mouseenter', handleHover);

checkPreference('themePreference', 'night', togglePageMode);
checkPreference('modePreference', 'tablecheck', toggleTable);