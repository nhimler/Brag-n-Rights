let aiResponseArea = document.getElementById("aiResponseArea");

let suggestBtn = document.getElementById("suggestBtn");

suggestBtn.addEventListener("click", async function() {
    let query = "";
    let selectedFoods = document.getElementById("selectedFood");
    if (selectedFoods.children.length === 0) {
        aiResponseArea.innerHTML = "<p>Please select some foods to get suggestions.</p>";
        return;
    }

    // Search children
    selectedFoods.querySelectorAll("button").forEach(function(button) {
        query += button.textContent + ", \n";
    });

    // let foodList = Array.from(selectedFoods).map(input => input.value).join(",");
    
    
    try{
    let response = await fetch("/api/ai/suggest?q=" + query, {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    });
    if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
    }
    content = await response.text();
    console.log(content);
    content = "1" + content.split("1")[1];


    let lines = content.split("\n");
    let suggestElements = [];
    lines.forEach(async function(line) {
        sgEl = document.createElement("button");
        sgEl.textContent = line;
        sgEl.classList.add("btn", "suggestion-fill-btn"); // Add Bootstrap classes for styling
        sgEl.addEventListener("click", async function() {
            let response = await fetch("/api/ai/fill?q=" + line, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                }
            });
            if (!response.ok) {
                throw new Error("Network response was not ok " + response.statusText);
            }
            let fillContent = await response.text();
            console.log("response: " + fillContent);
            fillContent = "{" + fillContent.split("{")[1]; // Extract the JSON part of the response
            fillContent = fillContent.split("}")[0] + "}"; // Extract the JSON part of the response
            fillContent = JSON.parse(fillContent); // Parse the JSON response
            document.getElementById("Description").value = fillContent.description;
            fillContent.type = fillContent.type.toLowerCase();
            fillContent.type = fillContent.type.charAt(0).toUpperCase() + fillContent.type.slice(1);
            console.log("Type (cleaned): " + fillContent.type);
            document.getElementById("MealType").value = fillContent.type;
            document.getElementById("MealName").value = line;
        });
        suggestElements.push(sgEl);
    });

    // Clear the previous suggestions
    aiResponseArea.innerHTML = ""; // Clear the response area
    // Append the new suggestions to the response area
    suggestElements.forEach(function(sgEl) {
        aiResponseArea.appendChild(sgEl); // Append each suggestion element to the response area
    });

    // content = content.replace(/\n/g, "<br>"); // Replace newline characters with <br> tags
    // content = content.replace(/"/g, ""); // Remove double quotes
    // aiResponseArea.innerHTML = content; // Place Suggestions in the response area
    }catch(error) {
        console.error("Error fetching suggestions:", error);
        aiResponseArea.innerHTML = "<p>Error fetching suggestions. Please try again later.</p>";
    };
});