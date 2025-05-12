let searchButton = document.getElementById("searchButton");

document.querySelectorAll(".old-food").forEach(async function (f) {
    console.log(f.getAttribute("data-api-id"));
    let response =  await fetch("/api/food/" + f.getAttribute("data-api-id"));
    if(response.ok){
        document.getElementById("empty-message").hidden = true
        let result = await response.json();
        console.log(result);
        document.getElementById("selectedFood").appendChild(generateAccordionItem(result, true));
        f.remove();
    } else {
        console.log("Error: " + response.status);
    }
});

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

    let heading = document.createElement("h3");
    heading.textContent = "Results";
    resultList.appendChild(heading);

    if (result.length === 0) {
        let noResults = document.createElement("p");
        noResults.className = "text-danger";
        noResults.textContent = "No results found";
        resultList.appendChild(noResults);
        return;
    }

    let accordion = document.createElement("div");
    accordion.id = "searchResultsAccordion";
    accordion.className = "accordion";
    result.forEach(food => {
        if(!findValueInSelected(selectedFood, food.foodId)){
            // console.log("foodId: " + food.foodId + " not found in selectedFood");
            let row = generateAccordionItem(food, false);
            accordion.appendChild(row);
        }
    });
    resultList.appendChild(accordion);
}

function generateAccordionItem(food, isSelected){
    let accordionItem = document.createElement("div");
    accordionItem.className = "accordion-item";
    let accordionHeader = document.createElement("h3");
    accordionHeader.className = "accordion-header";
    accordionHeader.setAttribute("id", "heading" + food.foodId);

    let accordionButton = document.createElement("button");
    accordionButton.className = "accordion-button collapsed";
    accordionButton.setAttribute("type", "button");
    accordionButton.setAttribute("data-bs-toggle", "collapse");
    accordionButton.setAttribute("data-bs-target", "#collapse" + food.foodId);
    accordionButton.setAttribute("aria-expanded", "false");
    accordionButton.setAttribute("aria-controls", "collapse" + food.foodId);
    accordionButton.textContent = food.foodName;

    accordionHeader.appendChild(accordionButton);

    let data = document.createElement("input");
    data.setAttribute("value", food.foodId);
    data.setAttribute("name", "Foods");
    data.setAttribute("type", "number");
    data.setAttribute("hidden", "true");
    data.setAttribute("readonly", "true");

    accordionHeader.appendChild(data);

    let accordionBody = document.createElement("div");
    accordionBody.className = "accordion-collapse collapse";
    accordionBody.setAttribute("id", "collapse" + food.foodId);
    accordionBody.setAttribute("aria-labelledby", "heading" + food.foodId);
    // accordionBody.setAttribute("data-bs-parent", "#searchResultsAccordion");

    let bodyContent = document.createElement("div");
    bodyContent.className = "accordion-body";

    
    let addBtn = generateAddButton(isSelected);
    bodyContent.appendChild(addBtn);

    let bodyText = document.createElement("span");
    bodyText.textContent = " " + food.foodDescription;

    bodyContent.appendChild(bodyText);

    accordionBody.appendChild(bodyContent);
    
    accordionItem.appendChild(accordionHeader);
    accordionItem.appendChild(accordionBody);

    return accordionItem;
}

function generateAddButton(isSelected){
    let addBtn = document.createElement("button");
    addBtn.className = "btn btn-secondary";
    if(isSelected){
        addBtn.textContent = "Remove";
    }else{
        addBtn.textContent = "Add";
    }
    addBtn.type = "button";
    addBtn.addEventListener("click", function(){
        try{
            if(this.textContent === "Add"){
                this.textContent = "Remove";
                let movedItem = this.parentElement.parentElement.parentElement;
                this.parentElement.parentElement.parentElement.remove();
                document.getElementById("selectedFood").appendChild(movedItem);
                document.getElementById("empty-message").hidden = true;
            }else{
                this.textContent = "Add";
                let movedItem = this.parentElement.parentElement.parentElement;
                this.parentElement.parentElement.parentElement.remove();
                document.getElementById("searchResultsAccordion").prepend(movedItem);
                if(document.getElementById("selectedFood").childElementCount === 0){
                    document.getElementById("empty-message").hidden = false;
                }
            }
        } catch (error) {
            //console.error(error);
            this.remove();
        }
    });

    return addBtn;
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

