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
    if (window.innerWidth <= 1024) {
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
    if (window.innerWidth <= 1024) {
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
                        var addres = abon.addressFlat == "" ? "вул. " + abon.addressStreet + " буд. " + abon.addressBuild :
                            "вул. " + abon.addressStreet + " буд. " + abon.addressBuild + " кв. " + abon.addressFlat
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
    var uidValue = document.getElementById('uid').value;
    $.ajax({
        url: "/api/Abonents/searchuidpi?uid=" + uidValue,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var addressElement = $('#address');
            var fioElement = $('#fio');
            var phoneElement = $('#phone');
            var commentElement = $('#comment');

            // Проверяем, что данные получены и не пустые
            if (data && Object.keys(data).length > 0) {
                var addres = data.addressFlat == "" ? "вул. " + data.addressStreet + " буд. " + data.addressBuild :
                    "вул. " + data.addressStreet + " буд. " + data.addressBuild + " кв. " + data.addressFlat;
                if (addressElement.val == "") addressElement.val(addres);
                fioElement.val(data.fio);
                phoneElement.val(data.phone);
                commentElement.val(data.comments);
            } else {
                console.log("Данные не получены или пустые.");
            }
        },
        error: function (error) {
            console.log("Произошла ошибка при выполнении запроса:", error);
        }
    });
    $.ajax({
        url: "/api/Abonents/searchuid?uid=" + uidValue,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var loginElement = $('#login');
            var passwordElement = $('#password');

            // Проверяем, что данные получены и не пустые
            if (data && Object.keys(data).length > 0) {
                loginElement.val(data.login);
                passwordElement.val(data.password);
            } else {
                console.log("Данные не получены или пустые.");
            }
        },
        error: function (error) {
            console.log("Произошла ошибка при выполнении запроса:", error);
        }
    });
    $.ajax({
        url: "/api/Abonents/searchuidinternet?uid=" + uidValue,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var tpNameElement = $('#tpName');

            // Проверяем, что данные получены и не пустые
            if (data && Object.keys(data).length > 0) {
                tpNameElement.val(data.tpName);
            } else {
                console.log("Данные не получены или пустые.");
            }
        },
        error: function (error) {
            console.log("Произошла ошибка при выполнении запроса:", error);
        }
    });
});

sideBar.addEventListener('mouseleave', handleOver);
sideBar.addEventListener('mouseenter', handleHover);

checkPreference('themePreference', 'night', togglePageMode);
checkPreference('modePreference', 'tablecheck', toggleTable);

let mounterCount = 1;

document.getElementById('add-mounter').addEventListener('click', function () {
    const totalCountInput = document.getElementById('totalCount');
    const totalCount = parseInt(totalCountInput.value);
    const newCount = totalCount + 1;

    const container = document.getElementById(`mounter-container-0`);
    const clone = container.cloneNode(true);

    // Изменяем атрибуты и идентификаторы клонированного контейнера
    const select = clone.querySelector('select');
    select.name = `Ids`;
    select.selectedIndex = 0;

    document.getElementById('mounters-form').appendChild(clone);

    totalCountInput.value = newCount;
    mounterCount = newCount;
});