let userSignedIn = false;

document.addEventListener("DOMContentLoaded", () => {
    if (isLoggedIn) {
        userSignedIn = true
    }

    const mapElement = document.getElementById("nearby-gyms-map")
    if (mapElement) {
        embedDefaultMap()
        navigator.geolocation.getCurrentPosition(embedMapAtUserPosition, getPositionError)
    }

    const setLocationButton = document.getElementById("set-location-button");
    if (setLocationButton) {
        setLocationButton.addEventListener("click", () => {
            // TODO: Find a way to update the map without having to call getCurrentPosition twice
            navigator.geolocation.getCurrentPosition(embedMapAtUserPosition, getPositionError);
            navigator.geolocation.getCurrentPosition(getNearbyGyms, noNearbyGyms);
        });
    }

    const getUserLocation = document.getElementById("getUserLocation-btn")
    if (getUserLocation) {
        console.log("getUserLocation-btn found")
        getUserLocation.addEventListener("click", () => {
            navigator.geolocation.getCurrentPosition((position) => {
                reverseGeocode(position.coords.latitude, position.coords.longitude)
            }, getPositionError)
        })
    }
    else {
        console.log("getUserLocation-btn not found")
    }

    const searchGymPostalCode = document.getElementById("nearby-gym-search-form")
    if (searchGymPostalCode) {
        searchGymPostalCode.addEventListener("submit", async (event) => {
            event.preventDefault()
            let postalCode = document.getElementById("postal-code-gym-search").value
            console.log(postalCode)
            let coordinates = await geocodePostal(postalCode)
            console.log(coordinates)

            if (coordinates) {
                let lat = coordinates.latitude
                let long = coordinates.longitude
                embedMapAtUserPosition({ coords: { latitude: lat, longitude: long } })
                getNearbyGyms({ coords: { latitude: lat, longitude: long } })
            }
            else {
                console.error("Error: couldn't get coordinates for postal code")
            }
        })
    }
})

function noNearbyGyms() {
    let gymList = document.getElementById("nearby-gym-search-list")
    document.getElementById("nearby-gyms-results-header").innerText = "No nearby gyms found."
    gymList.innerHTML = ""
}

function getPositionError(err) {
    console.log(`Error ${err.code}: couldn't get location. Issue: ${err.message}`)
    return null
}

async function embedDefaultMap() {
    const mapFrame = document.getElementById("nearby-gyms-map")
    let response = await fetch(`/api/maps`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if (response.ok) {
        let result = await response.json()
        mapFrame.src = `https://www.google.com/maps/embed/v1/view?key=${result.apiKey}&center=39.828175,-98.5795&zoom=4`
    }
}

async function embedMapAtUserPosition(position) {
    // console.log("Got to embedMap")
    const mapFrame = document.getElementById("nearby-gyms-map")
    let coordinates = position.coords

    let response = await fetch(`/api/maps`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if (response.ok) {
        let result = await response.json()
        // console.log("Got response")

        let lat = coordinates.latitude.toFixed(6)
        let long = coordinates.longitude.toFixed(6)

        mapFrame.src = `https://www.google.com/maps/embed/v1/view?key=${result.apiKey}&center=${lat},${long}&zoom=10`
    }
}

async function getNearbyGyms(pos) {
    // TODO: Call this method in a better way (ex: "api/maps/nearby?latitude=lat&longitude=long").
    // This would also involve changing how this function is called in the event listener.
    let lat = pos.coords.latitude
    let long = pos.coords.longitude
    let response = await fetch(`/api/maps/nearby/${lat}/${long}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if(response.ok){
        let result = await response.json()
        let gymList = document.getElementById("nearby-gym-search-list")
        gymList.textContent = ""

        if (result.length === 0) {
            document.getElementById("nearby-gyms-results-header").innerText = "No nearby gyms found."
            gymList.parentNode.replaceChild(gymList.cloneNode(false), gymList)
            return
        }
        else {
            document.getElementById("nearby-gyms-results-header").textContent = `Found ${result.length} nearby gyms`
        }
        for (let i = 0; i < result.length; i++) {
            let gym = result[i]
            // console.log(gym.regularOpeningHours)

            // // Gets open/close status and the next time it opens/closes. TODO: Fix this so it adjusts for UTC time and 24/7 gyms
            // let gymOpenStatus = gym.regularOpeningHours.openNow ? "Open" : "Closed"
            // const daysOfWeek = []
            // const currentDay = new Date().getDay()
            // console
            // console.log(`Open Status: ${gymOpenStatus}`)
            // console.log(`Today's hours: ${gym.regularOpening}`)
            // if (gymOpenStatus === "Open") {
            //     console.log("Today's hours: " + gym.regularOpeningHours.openingHours[currentDay])
            //     console.log("Closes at: " + gym.regularOpeningHours.nextCloseTime)
            // }
            // else {
            //     console.log("Opens at: " + gym.regularOpeningHours.nextOpenTime)
            // }
            // console.log(`${gym.regularOpeningHours.nextCloseTime} ${gym.regularOpeningHours.nextOpenTime}`)
            // let gymNextStatusHours = gymOpenStatus ? `Closes at ${gym.regularOpeningHours.nextCloseTime} ` : `Opens at ${gym.regularOpeningHours.nextOpenTime}`
            // gymNextStatusHours = gymNextStatusHours.split("T")[1].slice(0, -1)
            // console.log("Closes at: " + gym.regularOpeningHours.nextCloseTime)
            // console.log("Opens at: " + gym.regularOpeningHours.nextOpenTime)
            

            let gymResult = document.createElement("div")
            gymResult.setAttribute("class", "card mb-3 text-decoration-none text-dark diplayed-gym-card")
            
            let gymResultBody = document.createElement("div")
            gymResultBody.setAttribute("class", "card-body")
            gymResultBody.appendChild(document.createElement("h5")).innerText = gym.displayName.text

            let gymResultAddress = document.createElement("p")
            gymResultAddress.setAttribute("class", "card-text")
            gymResultAddress.innerText = gym.formattedAddress
            gymResultBody.appendChild(gymResultAddress)

            let gymHours = gym.regularOpeningHours.weekdayDescriptions
            let gymHoursParagraphs = document.createElement("div")
            gymHoursParagraphs.setAttribute("class", "d-flex flex-column mb-3")
            gymHours.forEach(day => {
                let gymHoursParagraph = document.createElement("p")
                gymHoursParagraph.setAttribute("class", "card-text my-1")
                gymHoursParagraph.innerText = day
                gymHoursParagraphs.appendChild(gymHoursParagraph)
            })
            gymResultBody.appendChild(gymHoursParagraphs)

            console.log("Getting gym rating: " + gym.rating)
            let gymRating = document.createElement("p")
            gymRating.setAttribute("class", "card-text")
            gymRating.innerText = `Rating: ${gym.rating} / 5.0`
            gymResultBody.appendChild(gymRating)

            let gymResultActions = document.createElement("div")
            gymResultActions.setAttribute("class", "d-flex justify-content-between align-items-center")
            
            // Preparing the gym's id for the bookmark button (if they're logged in)
            if (userSignedIn) {
                let gymPlaceID = gym.name.replace("places/", "")
                let isBookmarked = await isGymBookmarked(gymPlaceID)

                let bookmarkButton = document.createElement("button")
                bookmarkButton.setAttribute("class", "btn bookmark-gym-button")
                bookmarkButton.setAttribute("id", gymPlaceID)
                bookmarkButton.setAttribute("type", "button")
                
                if (isBookmarked) {
                    bookmarkButton.setAttribute("disabled", "true")
                    let bookmarkStar = document.createElement("i")
                    bookmarkStar.setAttribute("class", "fa-solid fa-star bookmarked-star")
                    bookmarkButton.appendChild(bookmarkStar)
                    bookmarkButton.appendChild(document.createTextNode(" Bookmarked"))
                    bookmarkButton.setAttribute("class", "btn bookmark-gym-button disabled")
                    bookmarkButton.setAttribute("aria-disabled", "true")
                    bookmarkButton.setAttribute("disabled", "true")
                    gymResultActions.appendChild(bookmarkButton)
                }

                else {
                    let bookmarkStar = document.createElement("i")
                    bookmarkStar.setAttribute("class", "fa-solid fa-star")
                    bookmarkButton.appendChild(bookmarkStar)

                    bookmarkButton.appendChild(document.createTextNode(" Add to Boomarks"))
                    bookmarkButton.addEventListener("click", () => {
                        console.log("Bookmark button clicked")
                        bookmarkGym(gymPlaceID)
                        bookmarkButton.setAttribute("disabled", "true")
                        bookmarkButton.setAttribute("class", "btn bookmark-gym-button disabled")
                        bookmarkButton.setAttribute("aria-disabled", "true")
                        bookmarkButton.textContent = ""

                        let bookmarkedStar = document.createElement("i")
                        bookmarkedStar.setAttribute("class", "fa-solid fa-star fa-bounce freshly-bookmarked-star")
                        bookmarkButton.appendChild(bookmarkedStar)
                        bookmarkButton.appendChild(document.createTextNode(" Bookmarked"))
                    })
                    gymResultActions.appendChild(bookmarkButton)
                }
            }

            let gymWebsiteButton = document.createElement("a")
            gymWebsiteButton.setAttribute("target", "_blank")
            gymWebsiteButton.setAttribute("rel", "noopener noreferrer")
            gymWebsiteButton.setAttribute("href", gym.websiteUri)
            gymWebsiteButton.setAttribute("class", "text-decoration-none text-dark")

            let gymWebsiteIcon = document.createElement("i")
            gymWebsiteIcon.setAttribute("class", "fa-solid fa-arrow-up-right-from-square")
            gymWebsiteButton.appendChild(gymWebsiteIcon)
            gymWebsiteButton.appendChild(document.createTextNode(" View Gym Website"))
            gymResultActions.appendChild(gymWebsiteButton)

            gymResultBody.appendChild(gymResultActions)
            gymResult.appendChild(gymResultBody)
            gymList.appendChild(gymResult)
        }
    }
    else {
        console.log("Error: " + response.status)
    }
}

async function bookmarkGym(gymPlaceID) {
    let response = await fetch(`/api/users/bookmarkGym/${gymPlaceID}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
    })

    if (response.ok) {
        console.log("Gym bookmarked successfully")
    }
    else {
        console.log("Error: " + response.status)
    }
}

async function isGymBookmarked(gymPlaceID) {
    let response = await fetch(`/api/users/isGymBookmarked/${gymPlaceID}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'}
        })
        if (response.ok) {
            let result = await response.json()
            console.log(result)
            return result
        }
        else {
            console.log("Error in isGymBookmarked: " + response.status)
        }
}


async function reverseGeocode(lat, long) {
    let response = await fetch(`/api/maps/reversegeocode/${lat}/${long}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if(response.ok){
        let result = await response.json()
        console.log(result)
        let userLocation = document.getElementById("user-location")
        userLocation.innerHTML = `Location: ${result.address}`
        
    }
    else {
        console.log("Error: " + response.status)
    }
}

async function geocodePostal(postalCode) {
    let response = await fetch(`/api/maps/geocode/${postalCode}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if(response.ok){
        let result = await response.json()
        console.log(result)
        return result
    }
    else {
        console.log("Error: " + response.status)
    }
}

if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        getPositionError,
        embedDefaultMap,
        embedMapAtUserPosition,
        getNearbyGyms
    }
}