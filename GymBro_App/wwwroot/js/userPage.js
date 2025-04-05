document.addEventListener("DOMContentLoaded", () => {
    if (document.getElementById("profile-picture")) {
        document.getElementById("profile-picture").addEventListener("change", updateProfilePicture)
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
    }
    console.log(document.getElementById("profile-picture-preview").src)
    reader.readAsDataURL(files[0])
    // let formData = new FormData()
    // formData.append("file", file)
}

