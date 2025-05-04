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

      container.innerHTML = '';
      exercises.forEach((ex, idx) => {
        container.innerHTML += `
          <div class="exercise-detail mb-4">
            <div class="row">
              <div class="col-6 d-flex justify-content-center align-items-center">
                <img src="${ex.gifUrl}" class="img-fluid" alt="${ex.name}">
              </div>
              <div class="col-6">
                <h5 class="mb-2">${ex.name}</h5>
                <button class="btn btn-sm btn-outline-primary mb-2"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target="#details-${idx}"
                        aria-expanded="false"
                        aria-controls="details-${idx}">
                  Show more details
                </button>
                <div class="collapse" id="details-${idx}">
                  <p><b>Equipment:</b> ${ex.equipment.charAt(0).toUpperCase() + ex.equipment.slice(1)}</p>
                  <p><b>Body Part:</b> ${ex.bodyPart.charAt(0).toUpperCase() + ex.bodyPart.slice(1)}</p>
                  <p><b>Primary Target Muscle:</b> ${ex.target.charAt(0).toUpperCase() + ex.target.slice(1)}</p>
                  <p><b>Secondary Target Muscle(s):</b> ${
                    ex.secondaryMuscles.length
                      ? ex.secondaryMuscles
                          .map(m => m.charAt(0).toUpperCase() + m.slice(1))
                          .join(', ')
                      : 'None'
                  }</p>
                  <h6 class="mt-3">Instructions:</h6>
                  <ol class="list-group list-group-numbered">
                    ${ex.instructions.map(i => `<li class="list-group-item">${i}</li>`).join('')}
                  </ol>
                </div>
              </div>
            </div>
          </div>`;
      });
    });
  });
});