// Toggle past competitions
const compButton = document.getElementById('ViewPastCompetitions');
const pastContainerId = 'pastCompetitionsContainer';
let pastVisible = false;

compButton.addEventListener('click', async () => {
  const container = document.getElementById(pastContainerId);

  if (pastVisible) {
    // hide
    container.innerHTML = '';
    compButton.textContent = 'View Past Competitions';
    pastVisible = false;
    return;
  }

  try {
    const res = await fetch('/api/StepCompetitionAPI/PastCompetitions');
    if (!res.ok) throw new Error('Fetch failed');

    const data = await res.json();
    if (data.length === 0) {
      container.innerHTML = '<p>No past competitions found for this user.</p>';
    } else {
      // note: showLeaveButton = false here
      renderCompetitions(data, pastContainerId, false);
    }

    compButton.textContent = 'Hide Past Competitions';
    pastVisible = true;

  } catch (err) {
    console.error(err);
    alert('Could not load past competitions.');
  }
});


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


function renderCompetitions(competitions, containerId, showLeaveButton) {
    const container = document.getElementById(containerId);
    container.innerHTML = '';
  
    competitions.forEach(comp => {
      // Card wrapper
      const card = document.createElement('div');
      card.className = 'card mb-3 CompetitionCard';
      card.id = `competition-${comp.competitionID}`;
  
      const cardBody = document.createElement('div');
      cardBody.className = 'card-body';
  
      // 1) Title
      const title = document.createElement('h5');
      title.textContent = '7 Day Step Competition 🔥';
  
      // 2) Status line
      //    find the participant with the max steps
      const top = comp.participants.reduce(
        (best, curr) => curr.steps > best.steps ? curr : best,
        comp.participants[0]
      );
  
      const status = document.createElement('p');
      if (comp.isActive) {
        // Find second top steps (if any)
        let secondTopSteps = null;
        if (comp.participants.length > 1) {
          // Sort descending by steps
          const sorted = [...comp.participants].sort((a, b) => b.steps - a.steps);
          secondTopSteps = sorted[1].steps;
        }
        if (secondTopSteps !== null && top.steps !== secondTopSteps) {
          status.textContent = `${top.username} is winning with ${top.steps} steps!`;
          status.className = 'Winning';
        } else {
          status.textContent = '';
          status.className = '';
        }
      } else {
        status.textContent = `${top.username} won with ${top.steps} steps! 🎉`;
        status.className = 'Won';
      }
  
      // 3) Date range
      const dates = document.createElement('p');
      dates.textContent =
        `From ${new Date(comp.startDate).toLocaleDateString()} `
        + `to ${new Date(comp.endDate).toLocaleDateString()}`;
  
      // 4) Participant list
      const ol = document.createElement('ol');
      comp.participants.forEach(p => {
        const li = document.createElement('li');
        li.textContent = `${p.username} – ${p.steps} steps`;
        ol.appendChild(li);
      });
  
      // assemble
      cardBody.appendChild(title);
      cardBody.appendChild(status);
      cardBody.appendChild(dates);
      cardBody.appendChild(ol);
  
      // 5) optional leave‐button
      if (showLeaveButton) {
        const leaveBtn = createLeaveButton(comp.competitionID);
        cardBody.appendChild(leaveBtn);
      }
  
      card.appendChild(cardBody);
      container.appendChild(card);
    });
  }
  
  

function createLeaveButton(competitionID) {
    const LeaveBtn = document.createElement('button');
    LeaveBtn.className = 'btn btn-danger';
    LeaveBtn.textContent = 'Leave Competition';
    LeaveBtn.setAttribute('competition-id', competitionID);

    LeaveBtn.addEventListener('click', async () => {
        // Use template literals to insert the competitionID into the URL
        const response = await fetch(`/api/StepCompetitionAPI/LeaveCompetition/${competitionID}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            alert('Failed to leave competition.');
            return;
        } else {
            // If successful, remove the competition card from the DOM
            document.getElementById(`competition-${competitionID}`).remove();
        }
    });

    return LeaveBtn;
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
            renderCompetitions(competitions, 'competitionListContainer', true);

        } catch (error) {
            console.error('Error:', error);
            alert('Error starting competition.');
        }
    });

    // Load existing competitions
    (async () => {
        const res = await fetch('/api/StepCompetitionAPI/UserCompetitions');
        if (res.ok) {
          const data = await res.json();
          renderCompetitions(data, 'competitionListContainer', true);
        } else {
          console.error('Failed to load competitions', await res.text());
        }

        const recentRes = await fetch('/api/StepCompetitionAPI/RecentlyEndedCompetitions');
        if (recentRes.ok) {
          const recentData = await recentRes.json();
          if (recentData.length === 0) return;           // nothing to show
          const wrapper = document.getElementById('RecentlyEndedComp');
          wrapper.innerHTML = `
            <h5>Recently Ended Competitions</h5><br>
            <div id="recentCompetitionList" class="mt-4"></div>
          `;
      
          renderCompetitions(recentData, 'recentCompetitionList', false);
        } else {
          console.error('Failed to load recently ended competitions', await recentRes.text());
        }
      })();
});

export { addInvitedUser, resetCompetitionForm, renderCompetitions };
