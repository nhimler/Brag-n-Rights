let searchButton = document.getElementById("searchButton");

searchButton.addEventListener("click", async function () {
    let query = document.getElementById("searchBar").value;
    console.log("searching for: " + query);

    let response =  await fetch("/api/food/search?q=" + query);
    let result;
    if(response.ok){
        result = await response.json();
        console.log(result);
        await displaySearchResults(result);
    } else {
        console.log("Error: " + response.status);
    }
});


async function displaySearchResults(result){
    console.log("displaySearchResults called with:", result); // Debugging line

    let resultList = document.getElementById("searchResults");
    if (!resultList) {
        console.error("resultsList element not found");
        return;
    }
    selectedFood = document.getElementById("selectedFood");
    resultList.innerHTML = ""; 

    let heading = document.createElement("h2");
    heading.textContent = "Results";
    resultList.appendChild(heading);

    let ul = document.createElement("ul");
    result.forEach(food => {
        if(!findValueInChildren(selectedFood, food.foodId)){
            // console.log("foodId: " + food.foodId + " not found in selectedFood");
            let li = document.createElement("li");
            li.value = food.foodId;
            li.textContent = food.foodName;
            let expandBtn = document.createElement("button");
            expandBtn.className = "btn";
            expandBtn.textContent = "v";
            expandBtn.addEventListener("click", function(){
                if(this.textContent === "v"){
                    this.textContent = "^";
                    let info = document.createElement("p");
                    info.textContent = food.foodDescription;
                    li.appendChild(info);
                }else{
                    this.textContent = "v";
                    li.removeChild(li.lastChild);
                }
            });

            let addBtn = document.createElement("button");
            addBtn.className = "btn";
            addBtn.textContent = "Add";
            addBtn.addEventListener("click", function(){
                if(this.textContent === "Add"){
                    movedLi = this.parentElement;
                    this.parentElement.remove();
                    selectedFood.appendChild(movedLi);
                }else{

                }
            });
            li.appendChild(expandBtn);
            li.appendChild(addBtn);
            ul.appendChild(li);
        }
    });
    resultList.appendChild(ul);
}

function findValueInChildren(parent, value){
    for(let i = 0; i < parent.children.length; i++){
        if(parent.children[i].getAttribute("value") === value){
            return true;
        }
    }
    return false;
}
