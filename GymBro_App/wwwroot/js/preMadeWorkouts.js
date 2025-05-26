// wwwroot/js/preMadeWorkouts.js

document.addEventListener('DOMContentLoaded', async () => {
  // 1) Start from the data the view put on window
  const plans = window.preMadePlansData || [];
  
  // 2) Hydrate each plan with full exercise details
  for (const plan of plans) {
    const detailTasks = plan.Exercises.map(apiId =>
      fetch(`/api/exercises/id/${encodeURIComponent(apiId)}`)
        .then(r => {
          if (!r.ok) throw new Error(`Exercise ${apiId} not found`);
          return r.json();
        })
        .then(arr => arr[0])
    );
    plan.fullExercises = await Promise.all(detailTasks);
  }

  // 3) Render into the DOM
  renderPreMadePlans(plans);
});

// Helper to title-case words
function capitalizeFirstLetterWord(word) {
  return word.charAt(0).toUpperCase() + word.slice(1).toLowerCase();
}

function createExerciseCard(ex) {
  const name = ex.name
    .split(' ')
    .map(w => capitalizeFirstLetterWord(w))
    .join(' ');

  const col = document.createElement('div');
  col.className = 'col-lg-3 col-md-4 col-sm-6 mb-3';

  const card = document.createElement('div');
  card.className = 'card exercise-card h-100';

  const img = document.createElement('img');
  img.src = ex.gifUrl;
  img.alt = `${name} Gif`;
  img.className = 'card-img-top';
  card.appendChild(img);

  const body = document.createElement('div');
  body.className = 'card-body d-flex flex-column';

  const title = document.createElement('h5');
  title.className = 'card-title';
  title.textContent = name;
  body.appendChild(title);

  const badges = document.createElement('div');
  badges.className = 'd-flex justify-content-evenly align-items-center mb-3';
  const bc = 'badge rounded-pill bg-info mb-1';
  badges.innerHTML = `
    <span class="${bc}">${capitalizeFirstLetterWord(ex.equipment)}</span>
    <span class="${bc}">${capitalizeFirstLetterWord(ex.bodyPart)}</span>
    <span class="${bc}">${capitalizeFirstLetterWord(ex.target)}</span>
  `;
  body.appendChild(badges);

  const btnWrap = document.createElement('div');
  btnWrap.className = 'mt-auto text-center';
  const btn = document.createElement('button');
  btn.type = 'button';
  btn.className = 'btn btn-primary btn-sm rounded-pill';
  btn.textContent = 'View Details';
  btn.setAttribute('data-bs-toggle', 'modal');
  btn.setAttribute('data-bs-target', '#exerciseModal');
  btn.addEventListener('click', () => populateExerciseModal(ex));
  btnWrap.appendChild(btn);
  body.appendChild(btnWrap);

  card.appendChild(body);
  col.appendChild(card);
  return col;
}

function renderPreMadePlans(plans) {
  const container = document.getElementById('PreMadeWorkoutList');
  if (!container) return console.error('No #PreMadeWorkoutList found');
  container.innerHTML = '';

  plans.forEach(plan => {
    // 1) Header + caret toggle (left-aligned)
    const planItem = document.createElement('div');
    planItem.className = 'plan-item mb-4';

    const header = document.createElement('div');
    header.className = 'd-flex align-items-center';
    header.style.cursor = 'pointer';

    const title = document.createElement('h2');
    title.className = 'mb-0';
    title.textContent = plan.PlanName;
    header.appendChild(title);

    const caret = document.createElement('span');
    caret.textContent = '▶';
    caret.style.marginLeft = '8px';
    caret.style.transition = 'transform 0.2s';
    header.appendChild(caret);

    planItem.appendChild(header);

    // 2) Collapsible panel
    const panel = document.createElement('div');
    panel.style.display = 'none';
    panel.className = 'mt-2';

    const useBtn = document.createElement('button');
    useBtn.className = 'btn btn-success btn-sm mb-3';
    useBtn.textContent = 'Use this plan';
    useBtn.addEventListener('click', () => usePreMadePlan(plan));
    panel.appendChild(useBtn);

    const row = document.createElement('div');
    row.className = 'row';
    plan.fullExercises.forEach(ex => row.appendChild(createExerciseCard(ex)));
    panel.appendChild(row);

    planItem.appendChild(panel);

    header.addEventListener('click', () => {
      const open = panel.style.display === 'block';
      panel.style.display = open ? 'none' : 'block';
      caret.textContent = open ? '▶' : '▼';
    });

    container.appendChild(planItem);
  });
}

async function usePreMadePlan(plan) {
  const payload = {
    PlanName:       plan.PlanName,
    Difficulty:     plan.DifficultyLevel,
    ExerciseApiIds: plan.Exercises
  };

  try {
    const res = await fetch('/api/workouts/applyTemplate', {
      method:  'POST',
      headers: { 'Content-Type': 'application/json' },
      body:    JSON.stringify(payload)
    });
    if (!res.ok) throw new Error(await res.text());
    window.location.href = '/Workouts';
  }
  catch (err) {
    console.error('usePreMadePlan error:', err);
    alert('Failed to send plan: ' + err.message);
  }
}
