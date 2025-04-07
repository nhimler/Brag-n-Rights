document.addEventListener("DOMContentLoaded", () => {
    if (document.getElementById("profile-picture")) {
        document.getElementById("profile-picture").addEventListener("change", previewProfilePicture)
    }
})

async function updateProfilePicture() {
    // Adapted from Samculo on StackOverflow
    // https://stackoverflow.com/a/67243169
    let file = document.getElementById("profile-picture").files[0]
    console.log(file)

    let formData = new FormData()
    formData.append("profilePicture", file)

    showProfilePicture(file)
    
    let response = await fetch("/api/users/updateProfilePicture", {
        method: "POST",
        body: formData,
    })

    if (document.getElementById("preview-container")) {
        document.getElementById("preview-container").innerHTML = ""
    }

    if (document.getElementById("profile-picture")) {
        document.getElementById("profile-picture").value = ""
    }

    if (response.ok) {
        console.log("Profile picture updated successfully")
    }
    else {
        console.error("Error updating profile picture: ", response.statusText)
    }
}

function previewProfilePicture() {
    let file = document.getElementById("profile-picture").files[0]
    console.log(file)

    const reader = new FileReader()

    reader.onload = (event) => {
        let preview = document.getElementById("preview-container")
        preview.innerHTML = ""

        let picturePreview = document.createElement("img");
        picturePreview.setAttribute("id", "profile-picture-preview");
        picturePreview.setAttribute("class", "img-fluid mb-2 user-profile");
        picturePreview.setAttribute("alt", "Profile picture preview");
        picturePreview.setAttribute("src", event.target.result);
        preview.appendChild(picturePreview);

        let updateProfileButton = document.createElement("button");
        updateProfileButton.setAttribute("id", "upload-profile-picture");
        updateProfileButton.setAttribute("class", "btn btn-primary my-3");
        updateProfileButton.innerText = "Update Profile Picture";
        updateProfileButton.addEventListener("click", () => {
            updateProfilePicture();
        });
        preview.appendChild(updateProfileButton);
    }
    reader.readAsDataURL(file)
}

function showProfilePicture(file) {
    const reader = new FileReader()
    reader.onload = (event) => {
        let profilePicture = document.getElementById("profile-pic")
        profilePicture.setAttribute("src", event.target.result)
    }
    reader.readAsDataURL(file)
}