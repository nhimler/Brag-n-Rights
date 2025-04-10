let aiResponseArea = document.getElementById("aiResponseArea");

let suggestBtn = document.getElementById("suggestBtn");

suggestBtn.addEventListener("click", async function() {
    let query = "";
    let selectedFoods = document.getElementById("selectedFood");
    // Search children
    selectedFoods.querySelectorAll("button").forEach(function(button) {
        query += button.textContent + ", \n";
    });

    // let foodList = Array.from(selectedFoods).map(input => input.value).join(",");
    
    // if (foodList.length === 0) {
    //     aiResponseArea.innerHTML = "<p>Please select some foods to get suggestions.</p>";
    //     return;
    // }

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
    console.log("response: " + response);
    content = await response.text();
    content = content.replace(/\n/g, "<br>"); // Replace newline characters with <br> tags
    content = content.replace(/"/g, ""); // Remove double quotes
    aiResponseArea.innerHTML = content; // Place Suggestions in the response area
    }catch(error) {
        console.error("Error fetching suggestions:", error);
        aiResponseArea.innerHTML = "<p>Error fetching suggestions. Please try again later.</p>";
    };
});