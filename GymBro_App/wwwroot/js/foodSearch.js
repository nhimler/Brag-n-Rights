let searchButton = document.getElementById("searchButton");

searchButton.addEventListener("click", async function () {
    let query = document.getElementById("searchBar").value;
    console.log("searching for: " + query);

    let response =  await fetch("/api/food/search?q=" + query);
    let result;
    if(response.ok){
        result = await response.json();
        console.log(result);
        displaySearchResults(result);
    } else {
        console.log("Error: " + response.status);
    }
});


function displaySearchResults(result){
    console.log("displaySearchResults called with:", result); // Debugging line

    let resultList = document.getElementById("searchResults");
    if (!resultList) {
        console.error("resultsList element not found");
        return;
    }
    resultList.innerHTML = "<h3>Results</h3><ul>";
    result.forEach(food => {
        resultList.innerHTML += "<li>" + food.foodName + "</li>";
    });
    resultList.innerHTML += "</ul>";
}

