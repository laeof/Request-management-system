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