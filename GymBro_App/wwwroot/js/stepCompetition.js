    const userSearchInput = document.getElementById('userSearch');
    const suggestionsBox = document.getElementById('suggestions');

    userSearchInput.addEventListener('input', async () => {
        const query = userSearchInput.value.trim();

        if (query.length < 2) {
            suggestionsBox.innerHTML = '';
            suggestionsBox.style.display = 'none';
            return;
        }

        const response = await fetch(`/api/StepCompetitionAPI/SearchUsers?username=${encodeURIComponent(query)}`);

        if (!response.ok) return;

        const users = await response.json();

        // Clear existing suggestions
        suggestionsBox.innerHTML = '';
        suggestionsBox.style.display = 'block';

        users.forEach(user => {
            const suggestionItem = document.createElement('button');
            suggestionItem.className = 'list-group-item list-group-item-action';
            suggestionItem.textContent = user;
            suggestionItem.type = 'button';

            // Add click behavior to populate the input and optionally trigger more logic
            suggestionItem.addEventListener('click', () => {
                userSearchInput.value = '';
                suggestionsBox.innerHTML = '';
                suggestionsBox.style.display = 'none';

                // Optional: add selected user to "Invited Users" list
                addInvitedUser(user);
            });

            suggestionsBox.appendChild(suggestionItem);
        });
    });



    // Prevent Enter key from doing anything
    userSearchInput.addEventListener('keydown', (event) => {
        if (event.key === 'Enter') {
            event.preventDefault();
        }
    });

    function addInvitedUser(username) {
        const invitedList = document.getElementById('invitedUserList');
        const invitedInputs = document.getElementById('invitedUserInputs');

        // Avoid duplicates
        if (document.getElementById(`invited-${username}`)) return;

        // Create list item with remove button
        const li = document.createElement('li');
        li.className = 'list-group-item d-flex justify-content-between align-items-center';
        li.id = `invited-${username}`;

        const span = document.createElement('span');
        span.textContent = username;

        const removeBtn = document.createElement('button');
        removeBtn.className = 'btn btn-sm btn-danger';
        removeBtn.textContent = '✕';
        removeBtn.addEventListener('click', () => {
            li.remove(); // remove from visual list
            const input = document.getElementById(`input-${username}`);
            if (input) input.remove(); // remove hidden input
        });

        li.appendChild(span);
        li.appendChild(removeBtn);
        invitedList.appendChild(li);

        // Add hidden input
        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'InvitedUsernames'; // must match backend
        input.value = username;
        input.id = `input-${username}`;
        invitedInputs.appendChild(input);
    }



    const openBtn = document.getElementById('openCompetitionFormBtn');
    const popup = document.getElementById('competitionPopup');
    const closeBtn = document.getElementById('closePopupBtn');

    openBtn.addEventListener('click', () => {
        popup.style.display = 'flex';
    });

    closeBtn.addEventListener('click', () => {
        popup.style.display = 'none';
    });

    window.addEventListener('click', (e) => {
        if (e.target === popup) {
            popup.style.display = 'none';
        }
    });


    document.getElementById('competitionForm').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission (page reload)
    
        const formData = new FormData(this); // Get all form data, including hidden inputs for invited users
    
        try {
            // Send the form data to the backend using fetch (AJAX request)
            const response = await fetch('/api/StepCompetitionAPI/StartCompetition', {
                method: 'POST',
                body: formData // This will include all form fields, including the invited user names
            });
    
            // Check if the response is okay (successful)
            if (!response.ok) {
                alert('Failed to start competition.');
                return;
            }
            const competitions = await response.json(); // This is the list returned from backend

            // Close the modal
            document.getElementById('competitionPopup').style.display = 'none';
            
            resetCompetitionForm();

            renderCompetitions(competitions);

        } catch (error) {
            console.error('Error:', error);
            alert('Error starting competition.');
        }
    });
    
    // Function to reset the competition form (clear the list of invited users and hidden inputs)
    function resetCompetitionForm() {
        // Clear the visual list of invited users
        document.getElementById('invitedUserList').innerHTML = '';
        
        // Clear the hidden inputs that will be submitted
        document.getElementById('invitedUserInputs').innerHTML = '';
    
        // Optionally reset the input search box
        document.getElementById('userSearch').value = '';
        document.getElementById('suggestions').innerHTML = '';
        document.getElementById('suggestions').style.display = 'none';
    }

    function renderCompetitions(competitions) {
        const container = document.getElementById('competitionListContainer');
        container.innerHTML = ''; // Clear old ones
    
        competitions.forEach(comp => {
            const card = document.createElement('div');
            card.className = 'card mb-3';
    
            const cardBody = document.createElement('div');
            cardBody.className = 'card-body';
    
            const title = document.createElement('h5');
            title.textContent = `Competition #${comp.competitionID}`;
    
            const dates = document.createElement('p');
            dates.textContent = `From ${new Date(comp.startDate).toLocaleDateString()} to ${new Date(comp.endDate).toLocaleDateString()}`;
    
            const participantList = document.createElement('ul');
            comp.participants.forEach(p => {
                const li = document.createElement('li');
                li.textContent = `${p.username} - ${p.steps} steps`;
                participantList.appendChild(li);
            });
    
            cardBody.appendChild(title);
            cardBody.appendChild(dates);
            cardBody.appendChild(participantList);
            card.appendChild(cardBody);
            container.appendChild(card);
        });
    }

    window.addEventListener('DOMContentLoaded', async () => {
        const response = await fetch('/api/StepCompetitionAPI/UserCompetitions');
        if (response.ok) {
            const competitions = await response.json();
            renderCompetitions(competitions);
        }
    });
    
    