var sideBar = document.getElementById("leftmenu");
var container = document.getElementById("container");
var table = document.getElementById("table");

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
var checkedInputs = document.getElementById("tablecheck");

function toggleTable() {
    if (!checkedInputs.checked) {
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
    if (minimized) {
        sideBar.classList.remove("minimized");
    }
}
function handleOver() {
    if (minimized) {
        sideBar.classList.add("minimized");
    }
}

sideBar.addEventListener('mouseleave', handleOver);
sideBar.addEventListener('mouseenter', handleHover);