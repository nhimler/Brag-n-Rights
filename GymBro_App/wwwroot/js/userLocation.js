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
    let lat = pos.coords.latitude;
    let long = pos.coords.longitude;
    let response = await fetch(`/api/maps/nearby/${lat}/${long}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (response.ok) {
        let result = await response.json();
        renderNearbyGyms(result);
    } else {
        console.log("Error: " + response.status);
    }
}

function renderNearbyGyms(gyms) {
    let gymList = document.getElementById("nearby-gym-search-list");
    gymList.textContent = "";

    if (!gyms || gyms.length === 0) {
        document.getElementById("nearby-gyms-results-header").innerText = "No nearby gyms found.";
        gymList.parentNode.replaceChild(gymList.cloneNode(false), gymList);
        return;
    } else {
        document.getElementById("nearby-gyms-results-header").textContent = `Found ${gyms.length} nearby gyms`;
    }

    gyms.forEach(gym => {
        let gymResult = createGymCard(gym);
        gymList.appendChild(gymResult);
    });
}

function createGymCard(gym) {
    let gymResult = document.createElement("div");
    gymResult.setAttribute("class", "card mb-3 text-decoration-none text-dark diplayed-gym-card");

    let gymResultBody = document.createElement("div");
    gymResultBody.setAttribute("class", "card-body");
    gymResultBody.appendChild(document.createElement("h5")).innerText = gym.displayName.text;

    let gymResultAddress = createGymAddressElement(gym);
    gymResultBody.appendChild(gymResultAddress);

    let gymHoursParagraphs = createGymHoursElement(gym);
    gymResultBody.appendChild(gymHoursParagraphs);

    let gymRatingDiv = createGymRatingElement(gym);
    gymResultBody.appendChild(gymRatingDiv);

    let gymResultActions = createGymActions(gym);
    gymResultBody.appendChild(gymResultActions);

    gymResult.appendChild(gymResultBody);
    return gymResult;
}

function createGymActions(gym) {
    let gymResultActions = document.createElement("div");
    gymResultActions.setAttribute("class", "d-flex justify-content-between align-items-center");

    // Bookmark button (if logged in)
    if (userSignedIn) {
        let gymPlaceID = gym.name.replace("places/", "");
        let bookmarkButton = createBookmarkButton(gymPlaceID);
        gymResultActions.appendChild(bookmarkButton);
    }

    let gymWebsiteButton = document.createElement("a");
    gymWebsiteButton.setAttribute("target", "_blank");
    gymWebsiteButton.setAttribute("rel", "noopener noreferrer");
    gymWebsiteButton.setAttribute("href", gym.websiteUri);
    gymWebsiteButton.setAttribute("class", "text-decoration-none text-dark");

    let gymWebsiteIcon = document.createElement("i");
    gymWebsiteIcon.setAttribute("class", "fa-solid fa-arrow-up-right-from-square");
    gymWebsiteButton.appendChild(gymWebsiteIcon);
    gymWebsiteButton.appendChild(document.createTextNode(" View Gym Website"));
    gymResultActions.appendChild(gymWebsiteButton);

    return gymResultActions;
}

function createBookmarkButton(gymPlaceID) {
    let bookmarkButton = document.createElement("button");
    bookmarkButton.setAttribute("class", "btn bookmark-gym-button");
    bookmarkButton.setAttribute("id", gymPlaceID);
    bookmarkButton.setAttribute("type", "button");

    // Set initial state asynchronously
    isGymBookmarked(gymPlaceID).then(isBookmarked => {
        updateBookmarkButton(isBookmarked, bookmarkButton);

        bookmarkButton.addEventListener("click", async () => {
            if (bookmarkButton.disabled) return;
            bookmarkButton.disabled = true;
            let currentlyBookmarked = await isGymBookmarked(gymPlaceID);
            if (currentlyBookmarked) {
                await removeGymBookmark(gymPlaceID, bookmarkButton);
            } else {
                await bookmarkGym(gymPlaceID);
            }
            let updatedBookmarked = await isGymBookmarked(gymPlaceID);
            updateBookmarkButton(updatedBookmarked, bookmarkButton);
            bookmarkButton.disabled = false;
        });
    });

    return bookmarkButton;
}

function updateBookmarkButton(bookmarked, bookmarkButton) {
    bookmarkButton.innerHTML = "";
    let icon = document.createElement("i");
    if (bookmarked) {
        icon.setAttribute("class", "fa-solid fa-bookmark fa-bounce bookmarked-icon");
        icon.setAttribute("style", "--fa-animation-iteration-count: 1;")
        bookmarkButton.appendChild(icon);
        bookmarkButton.appendChild(document.createTextNode(" Remove Bookmark"));
    } else {
        icon.setAttribute("class", "fa-solid fa-bookmark");
        bookmarkButton.appendChild(icon);
        bookmarkButton.appendChild(document.createTextNode(" Add to Bookmarks"));
    }
}

function createGymAddressElement(gym) {
    let gymResultAddress = document.createElement("p");
    gymResultAddress.setAttribute("class", "card-text");
    gymResultAddress.innerText = gym.formattedAddress;
    return gymResultAddress;
}

// Google's Nearby Search API can give us the opening and closing hours for a gym.
// TODO: Fix this so it adjusts for UTC time so that the hours are correct for the user's timezone
function createGymHoursElement(gym) {
    let gymHours = gym.regularOpeningHours.weekdayDescriptions;
    let gymHoursParagraphs = document.createElement("div");
    gymHoursParagraphs.setAttribute("class", "d-flex flex-column mb-3");
    gymHours.forEach(day => {
        let gymHoursParagraph = document.createElement("p");
        gymHoursParagraph.setAttribute("class", "card-text my-1");
        gymHoursParagraph.innerText = day;
        gymHoursParagraphs.appendChild(gymHoursParagraph);
    });
    return gymHoursParagraphs;
}

function createGymRatingElement(gym) {
    // console.log("Getting gym rating: " + gym.rating);

    let gymRatingDiv = document.createElement("div");
    gymRatingDiv.setAttribute("class", "d-flex align-items-center mb-3");
    let ratingText = document.createElement("p");
    ratingText.setAttribute("class", "card-text mb-0 me-2");
    ratingText.innerText = "Rating: ";
    gymRatingDiv.appendChild(ratingText);

    let numFullStars = Math.floor(gym.rating);
    let numHalfStars = Math.round(gym.rating - numFullStars);
    let numEmptyStars = 5 - numFullStars - numHalfStars;

    for (let j = 0; j < numFullStars; j++) {
        let fullStarIcon = document.createElement("i");
        fullStarIcon.setAttribute("class", "fa-solid fa-star gym-rating-star");
        gymRatingDiv.appendChild(fullStarIcon);
    }

    for (let j = 0; j < numHalfStars; j++) {
        let halfStarIcon = document.createElement("i");
        halfStarIcon.setAttribute("class", "fa-regular fa-star-half-stroke gym-rating-star");
        gymRatingDiv.appendChild(halfStarIcon);
    }

    for (let j = 0; j < numEmptyStars; j++) {
        let fullStarIcon = document.createElement("i");
        fullStarIcon.setAttribute("class", "fa-regular fa-star gym-rating-star");
        gymRatingDiv.appendChild(fullStarIcon);
    }

    let gymRatingText = document.createElement("p");
    gymRatingText.setAttribute("class", "card-text mb-0 ms-2");
    gymRatingText.innerText = `(${gym.rating})`;

    gymRatingDiv.appendChild(gymRatingText);
    return gymRatingDiv;
}

async function removeGymBookmark(gymPlaceID, bookmarkButton) {
    await deleteGymBookmark(gymPlaceID);
    bookmarkButton.setAttribute("class", "btn bookmark-gym-button");
    bookmarkButton.textContent = "";
    let unbookmarkedIcon = document.createElement("i");
    unbookmarkedIcon.setAttribute("class", "fa-solid fa-bookmark fa-bounce freshly-bookmarked-icon");
    bookmarkButton.appendChild(unbookmarkedIcon);
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
            'Content-Type': 'application/json'
        }
    })
    if (response.ok) {
        let result = await response.json()
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

    if (response.ok) {
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

    if (response.ok) {
        let result = await response.json()
        console.log(result)
        return result
    }
    else {
        console.log("Error: " + response.status)
    }
}

async function deleteGymBookmark(gymPlaceID) {
    let response = await fetch(`/api/gymUser/bookmark/delete/${gymPlaceID}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
    })
    if (response.ok) {
        console.log("Gym bookmark deleted successfully")
        return true
    }
    else {
        console.log("Something went wrong when deleting the bookmark: " + response.status)
        return false
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