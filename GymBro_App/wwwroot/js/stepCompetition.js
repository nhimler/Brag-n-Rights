function addInvitedUser(username) {
    const invitedList = document.getElementById('invitedUserList');
    const invitedInputs = document.getElementById('invitedUserInputs');

    if (document.getElementById(`invited-${username}`)) return;

    const li = document.createElement('li');
    li.className = 'list-group-item d-flex justify-content-between align-items-center';
    li.id = `invited-${username}`;

    const span = document.createElement('span');
    span.textContent = username;

    const removeBtn = document.createElement('button');
    removeBtn.className = 'btn btn-sm btn-danger';
    removeBtn.textContent = '✕';
    removeBtn.addEventListener('click', () => {
        li.remove();
        const input = document.getElementById(`input-${username}`);
        if (input) input.remove();
    });

    li.appendChild(span);
    li.appendChild(removeBtn);
    invitedList.appendChild(li);

    const input = document.createElement('input');
    input.type = 'hidden';
    input.name = 'InvitedUsernames';
    input.value = username;
    input.id = `input-${username}`;
    invitedInputs.appendChild(input);
}

function resetCompetitionForm() {
    document.getElementById('invitedUserList').innerHTML = '';
    document.getElementById('invitedUserInputs').innerHTML = '';
    document.getElementById('userSearch').value = '';
    document.getElementById('suggestions').innerHTML = '';
    document.getElementById('suggestions').style.display = 'none';
}

function renderCompetitions(competitions) {
    const container = document.getElementById('competitionListContainer');
    container.innerHTML = '';

    competitions.forEach(comp => {
        const card = document.createElement('div');
        card.className = 'card mb-3';

        const cardBody = document.createElement('div');
        cardBody.className = 'card-body';

        const title = document.createElement('h5');
        title.textContent = `7 Day Step Competition🔥`;

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

// DOM interaction only runs after DOM is ready
window.addEventListener('DOMContentLoaded', () => {
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

        suggestionsBox.innerHTML = '';
        suggestionsBox.style.display = 'block';

        users.forEach(user => {
            const suggestionItem = document.createElement('button');
            suggestionItem.className = 'list-group-item list-group-item-action';
            suggestionItem.textContent = user;
            suggestionItem.type = 'button';

            suggestionItem.addEventListener('click', () => {
                userSearchInput.value = '';
                suggestionsBox.innerHTML = '';
                suggestionsBox.style.display = 'none';

                addInvitedUser(user);
            });

            suggestionsBox.appendChild(suggestionItem);
        });
    });

    userSearchInput.addEventListener('keydown', (event) => {
        if (event.key === 'Enter') {
            event.preventDefault();
        }
    });

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
        event.preventDefault();
        const formData = new FormData(this);

        try {
            const response = await fetch('/api/StepCompetitionAPI/StartCompetition', {
                method: 'POST',
                body: formData
            });

            if (!response.ok) {
                alert('Failed to start competition.');
                return;
            }

            const competitions = await response.json();
            document.getElementById('competitionPopup').style.display = 'none';
            resetCompetitionForm();
            renderCompetitions(competitions);

        } catch (error) {
            console.error('Error:', error);
            alert('Error starting competition.');
        }
    });

    // Load existing competitions
    (async () => {
        const response = await fetch('/api/StepCompetitionAPI/UserCompetitions');
        if (response.ok) {
            const competitions = await response.json();
            renderCompetitions(competitions);
        }
    })();
});

export { addInvitedUser, resetCompetitionForm, renderCompetitions };
