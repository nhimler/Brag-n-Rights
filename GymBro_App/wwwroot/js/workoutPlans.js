document.addEventListener('DOMContentLoaded', () => {
  document.querySelectorAll('.view-exercises-btn').forEach(btn => {
    btn.addEventListener('click', async () => {
      const planId = btn.dataset.planId;
      const resp = await fetch(`/api/Workouts/${planId}/Exercises`);
      const exercises = resp.ok ? await resp.json() : [];
      const container = document.getElementById('exercisesModalBody');
      container.innerHTML = '';

      if (!exercises.length) {
        container.innerHTML = '<p class="text-center">No exercises found.</p>';
        return;
      }

      exercises.forEach(ex => {
        const card = document.createElement('div');
        card.className = 'card mb-3';
        card.innerHTML = `
          <img src="${ex.gifUrl}" class="card-img-top" alt="${ex.name}">
          <div class="card-body">
            <h5 class="card-title">${ex.name}</h5>
            <ol class="list-group list-group-numbered">
              ${ex.instructions.map(i => `<li class="list-group-item">${i}</li>`).join('')}
            </ol>
          </div>`;
        container.appendChild(card);
      });
    });
  });
});