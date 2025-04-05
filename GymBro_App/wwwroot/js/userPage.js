document.addEventListener("DOMContentLoaded", () => {
    if (document.getElementById("profile-picture")) {
        document.getElementById("profile-picture").addEventListener("change", previewProfilePicture)
    }
})

async function updateProfilePicture() {
    // Adapted from Samculo on StackOverflow
    // https://stackoverflow.com/a/67243169
    let files = document.getElementById("profile-picture").files
    let file = document.getElementById("profile-picture").files[0]
    console.log(file)

    const reader = new FileReader()

    reader.onload = async (event) => {
        document.getElementById("profile-picture-preview").src = event.target.result
        document.getElementById("profile-pic").src = event.target.result
    }
    reader.readAsDataURL(files[0])
    
    let formData = new FormData()
    formData.append("profilePicture", file)

    
    let response = await fetch("/api/users/updateProfilePicture", {
        method: "POST",
        body: formData,
    })

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
        preview.appendChild(document.createElement("img"))
        preview.lastChild.setAttribute("id", "profile-picture-preview")
        
        picturePreview = document.getElementById("profile-picture-preview")
        picturePreview.setAttribute("class", "img-fluid mb-3")
        picturePreview.setAttribute("alt", "Profile picture preview")
        picturePreview.setAttribute("style", "width: 100px; height: 100px;")
        picturePreview.setAttribute("src", event.target.result)

        preview.appendChild(document.createElement("button"))
        preview.lastChild.setAttribute("id", "upload-profile-picture")
        let updateProfileButton = document.getElementById("upload-profile-picture")
        updateProfileButton.setAttribute("class", "btn btn-primary")
        
        updateProfileButton.innerText = "Update Profile Picture"
        updateProfileButton.addEventListener("click", () => {
            updateProfilePicture()
        })
    }
    reader.readAsDataURL(file)
}