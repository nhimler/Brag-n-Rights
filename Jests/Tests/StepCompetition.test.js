/**
 * @jest-environment jsdom
 */

const {
    addInvitedUser,
    renderCompetitions,
} = require('../../GymBro_App/wwwroot/js/stepCompetition');

beforeEach(() => {
    document.body.innerHTML = `
        <input id="userSearch" />
        <div id="suggestions"></div>
        <ul id="invitedUserList"></ul>
        <div id="invitedUserInputs"></div>
        <button id="openCompetitionFormBtn"></button>
        <div id="competitionPopup"></div>
        <button id="closePopupBtn"></button>
        <form id="competitionForm"></form>
        <div id="competitionListContainer"></div>
    `;
});

describe('addInvitedUser', () => {
    it('adds a user to the DOM list and input', () => {
        addInvitedUser('alice');

        const listItem = document.getElementById('invited-alice');
        const input = document.getElementById('input-alice');

        expect(listItem).not.toBeNull();
        expect(input).not.toBeNull();
        expect(input.value).toBe('alice');
    });

    it('does not add the same user twice', () => {
        addInvitedUser('bob');
        addInvitedUser('bob');

        const items = document.querySelectorAll('#invitedUserList li');
        expect(items.length).toBe(1);
    });

    it('removes the user when the remove button is clicked', () => {
        addInvitedUser('carol');

        const removeBtn = document.querySelector('#invited-carol button');
        removeBtn.click();

        expect(document.getElementById('invited-carol')).toBeNull();
        expect(document.getElementById('input-carol')).toBeNull();
    });
});

describe('renderCompetitions', () => {
    it('renders competition cards with correct info', () => {
        const competitions = [
            {
                competitionID: 1,
                startDate: '2025-05-01',
                endDate: '2025-05-31',
                participants: [
                    { username: 'alice', steps: 1000 },
                    { username: 'bob', steps: 1500 },
                ]
            }
        ];

        renderCompetitions(competitions);

        const cards = document.querySelectorAll('.card');
        expect(cards.length).toBe(1);

        const title = cards[0].querySelector('h5');
        expect(title.textContent).toBe('Competition #1');

        const dateText = cards[0].querySelector('p').textContent;
        expect(dateText).toContain('From');
        expect(dateText).toContain('to');

        const participants = cards[0].querySelectorAll('li');
        expect(participants.length).toBe(2);
        expect(participants[0].textContent).toBe('alice - 1000 steps');
        expect(participants[1].textContent).toBe('bob - 1500 steps');
    });

    it('clears previous competitions before rendering', () => {
        // Pre-populate container with dummy content
        const container = document.getElementById('competitionListContainer');
        container.innerHTML = '<div class="card">Old Content</div>';

        renderCompetitions([]);

        expect(container.innerHTML).toBe('');
    });
});

describe('resetCompetitionForm', () => {
    const { resetCompetitionForm } = require('../../GymBro_App/wwwroot/js/stepCompetition');

    it('clears invited users list, inputs, search field, and suggestions', () => {
        // Populate DOM elements with fake content to simulate a filled-out form
        document.getElementById('invitedUserList').innerHTML = '<li>MockUser</li>';
        document.getElementById('invitedUserInputs').innerHTML = '<input type="hidden" />';
        document.getElementById('userSearch').value = 'someone';
        document.getElementById('suggestions').innerHTML = '<div>Suggestion</div>';
        document.getElementById('suggestions').style.display = 'block';

        // Call the function
        resetCompetitionForm();

        // Assertions
        expect(document.getElementById('invitedUserList').innerHTML).toBe('');
        expect(document.getElementById('invitedUserInputs').innerHTML).toBe('');
        expect(document.getElementById('userSearch').value).toBe('');
        expect(document.getElementById('suggestions').innerHTML).toBe('');
        expect(document.getElementById('suggestions').style.display).toBe('none');
    });
});
