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

      exercises.forEach((ex, idx) => {
        container.innerHTML += `
          <div class="exercise-detail mb-4">
            <div class="row">
              <div class="col-6 d-flex justify-content-center align-items-center">
                <img src="${ex.gifUrl}" class="img-fluid" alt="${ex.name}">
              </div>
              <div class="col-6">
                <h5>${ex.name}</h5>
                <div class="mb-2">
                  <label>Sets:
                    <input type="number"
                           class="form-control sets-input"
                           data-apiid="${ex.id}"
                           value="${ex.sets}" />
                  </label>
                </div>
                <div class="mb-2">
                  <label>Reps:
                    <input type="number"
                           class="form-control reps-input"
                           data-apiid="${ex.id}"
                           value="${ex.reps}" />
                  </label>
                </div>
                <div class="mb-2">
                  <button class="btn btn-success btn-sm save-changes-btn"
                          type="button"
                          data-apiid="${ex.id}">
                    Save Changes
                  </button>
                </div>
                <button class="btn btn-sm btn-outline-primary mb-2"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target="#details-${idx}">
                  Show more details
                </button>
                <div class="collapse" id="details-${idx}">
                  <p><b>Equipment:</b> ${capitalize(ex.equipment)}</p>
                  <p><b>Body Part:</b> ${capitalize(ex.bodyPart)}</p>
                  <p><b>Primary Target:</b> ${capitalize(ex.target)}</p>
                  <p><b>Secondary:</b> ${
                    ex.secondaryMuscles.length
                      ? ex.secondaryMuscles.map(capitalize).join(', ')
                      : 'None'
                  }</p>
                  <h6>Instructions:</h6>
                  <ol class="list-group list-group-numbered mb-3">
                    ${ex.instructions.map(i => `<li>${i}</li>`).join('')}
                  </ol>
                </div>
              </div>
            </div>
          </div>`;
      });

      container.querySelectorAll('.save-changes-btn').forEach(btn => {
        btn.addEventListener('click', async () => {
          const apiId = btn.dataset.apiid;
          const sets  = container.querySelector(`.sets-input[data-apiid="${apiId}"]`).value;
          const reps  = container.querySelector(`.reps-input[data-apiid="${apiId}"]`).value;
          const res = await fetch('/api/Workouts/Exercise', {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              planId: parseInt(planId),
              apiId,
              sets: parseInt(sets),
              reps: parseInt(reps)
            })
          });
          if (res.ok) {
            btn.textContent = 'Saved';
            btn.classList.replace('btn-success','btn-secondary');
          } else {
            btn.textContent = 'Error';
          }
        });
      });

      if (!document.getElementById('saveAllExercisesBtn')) {
        const saveAllBtn = document.createElement('button');
        saveAllBtn.id = 'saveAllExercisesBtn';
        saveAllBtn.className = 'btn btn-primary mt-3 float-end';
        saveAllBtn.textContent = 'Save All Changes';
        container.appendChild(saveAllBtn);

        saveAllBtn.addEventListener('click', async () => {
          const updateTasks = Array.from(container.querySelectorAll('.exercise-detail')).map(detail => {
            const apiId = detail.querySelector('.sets-input').dataset.apiid;
            const sets = parseInt(detail.querySelector('.sets-input').value);
            const reps = parseInt(detail.querySelector('.reps-input').value);
            return fetch('/api/Workouts/Exercise', {
              method: 'PUT',
              headers: { 'Content-Type': 'application/json' },
              body: JSON.stringify({ planId: parseInt(planId), apiId, sets, reps })
            });
          });

          const results = await Promise.all(updateTasks);
          if (results.every(r => r.ok)) {
            const modalEl = document.getElementById('exercisesModal');
            const modal = bootstrap.Modal.getInstance(modalEl) 
                       || bootstrap.Modal.getOrCreateInstance(modalEl);
            modal.hide();
          } else {
            console.error('One or more saves failed.');
          }
        });
      }

    });
  });
});

function capitalize(str) {
  return str.charAt(0).toUpperCase() + str.slice(1);
}