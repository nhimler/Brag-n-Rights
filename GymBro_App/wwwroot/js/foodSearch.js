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

    let table = document.createElement("table");
    table.className = "table table-bordered";
    let tbody = document.createElement("tbody");
    result.forEach(food => {
        if(!findValueInSelected(selectedFood, food.foodId)){
            // console.log("foodId: " + food.foodId + " not found in selectedFood");
            let tr = document.createElement("tr");
            let td = document.createElement("td");
            let data = document.createElement("input");
            td.colspan = 2;
            data.setAttribute("value", food.foodId);
            data.setAttribute("name", "Foods");
            data.setAttribute("type", "number");
            data.setAttribute("hidden", "true");
            data.setAttribute("readonly", "true");
            td.textContent = food.foodName;

            let expandBtn = document.createElement("button");
            expandBtn.className = "btn";
            expandBtn.textContent = "v";
            expandBtn.type = "button";
            expandBtn.addEventListener("click", function(){
                if(this.textContent === "v"){
                    this.textContent = "^";
                    let info = document.createElement("p");
                    info.textContent = food.foodDescription;
                    td.appendChild(info);
                }else{
                    this.textContent = "v";
                    td.removeChild(td.lastChild);
                }
            });

            td.appendChild(expandBtn);
            td.appendChild(data);

            let td2 = document.createElement("td");
            let addBtn = document.createElement("button");
            addBtn.className = "btn";
            addBtn.textContent = "Add";
            addBtn.type = "button";
            addBtn.addEventListener("click", function(){
                if(this.textContent === "Add"){
                    this.textContent = "Remove";
                    let movedTr = this.parentElement.parentElement;
                    this.parentElement.parentElement.remove();
                    selectedFood.appendChild(movedTr);
                }else{
                    this.textContent = "Add";
                    let movedTr = this.parentElement.parentElement;
                    this.parentElement.parentElement.remove();
                    tbody.prepend(movedTr);

                }
            });
            td2.appendChild(addBtn);
            tr.appendChild(td);
            tr.appendChild(td2);
            tbody.appendChild(tr);
            table.appendChild(tbody);
        }
    });
    resultList.appendChild(table);
}

function findValueInSelected(parent, value){
    if(!parent){
        console.log("parent is null");
        return false;
    }
    for (let input of parent.querySelectorAll("input")) {
        if (input.value === value) {
            return true;
        }
    }
    return false;
}

