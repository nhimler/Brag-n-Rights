let button = document.getElementById('ViewPastMedals');
let container = document.getElementById('pastMedalsContainer');
let medalsVisible = false;

button.addEventListener('click', async function () {
    if (medalsVisible) {
        container.innerHTML = '';
        button.textContent = 'View Past Medals';
        medalsVisible = false;
        return;
    }

    try {
        const response = await fetch('/api/AwardedMedalAPI/GetAwardedMedals');
        if (!response.ok) {
            throw new Error("Failed to fetch past medals.");
        }

        const medals = await response.json();

                // If no medals found, display a friendly message.
        if (medals.length === 0) {
            container.innerHTML = '<p>No medals found for this user.</p>';
            button.textContent = 'Hide Past Medals';
            medalsVisible = true;
            return;
        }

        // Group by Month/Year
        const grouped = medals.reduce((groups, medal) => {
            const key = formatMonthYear(medal.earnedDate);
            if (!groups[key]) {
                groups[key] = [];
            }
            groups[key].push(medal);
            return groups;
        }, {});

        container.innerHTML = '';

        for (const [month, group] of Object.entries(grouped)) {
            const section = document.createElement('div'); // New wrapper
            section.classList.add('medal-month-section');
        
            const header = document.createElement('h2');
            header.textContent = month;
            header.classList.add('medal-month-header');
            section.appendChild(header);
        
            const groupContainer = document.createElement('div');
            groupContainer.classList.add('medal-container');
        
            group.forEach(medal => {
                const medalCard = document.createElement('div');
                medalCard.classList.add('medal-card', 'unlocked');
                medalCard.innerHTML = `
                    <img src="${medal.medalImage.startsWith('/') ? medal.medalImage : '/' + medal.medalImage}" alt="${medal.medalName}" class="medal-image" />
                    <h3>${medal.medalName}</h3>
                    <p class="medal-earned-date"><strong>Earned:</strong> ${medal.earnedDate}</p>
                `;
                groupContainer.appendChild(medalCard);
            });
        
            section.appendChild(groupContainer);
            container.appendChild(section);
        }
        

        button.textContent = 'Hide Past Medals';
        medalsVisible = true;

    } catch (error) {
        console.error("Error loading medals:", error);
        alert("Something went wrong while loading past medals.");
    }
});

function formatMonthYear(dateStr) {
    const date = new Date(dateStr);
    return date.toLocaleDateString('en-US', { month: 'long', year: 'numeric' });
}
