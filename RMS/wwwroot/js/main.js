function toggleRequests(arg) {
    var ulItem = document.getElementById(arg);
    var liItems = ulItem.getElementsByTagName('li');

    if (ulItem.classList.contains("drop-list")) {
        ulItem.classList.remove("drop-list");
        ulItem.classList.add("list");
    } else {
        ulItem.classList.remove("list");
        ulItem.classList.add("drop-list");
    }

    for (var i = 0; i < liItems.length; i++) {
        liItems[i].className = liItems[i].className === 'drop-item' ? 'item' : 'drop-item';
    }
}